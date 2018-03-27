using System.Collections.Generic;
using System.ComponentModel;

namespace UltimateWater.Internal
{
    using System;
    using UnityEngine;

    [Serializable]
    public class WaterCameraSubmersion
    {
        #region Public Variables
        public SubmersionState State
        {
            get
            {
                var water = _Camera._ContainingWater;

                // if we didn't found any water
                if (water == null)
                {
                    return SubmersionState.None;
                }

                // for non-boundless water always use Partial state as determining this would be too costly
                if (!water.Volume.Boundless)
                {
                    return SubmersionState.Partial;
                }

                // check with the evaluator
                return Evaluate();
            }
        }
        #endregion Public Variables

        #region Public Methods
        public void OnEnable(WaterCamera camera)
        {
            _Camera = camera;
        }

        public void OnDisable()
        {
        }

        public void OnDrawGizmos()
        {
            var camera = _Camera.CameraComponent;
            var height = CalculateNearPlaneHeight(camera);

            CreatePlanePoints(camera, _Subdivisions, _Points);

            Gizmos.color = new Color(0, 1, 1, 0.2f);
            for (int i = 0; i < _Points.Count; ++i)
            {
                Gizmos.DrawSphere(_Points[i], height * _Radius);
            }
        }

        public void Create()
        {
            if (_Samples != null)
            {
                Destroy();
            }

            var water = _Camera._ContainingWater;
            if (water != null)
            {
                var camera = _Camera.CameraComponent;
                const float precision = 0.4f;

                _Samples = new List<WaterSample>();
                CreatePlanePoints(camera, _Subdivisions, _Points);

                for (int i = 0; i < _Points.Count; ++i)
                {
                    var sample = new WaterSample(water, WaterSample.DisplacementMode.Height, precision);
                    sample.Start(_Points[i]);

                    _Samples.Add(sample);
                }
            }
        }
        public void Destroy()
        {
            if (_Samples != null)
            {
                foreach (var sample in _Samples)
                {
                    sample.Stop();
                }
                _Samples.Clear();
                _Samples = null;
            }

            _Points.Clear();
        }

        public void OnValidate()
        {
            if (_Subdivisions < 0)
            {
                _Subdivisions = 0;
            }
            if (_Radius < 0.0f)
            {
                _Radius = 0.0f;
            }
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField, Range(0.0f, 2.0f)] private float _Radius = 1.0f;
        [SerializeField] private int _Subdivisions;
        #endregion Inspector Variables

        #region Private Variables
        private WaterCamera _Camera;
        private List<WaterSample> _Samples;
        private readonly List<Vector3> _Points = new List<Vector3>();
        #endregion Private Variables

        #region Private Methods
        private SubmersionState Evaluate()
        {
            var count = 0;
            var height = CalculateNearPlaneHeight(_Camera.CameraComponent);
            var offset = height * _Radius;

            // update points positions
            CreatePlanePoints(_Camera.CameraComponent, _Subdivisions, _Points);

            for (int i = 0; i < _Samples.Count; ++i)
            {
                var point = _Points[i];
                var water = _Samples[i].GetAndReset(point);

                if (point.y + offset <= water.y)
                {
                    count++;
                }
                if ((point.y + offset >= water.y) && (point.y - offset <= water.y))
                {
                    return SubmersionState.Partial;
                }
            }

            // if all the samples are submerged
            if (count == _Samples.Count)
            {
                return SubmersionState.Full;
            }

            // if none of the samples are submerged
            if (count == 0)
            {
                return SubmersionState.None;
            }

            // if some of the samples are submerged
            return SubmersionState.Partial;
        }

        private static void CreatePlanePoints(Camera camera, int subdivisions, List<Vector3> result)
        {
            result.Clear();

            var height = CalculateNearPlaneHeight(camera);
            var width = height * camera.aspect;

            var center = GetNearPlaneCenter(camera);

            if (subdivisions == 0)
            {
                result.Add(center);
                return;
            }
            subdivisions -= 1;

            var halfUp = 0.5f * height * camera.transform.up;
            var halfRight = 0.5f * width * camera.transform.right;

            var ld = center - halfUp - halfRight;
            var lu = center + halfUp - halfRight;
            var ru = center + halfUp + halfRight;
            var rd = center - halfUp + halfRight;

            result.Add(ld);
            result.Add(lu);
            result.Add(ru);
            result.Add(rd);

            for (int i = 0; i < subdivisions; ++i)
            {
                float t = (i + 1.0f) / (subdivisions + 1.0f);

                result.Add(Vector3.Lerp(ld, lu, t));
                result.Add(Vector3.Lerp(lu, ru, t));
                result.Add(Vector3.Lerp(ru, rd, t));
                result.Add(Vector3.Lerp(rd, ld, t));
            }
        }
        #endregion Private Methods

        #region Helper Methods
        private static Vector3 GetNearPlaneCenter(Camera camera)
        {
            return camera.transform.position + camera.transform.forward * camera.nearClipPlane;
        }
        private static float CalculateNearPlaneHeight(Camera camera)
        {
            return 2.0f * camera.nearClipPlane * Mathf.Tan(0.5f * camera.fieldOfView * Mathf.Deg2Rad);
        }
        #endregion Helper Methods
    }
}