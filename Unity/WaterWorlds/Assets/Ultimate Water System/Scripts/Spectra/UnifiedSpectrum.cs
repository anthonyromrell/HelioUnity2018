using UnityEngine;

namespace UltimateWater
{
    /// <summary>
    ///     Spectrum is based on the following paper:
    ///     "A unified directional spectrum for long and short wind-driven waves." T. Elfouhaily, B. Chapron, and K.Katsaros
    ///     Institut
    /// </summary>
    public class UnifiedSpectrum : WaterWavesSpectrum
    {
        #region Public Methods
        public UnifiedSpectrum(float tileSize, float gravity, float windSpeed, float amplitude, float freqScale, float fetch) : base(tileSize, gravity, windSpeed, amplitude)
        {
            _Fetch = fetch;
            _FreqScale = freqScale;
        }

        public override void ComputeSpectrum(Vector3[] spectrum, float tileSizeMultiplier, int maxResolution, System.Random random)
        {
            int resolution = Mathf.RoundToInt(Mathf.Sqrt(spectrum.Length));
            int halfResolution = resolution / 2;
            int numRandomSkips = (maxResolution - resolution) / 2;

            if (numRandomSkips < 0)
                numRandomSkips = 0;

            float frequencyScale = 2.0f * Mathf.PI / (TileSize * tileSizeMultiplier * _FreqScale);

            float u10 = _WindSpeed;

            //float omegac = 0.84f;
            float omegac = 0.84f * Mathf.Pow((float)System.Math.Tanh(Mathf.Pow(_Fetch / 22000.0f, 0.4f)), -0.75f);
            float gamma = omegac <= 1.0f ? 1.7f : 1.7f + 6.0f * Mathf.Log(omegac);

            // short-wave parameters
            const float cm = 0.23f;
            float km = 2.0f * _Gravity / (cm * cm);

            // long-wave parameters
            float kp = _Gravity * FastMath.Pow2(omegac / u10);
            float cp = PhaseSpeed(kp, km);

            float omega = u10 / cp;
            float alphap = 0.006f * Mathf.Sqrt(omega);
            float omega1 = -omega / Mathf.Sqrt(10.0f);

            float sigma = 0.08f * (1.0f + 4.0f * Mathf.Pow(omegac, -3.0f));
            float sigma1 = 1.0f / (2.0f * sigma * sigma);

            float z0 = 3.7e-5f * u10 * u10 / _Gravity * Mathf.Pow(u10 / cp, 0.9f);
            float friction = u10 * 0.41f / Mathf.Log(10.0f / z0);           // 0.41 is the estimated 'k' from "the law of the wall"

            float a0 = Mathf.Log(2.0f) / 4.0f;
            float ap = 4.0f;
            float am = 0.13f * friction / cm;

            float alpham = 0.01f * (friction < cm ? 1.0f + Mathf.Log(friction / cm) : 1.0f + 3.0f * Mathf.Log(friction / cm));

            // skip random values that normally would be generated at max resolution
#pragma warning disable 0219
            for (int i = 0; i < numRandomSkips; ++i)
            {
                for (int ii = 0; ii < maxResolution; ++ii)
                {
                    Random.Range(0.000001f, 1.0f);
                    // _t_ value is used to forward the random values
                    // ReSharper disable once NotAccessedVariable
                    float t = Random.value;
                    Random.Range(0.000001f, 1.0f);
                    // ReSharper disable once RedundantAssignment
                    t = Random.value;
                }
            }

            for (int x = 0; x < resolution; ++x)
            {
                float kx = frequencyScale * (x/* + 0.5f*/ - halfResolution);
                float kxkx = kx * kx;

                int xCoord = (x + halfResolution) % resolution;
                int offset = xCoord * resolution;

                // skip random values that normally would be generated at max resolution
                for (int i = 0; i < numRandomSkips; ++i)
                {
                    Random.Range(0.000001f, 1.0f);
                    // _t_ value is used to forward the random values
                    // ReSharper disable once NotAccessedVariable
                    float t = Random.value;
                    Random.Range(0.000001f, 1.0f);
                    // ReSharper disable once RedundantAssignment
                    t = Random.value;
                }

                for (int y = 0; y < resolution; ++y)
                {
                    float ky = frequencyScale * (y/* + 0.5f*/ - halfResolution);

                    float k = Mathf.Sqrt(kxkx + ky * ky);
                    float c = PhaseSpeed(k, km);

                    /*
                     * Long-wave spectrum (bl)
                     */
                    float moskowitz = Mathf.Exp((-5.0f / 4.0f) * FastMath.Pow2(kp / k));

                    float sqrtkDivkp = Mathf.Sqrt(k / kp) - 1.0f;
                    float r = Mathf.Exp(-FastMath.Pow2(sqrtkDivkp) * sigma1);
                    float jonswap = Mathf.Pow(gamma, r);

                    float fp = moskowitz * jonswap * Mathf.Exp(omega1 * sqrtkDivkp);

                    float bl = 0.5f * alphap * (cp / c) * fp;

                    /*
                     * Short-wave spectrum (bh)
                     */
                    float fm = Mathf.Exp(-0.25f * FastMath.Pow2(k / km - 1.0f));
                    float bh = 0.5f * alpham * (cm / c) * fm * moskowitz;               // equation in paper seems to be wrong (missing moskowitz term) / it's fixed now

                    /*
                     * Directionality
                     */
                    float deltak = (float)System.Math.Tanh(a0 + ap * Mathf.Pow(c / cp, 2.5f) + am * Mathf.Pow(cm / c, 2.5f));

                    //float dp = windSpeed.x * kx / k + windSpeed.y * ky / k;
                    //float phi = Mathf.Acos(dp);

                    /*
                     * Total omni-directional spectrum
                     */
                    float sk = _Amplitude * (bl + bh) /* (1.0f + deltak * Mathf.Cos(2.0f * phi))*/ / (k * k * k * k * 2.0f * Mathf.PI);

                    // precision problems may sometimes produce negative values here
                    if (sk > 0.0f)
                        sk = Mathf.Sqrt(sk) * frequencyScale * 0.5f;            // 1.1 added * 0.5 to match empirical wikipedia wave height data
                    else
                        sk = 0.0f;

                    float h = FastMath.Gauss01() * sk;
                    float hi = FastMath.Gauss01() * sk;

                    int yCoord = (y + halfResolution) % resolution;

                    if (x == halfResolution && y == halfResolution)
                    {
                        h = 0.0f;
                        hi = 0.0f;
                        deltak = 0.0f;
                    }

                    spectrum[offset + yCoord] = new Vector3(h, hi, deltak);
                }

                // skip random values that normally would be generated at max resolution
                for (int i = 0; i < numRandomSkips; ++i)
                {
                    Random.Range(0.000001f, 1.0f);
                    // _t_ value is used to forward the random values
                    // ReSharper disable once NotAccessedVariable
                    float t = Random.value;
                    Random.Range(0.000001f, 1.0f);
                    // ReSharper disable once RedundantAssignment
                    t = Random.value;
                }
            }

            // skip random values that normally would be generated at max resolution
            for (int i = 0; i < numRandomSkips; ++i)
            {
                for (int ii = 0; ii < maxResolution; ++ii)
                {
                    Random.Range(0.000001f, 1.0f);
                    // _t_ value is used to forward the random values
                    // ReSharper disable once NotAccessedVariable
                    float t = Random.value;
                    Random.Range(0.000001f, 1.0f);
                    // ReSharper disable once RedundantAssignment
                    t = Random.value;
                }
            }
#pragma warning restore 0219
        }
        #endregion Public Methods

        #region Private Variables
        private readonly float _Fetch;
        private readonly float _FreqScale;
        #endregion Private Variables

        #region Private Methods
        private float PhaseSpeed(float k, float km)
        {
            return Mathf.Sqrt(_Gravity / k * (1.0f + FastMath.Pow2(k / km)));
        }
        #endregion Private Methods
    }
}