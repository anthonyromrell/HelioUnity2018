namespace UltimateWater
{
    using UnityEngine;

    public struct WaterWave : System.IComparable<WaterWave>
    {
        #region Public Variables
        public float K
        {
            get { return Mathf.Sqrt(_Kx * _Kx + _Kz * _Kz); }
        }
        #endregion Public Variables

        #region Public Methods
        public int CompareTo(WaterWave other)
        {
            return other._CPUPriority.CompareTo(_CPUPriority);
        }
        public float GetHeightAt(float x, float z, float t)
        {
            float dot = _Kx * x + _Kz * z;
            return _Amplitude * Mathf.Sin(dot + t * _W + _Offset);
        }
        public void GetForceAndHeightAt(float x, float z, float t, ref Vector4 result)
        {
            float dot = _Kx * x + _Kz * z;

            //FastMath.SinCos2048(dot + t * w + offset, out s, out c);		// inlined below
            int icx = ((int)((dot + t * _W + _Offset) * 325.949f) & 2047);
            float s = FastMath.Sines[icx];
            float c = FastMath.Cosines[icx];

            float sa = _Amplitude * s;
            float ca = _Amplitude * c;

            result.x += _Nkx * sa;
            result.z += _Nky * sa;
            result.y += ca;
            result.w += sa;
        }
        // (kx, kz, w) * (x, z, t)
        public Vector2 GetRawHorizontalDisplacementAt(float x, float z, float t)
        {
            float dot = _Kx * x + _Kz * z;
            float c = _Amplitude * Mathf.Cos(dot + t * _W + _Offset);

            return new Vector2(_Nkx * c, _Nky * c);
        }
        /// <summary>
        /// Computes full wave displacement at a point.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector3 GetDisplacementAt(float x, float z, float t)
        {
            float dot = _Kx * x + _Kz * z;

            float s, c;
            FastMath.SinCos2048(dot + t * _W + _Offset, out s, out c);

            c *= _Amplitude;

            return new Vector3(_Nkx * c, s * _Amplitude, _Nky * c);
        }

        public WaterWave(byte scaleIndex, float offsetX, float offsetZ, ushort u, ushort v, float kx, float kz, float k, float w, float amplitude)
        {
            _ScaleIndex = scaleIndex;
            _DotOffset = offsetX * kx + offsetZ * kz;// + 0.0329f;
            _U = u;
            _V = v;
            _Kx = kx;
            _Kz = kz;
            _Nkx = k != 0 ? kx / k : 0.707107f;
            _Nky = k != 0 ? kz / k : 0.707107f;
            _Amplitude = 2.0f * amplitude;
            _Offset = 0.0f;
            _W = w;
            _CPUPriority = (amplitude >= 0 ? amplitude : -amplitude);
        }

        public void UpdateSpectralValues(Vector3[][] spectrum, Vector2 windDirection, float directionalityInv, int resolution, float horizontalScale)
        {
            var s = spectrum[_ScaleIndex][_U * resolution + _V];

            float dp = windDirection.x * _Nkx + windDirection.y * _Nky;
            float phi = Mathf.Acos(dp * 0.999f);
            float scale = Mathf.Sqrt(1.0f + s.z * Mathf.Cos(2.0f * phi));
            if (dp < 0.0f) scale *= directionalityInv;

            float sx = s.x * scale;
            float sy = s.y * scale;

            _Amplitude = 2.0f * Mathf.Sqrt(sx * sx + sy * sy);
            _Offset = Mathf.Atan2(Mathf.Abs(sx), Mathf.Abs(sy));

            if (sy > 0.0f)
            {
                _Amplitude = -_Amplitude;
                _Offset = -_Offset;
            }

            if (sx < 0.0f) _Offset = -_Offset;

            _Offset += _DotOffset;
            _CPUPriority = (_Amplitude >= 0 ? _Amplitude : -_Amplitude);
        }
        #endregion Public Methods

        #region Private Variables
        private readonly ushort _U, _V;
        private readonly float _Kx, _Kz;
        private readonly float _DotOffset;

        internal readonly float _Nkx, _Nky;
        internal readonly float _W;
        internal readonly byte _ScaleIndex;

        internal float _Amplitude;
        internal float _CPUPriority;         // TODO: pack in ushort
        internal float _Offset;
        #endregion Private Variables
    }
}