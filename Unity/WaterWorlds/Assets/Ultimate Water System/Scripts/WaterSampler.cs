namespace UltimateWater
{
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Creates particle effects when interacting with water
    /// </summary>
    [AddComponentMenu("Ultimate Water/Water Sampler")]
    public class WaterSampler : MonoBehaviour
    {
        #region Public Types
        [System.Serializable]
        public class WaterSubmersionEvent : UnityEvent<SubmersionState> { }

        public enum SubmersionState
        {
            Under,
            Above
        }
        #endregion Public Types

        #region Inspector Variables
        [Header("References")]
        [SerializeField]
        private Water _Water;

        #endregion Inspector Variables

        #region Public Variables
        public float Hysteresis = 0.1f;

        public float Height { get; private set; }
        public float Velocity { get; private set; }

        public SubmersionState State { get; private set; }

        [Header("Events")]
        public WaterSubmersionEvent OnSubmersionStateChanged;

        public bool IsInitialized
        {
            get { return _Water != null && _Sample != null; }
        }
        #endregion Public Variables

        #region Private Variables
        private WaterSample _Sample;

        private float _PreviousWaterHeight;
        private float _PreviousObjectHeight;
        #endregion Private Variables

        #region Unity Messages
        private void Start()
        {
            if (_Water == null)
            {
                _Water = Utilities.GetWaterReference();
            }

            _Sample = new WaterSample(_Water);
        }

        private void Update()
        {
            var result = _Sample.GetAndReset(transform.position);

            float objectVelocity = (transform.position.y - _PreviousObjectHeight) / Time.deltaTime;
            float waterVelocity = (result.y - _PreviousWaterHeight) / Time.deltaTime;

            Velocity = Mathf.Abs(objectVelocity - waterVelocity);
            Height = transform.position.y - result.y;

            // if the current state differs from saved,
            // and the height difference is bigger than minimum required
            if (State != GetState(Height) && Mathf.Abs(Height) > Hysteresis)
            {
                State = Height > 0.0f ? SubmersionState.Above : SubmersionState.Under;
                OnSubmersionStateChanged.Invoke(State);
            }

            _PreviousObjectHeight = transform.position.y;
            _PreviousWaterHeight = result.y;

            transform.rotation = Quaternion.identity;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.2f);
        }

        private void Reset()
        {
            _Water = Utilities.GetWaterReference();
        }

        private static SubmersionState GetState(float height)
        {
            return height > 0.0f ? SubmersionState.Above : SubmersionState.Under;
        }
        #endregion Unity Messages
    }
}