namespace UltimateWater
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Serialization;
    using Internal;

    /// <summary>
    ///     Finds out in which water volume this GameObject is contained and raises events on enter/leave.
    /// </summary>
    public sealed class WaterVolumeProbe : MonoBehaviour
    {
        #region Public Variables
        public Water CurrentWater
        {
            get { return _CurrentWater; }
        }

        public UnityEvent Enter
        {
            get { return _Enter != null ? _Enter : (_Enter = new UnityEvent()); }
        }

        public UnityEvent Leave
        {
            get { return _Leave != null ? _Leave : (_Leave = new UnityEvent()); }
        }
        #endregion Public Variables

        #region Public Methods
        public static WaterVolumeProbe CreateProbe(Transform target, float size = 0.0f)
        {
            var go = new GameObject("Water Volume Probe") { hideFlags = HideFlags.HideAndDontSave };
            go.transform.position = target.position;
            go.layer = WaterProjectSettings.Instance.WaterCollidersLayer;                               // TransparentFX layer by default

            var sphereCollider = go.AddComponent<SphereCollider>();
            sphereCollider.radius = size;
            sphereCollider.isTrigger = true;

            var rigidBody = go.AddComponent<Rigidbody>();
            rigidBody.isKinematic = true;
            rigidBody.mass = 0.0000001f;

            var probe = go.AddComponent<WaterVolumeProbe>();
            probe._Target = target;
            probe._Targetted = true;
            probe._Size = size;
            probe._Exclusions = target.GetComponentsInChildren<WaterVolumeSubtract>(true);

            return probe;
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField, FormerlySerializedAs("enter")]
        private UnityEvent _Enter;

        [SerializeField, FormerlySerializedAs("leave")]
        private UnityEvent _Leave;
        #endregion Inspector Variables

        #region Unity Methods
        private void Start()
        {
            ScanWaters();
        }

        private void FixedUpdate()
        {
            if (_Targetted)
            {
                if (_Target == null)
                {
                    Destroy(gameObject);            // cleans itself if target has been destroyed
                    return;
                }

                transform.position = _Target.position;
            }

            if (_CurrentWater != null && _CurrentWater.Volume.Boundless)
            {
                if (!_CurrentWater.Volume.IsPointInsideMainVolume(transform.position) && !_CurrentWater.Volume.IsPointInside(transform.position, _Exclusions, _Size))
                    LeaveCurrentWater();
            }
            else if (_CurrentWater == null)
                ScanBoundlessWaters();
        }

        private void OnDestroy()
        {
            _CurrentWater = null;

            if (_Enter != null)
            {
                _Enter.RemoveAllListeners();
                _Enter = null;
            }

            if (_Leave != null)
            {
                _Leave.RemoveAllListeners();
                _Leave = null;
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (_CurrentWater != null)
            {
                var volumeSubtract = WaterVolumeBase.GetWaterVolume<WaterVolumeSubtract>(other);

                if (volumeSubtract != null && volumeSubtract.EnablePhysics)
                {
                    //if(!currentWater.Volume.IsPointInside(transform.position, exclusions, size))
                    LeaveCurrentWater();
                }
            }
            else
            {
                var volumeAdd = WaterVolumeBase.GetWaterVolume<WaterVolumeAdd>(other);

                if (volumeAdd != null && volumeAdd.EnablePhysics/* && volumeAdd.Water.Volume.IsPointInside(transform.position, exclusions, size)*/)
                    EnterWater(volumeAdd.Water);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (_CurrentWater == null)
            {
                var volumeSubtract = WaterVolumeBase.GetWaterVolume<WaterVolumeSubtract>(other);

                if (volumeSubtract != null && volumeSubtract.EnablePhysics)
                    ScanWaters();
            }
            else
            {
                var volumeAdd = WaterVolumeBase.GetWaterVolume<WaterVolumeAdd>(other);

                if (volumeAdd != null && volumeAdd.Water == _CurrentWater && volumeAdd.EnablePhysics /* && !currentWater.Volume.IsPointInside(transform.position, exclusions, size)*/)
                    LeaveCurrentWater();
            }
        }
        #endregion Unity Methods

        #region Private Variables
        private Water _CurrentWater;
        private Transform _Target;
        private bool _Targetted;
        private WaterVolumeSubtract[] _Exclusions;
        private float _Size;
        #endregion Private Variables

        #region Private Methods
        [ContextMenu("Refresh Probe")]
        private void ScanWaters()
        {
            Vector3 position = transform.position;

            var waters = WaterSystem.Instance.Waters;
            int numWaters = waters.Count;

            for (int i = 0; i < numWaters; ++i)
            {
                if (waters[i].Volume.IsPointInside(position, _Exclusions, _Size))
                {
                    EnterWater(waters[i]);
                    return;
                }
            }

            LeaveCurrentWater();
        }

        private void ScanBoundlessWaters()
        {
            Vector3 position = transform.position;

            var boundlessWaters = WaterSystem.Instance.BoundlessWaters;
            int numInstances = boundlessWaters.Count;

            for (int i = 0; i < numInstances; ++i)
            {
                var water = boundlessWaters[i];

                if (water.Volume.IsPointInsideMainVolume(position) && water.Volume.IsPointInside(position, _Exclusions, _Size))
                {
                    EnterWater(water);
                    return;
                }
            }
        }

        private void EnterWater(Water water)
        {
            if (_CurrentWater == water) return;

            if (_CurrentWater != null)
                LeaveCurrentWater();

            _CurrentWater = water;

            if (_Enter != null)
                _Enter.Invoke();
        }

        private void LeaveCurrentWater()
        {
            if (_CurrentWater != null)
            {
                if (_Leave != null)
                    _Leave.Invoke();

                _CurrentWater = null;
            }
        }
        #endregion Private Methods
    }
}