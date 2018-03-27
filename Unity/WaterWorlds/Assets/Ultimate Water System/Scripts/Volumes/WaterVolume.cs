namespace UltimateWater
{
    using System.Collections.Generic;
    using UnityEngine;
    using Internal;

    [System.Serializable]
    public class WaterVolume : WaterModule
    {
        #region Inspector Variables
        [Tooltip("Makes water volume be infinite in horizontal directions and infinitely deep." +
                 " It is still reduced by subtractive colliders tho. Check that if this is an ocean, sea or if this water spans through most of the scene. If you will uncheck this, you will need to add some child colliders to define where water should display.")]
        [SerializeField]
        private bool _Boundless = true;
        #endregion Inspector Variables

        #region Private Variables
        private bool _CollidersAdded;
        private Water _Water;

        private readonly List<WaterVolumeAdd> _Volumes = new List<WaterVolumeAdd>();
        private readonly List<WaterVolumeSubtract> _Subtractors = new List<WaterVolumeSubtract>();
        #endregion Private Variables

        #region Public Variables
        public bool Boundless
        {
            get { return _Boundless; }
        }

        public bool HasRenderableAdditiveVolumes
        {
            get
            {
                for (int i = _Volumes.Count - 1; i >= 0; --i)
                {
                    if (_Volumes[i].RenderMode != WaterVolumeRenderMode.None)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
        #endregion Public Variables

        #region Public Methods
        public List<WaterVolumeAdd> GetVolumesDirect()
        {
            return _Volumes;
        }

        public List<WaterVolumeSubtract> GetSubtractiveVolumesDirect()
        {
            return _Subtractors;
        }

        public void Dispose()
        {
        }

        public void EnableRenderers(bool forBorderRendering = false)
        {
            for (int i = 0; i < _Volumes.Count; ++i)
            {
                _Volumes[i].EnableRenderers(forBorderRendering);
            }

            for (int i = 0; i < _Subtractors.Count; ++i)
            {
                _Subtractors[i].EnableRenderers(forBorderRendering);
            }
        }

        public void DisableRenderers()
        {
            for (int i = 0; i < _Volumes.Count; ++i)
            {
                _Volumes[i].DisableRenderers();
            }

            for (int i = 0; i < _Subtractors.Count; ++i)
            {
                _Subtractors[i].DisableRenderers();
            }
        }
        public bool IsPointInside(Vector3 point, WaterVolumeSubtract[] exclusions, float radius = 0.0f)
        {
            for (int i = _Subtractors.Count - 1; i >= 0; --i)
            {
                var volume = _Subtractors[i];
                if (volume.EnablePhysics && volume.IsPointInside(point) && !Contains(exclusions, volume))
                {
                    return false;
                }
            }

            if (_Boundless)
            {
                return (point.y - radius) <= (_Water.transform.position.y + _Water.MaxVerticalDisplacement);
            }

            for (int i = _Volumes.Count - 1; i >= 0; --i)
            {
                var volume = _Volumes[i];
                if (volume.EnablePhysics && volume.IsPointInside(point))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion Public Methods

        #region Private Methods
        internal override void Start(Water water)
        {
            _Water = water;
        }

        internal override void Enable()
        {
            if (!_CollidersAdded && Application.isPlaying)
            {
                var colliders = _Water.GetComponentsInChildren<Collider>(true);

                for (int i = 0; i < colliders.Length; ++i)
                {
                    var collider = colliders[i];
                    var volumeSubtract = collider.GetComponent<WaterVolumeSubtract>();

                    if (volumeSubtract == null)
                    {
                        var volumeAdd = collider.GetComponent<WaterVolumeAdd>();
                        AddVolume(volumeAdd != null ? volumeAdd : collider.gameObject.AddComponent<WaterVolumeAdd>());
                    }
                }

                _CollidersAdded = true;
            }

            EnableRenderers();
        }

        internal override void Disable()
        {
            Dispose();
            DisableRenderers();
        }

        internal void AddVolume(WaterVolumeAdd volume)
        {
            _Volumes.Add(volume);
            volume.AssignTo(_Water);
        }

        internal void RemoveVolume(WaterVolumeAdd volume)
        {
            _Volumes.Remove(volume);
        }

        internal void AddSubtractor(WaterVolumeSubtract volume)
        {
            _Subtractors.Add(volume);
            volume.AssignTo(_Water);
        }

        internal void RemoveSubtractor(WaterVolumeSubtract volume)
        {
            _Subtractors.Remove(volume);
        }

        private static bool Contains(WaterVolumeSubtract[] array, WaterVolumeSubtract element)
        {
            if (array == null)
            {
                return false;
            }
            for (int i = array.Length - 1; i >= 0; --i)
            {
                if (array[i] == element)
                {
                    return true;
                }
            }

            return false;
        }

        internal bool IsPointInsideMainVolume(Vector3 point, float radius = 0.0f)
        {
            if (_Boundless)
            {
                return (point.y - radius) <= (_Water.transform.position.y + _Water.MaxVerticalDisplacement);
            }

            return false;
        }
        #endregion Private Methods

        #region Unused Inherited
        internal override void Update()
        {
        }

        internal override void Destroy()
        {
        }

        internal override void Validate()
        {
        }
        #endregion Unused Inherited
    }
}