namespace UltimateWater.Internal
{
    using UnityEngine;

    /// <summary>
    ///     Animates water UV in time to simulate a water flow.
    /// </summary>
    [System.Serializable]
    public sealed class WaterUvAnimator : WaterModule
    {
        #region Public Variables
        public Vector2 WindOffset
        {
            get { return new Vector2(_WindOffset1X, _WindOffset1Y); }
        }
        public NormalMapAnimation NormalMapAnimation1
        {
            get { return _NormalMapAnimation1; }
            set
            {
                _NormalMapAnimation1 = value;
                _WindVectorsDirty = true;
                _UvTransform1.x = _NormalMapAnimation1.Tiling.x;
                _UvTransform1.y = _NormalMapAnimation1.Tiling.y;
            }
        }
        public NormalMapAnimation NormalMapAnimation2
        {
            get { return _NormalMapAnimation2; }
            set
            {
                _NormalMapAnimation2 = value;
                _WindVectorsDirty = true;
                _UvTransform2.x = _NormalMapAnimation2.Tiling.x;
                _UvTransform2.y = _NormalMapAnimation2.Tiling.y;
            }
        }
        #endregion Public Variables

        #region Private Variables
        private NormalMapAnimation _NormalMapAnimation1 = new NormalMapAnimation(1.0f, -10.0f, 1.0f, new Vector2(1.0f, 1.0f));
        private NormalMapAnimation _NormalMapAnimation2 = new NormalMapAnimation(-0.55f, 20.0f, 0.74f, new Vector2(1.5f, 1.5f));

        private float _WindOffset1X, _WindOffset1Y;
        private float _WindOffset2X, _WindOffset2Y;
        private Vector2 _WindSpeed1;
        private Vector2 _WindSpeed2;
        private Vector2 _WindSpeed;

        private Water _Water;
        private WindWaves _WindWaves;
        private bool _HasWindWaves;

        private Vector4 _UvTransform1;
        private Vector4 _UvTransform2;
        private bool _WindVectorsDirty = true;

        private float _LastTime;
        #endregion Private Variables

        #region Private Methods
        internal override void Start(Water water)
        {
            _Water = water;
            _WindWaves = water.WindWaves;
            _HasWindWaves = _WindWaves != null;
        }

        internal override void Update()
        {
            float time = _Water.Time;
            float deltaTime = time - _LastTime;
            _LastTime = time;

            if (_WindVectorsDirty || HasWindSpeedChanged())
            {
                PrecomputeWindVectors();
                _WindVectorsDirty = false;
            }

            // apply offset
            _WindOffset1X += _WindSpeed1.x * deltaTime;
            _WindOffset1Y += _WindSpeed1.y * deltaTime;
            _WindOffset2X += _WindSpeed2.x * deltaTime;
            _WindOffset2Y += _WindSpeed2.y * deltaTime;

            _UvTransform1.z = -_WindOffset1X * _UvTransform1.x;
            _UvTransform1.w = -_WindOffset1Y * _UvTransform1.y;

            _UvTransform2.z = -_WindOffset2X * _UvTransform2.x;
            _UvTransform2.w = -_WindOffset2Y * _UvTransform2.y;

            // apply to material
            var block = _Water.Renderer.PropertyBlock;
            block.SetVector(ShaderVariables.BumpMapST, _UvTransform1);
            block.SetVector(ShaderVariables.DetailAlbedoMapST, _UvTransform2);
        }

        private void PrecomputeWindVectors()
        {
            _WindSpeed = GetWindSpeed();
            _WindSpeed1 = FastMath.Rotate(_WindSpeed, _NormalMapAnimation1.Deviation * Mathf.Deg2Rad) * (_NormalMapAnimation1.Speed * 0.001365f);
            _WindSpeed2 = FastMath.Rotate(_WindSpeed, _NormalMapAnimation2.Deviation * Mathf.Deg2Rad) * (_NormalMapAnimation2.Speed * 0.00084f);
        }

        private Vector2 GetWindSpeed()
        {
            return _HasWindWaves ? _WindWaves.WindSpeed : new Vector2(1.0f, 0.0f);
        }

        private bool HasWindSpeedChanged()
        {
            return _HasWindWaves && _WindWaves.WindSpeedChanged;
        }
        #endregion Private Methods
    }
}