namespace UltimateWater
{
    using UnityEngine;

    /// <summary>
    ///     It's a simple script that makes the transform follow the initial point on water below. It is faster than physical
    ///     simulation and may be preferred for some small decal objects for performance reasons.
    /// </summary>
    public sealed class WaterFloat : MonoBehaviour
    {
        #region Public Types
        public enum DisplacementMode
        {
            Height,
            Displacement
        }
        #endregion Public Types

        #region Inspector Variables
        [SerializeField]
        private DisplacementMode _DisplacementMode = DisplacementMode.Displacement;

        [SerializeField]
        private float _HeightBonus;

        [Range(0.04f, 1.0f)]
        [SerializeField]
        private float _Precision = 0.2f;

        [SerializeField] private Water _Water;
        #endregion Inspector Variables

        #region Private Variables
        private Vector3 _InitialPosition;
        private Vector3 _PreviousPosition;
        private WaterSample _Sample;
        #endregion Private Variables

        #region Unity Messages
        private void Start()
        {
            // check references
            if (_Water == null)
            {
                _Water = Utilities.GetWaterReference();
            }
            if (_Water.IsNullReference(this)) { return; }

            _InitialPosition = transform.position;
            _PreviousPosition = _InitialPosition;

            _Sample = new WaterSample(_Water, (WaterSample.DisplacementMode)_DisplacementMode, _Precision);
            _Sample.Start(transform.position);
        }

        private void OnDisable()
        {
            _Sample.Stop();
        }

        private void LateUpdate()
        {
            _InitialPosition += transform.position - _PreviousPosition;

            Vector3 displaced = _Sample.GetAndReset(_InitialPosition.x, _InitialPosition.z,
                WaterSample.ComputationsMode.ForceCompletion);
            displaced.y += _HeightBonus;
            transform.position = displaced;

            _PreviousPosition = displaced;
        }
        private void Reset()
        {
            _Water = Utilities.GetWaterReference();
        }
        #endregion Unity Messages
    }
}