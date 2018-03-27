namespace UltimateWater.Internal
{
    using System.Collections.Generic;
    using UnityEngine;

#if WATER_SIMD
using Mono.Simd;
using vector4 = Mono.Simd.Vector4f;
#else

    using vector4 = UnityEngine.Vector4;

#endif

    public class CpuFFT
    {
        #region Public Types
        public class FFTBuffers
        {
            #region Public Variables
            public readonly float[] Timed;
            public readonly float[] PingPongA;
            public readonly float[] PingPongB;
            public readonly int[][] Indices;
            public readonly float[][] Weights;
            public readonly int NumButterflies;
            #endregion Public Variables

            #region Public Methods
            public FFTBuffers(int resolution)
            {
                _Resolution = resolution;
                Timed = new float[resolution * resolution * 12];
                PingPongA = new float[resolution * 12];
                PingPongB = new float[resolution * 12];
                NumButterflies = (int)(Mathf.Log(resolution) / Mathf.Log(2.0f));

                ButterflyFFTUtility.ComputeButterfly(resolution, NumButterflies, out Indices, out Weights);

                for (int ii = 0; ii < Indices.Length; ++ii)
                {
                    var localIndices = Indices[ii];

                    for (int i = 0; i < localIndices.Length; ++i)
                        localIndices[i] *= 12;
                }
            }

            public Vector3[] GetPrecomputedK(float tileSize)
            {
                Vector3[] map;

                if (!_PrecomputedKMap.TryGetValue(tileSize, out map))
                {
                    int halfResolution = _Resolution >> 1;
                    float frequencyScale = 2.0f * Mathf.PI / tileSize;

                    map = new Vector3[_Resolution * _Resolution];
                    int index = 0;

                    for (int y = 0; y < _Resolution; ++y)
                    {
                        int v = (y + halfResolution) % _Resolution;
                        float ky = frequencyScale * (v/* + 0.5f*/ - halfResolution);

                        for (int x = 0; x < _Resolution; ++x)
                        {
                            int u = (x + halfResolution) % _Resolution;
                            float kx = frequencyScale * (u/* + 0.5f*/ - halfResolution);

                            float k = Mathf.Sqrt(kx * kx + ky * ky);

                            map[index++] = new Vector3(k != 0 ? kx / k : 0.0f, k != 0 ? ky / k : 0.0f, k);
                        }
                    }

                    _PrecomputedKMap[tileSize] = map;
                }

                return map;
            }
            #endregion Public Methods

            #region Private Variables
            private readonly int _Resolution;
            private readonly Dictionary<float, Vector3[]> _PrecomputedKMap = new Dictionary<float, Vector3[]>(new FloatEqualityComparer());
            #endregion Private Variables
        }
        #endregion Public Types

        #region Public Methods
        public void Compute(WaterTileSpectrum targetSpectrum, float time, int outputBufferIndex)
        {
            _TargetSpectrum = targetSpectrum;
            _Time = time;

            Vector2[] displacements; vector4[] forceAndHeight;
            Vector2[] directionalSpectrum;

            lock (targetSpectrum)
            {
                _Resolution = targetSpectrum.ResolutionFFT;

                directionalSpectrum = targetSpectrum.DirectionalSpectrum;
                displacements = targetSpectrum.Displacements[outputBufferIndex];
                forceAndHeight = targetSpectrum.ForceAndHeight[outputBufferIndex];
            }

            FFTBuffers buffers;

            if (!_BuffersCache.TryGetValue(_Resolution, out buffers))
                _BuffersCache[_Resolution] = buffers = new FFTBuffers(_Resolution);

            float tileSize = targetSpectrum.WindWaves.UnscaledTileSizes[targetSpectrum.TileIndex];
            Vector3[] kMap = buffers.GetPrecomputedK(tileSize);

            if (targetSpectrum.DirectionalSpectrumDirty > 0)
            {
                ComputeDirectionalSpectra(targetSpectrum.TileIndex, directionalSpectrum, kMap);
                --targetSpectrum.DirectionalSpectrumDirty;
            }

            ComputeTimedSpectra(directionalSpectrum, buffers.Timed, kMap);
            ComputeFFT(buffers.Timed, displacements, forceAndHeight, buffers.Indices, buffers.Weights, buffers.PingPongA, buffers.PingPongB);
        }
        #endregion Public Methods

        #region Private Variables
        private WaterTileSpectrum _TargetSpectrum;
        private float _Time;
        private int _Resolution;

        private static readonly Dictionary<int, FFTBuffers> _BuffersCache = new Dictionary<int, FFTBuffers>();
        #endregion Private Variables

        #region Private Methods
        private void ComputeDirectionalSpectra(int scaleIndex, Vector2[] directional, Vector3[] kMap)
        {
            float directionality = 1.0f - _TargetSpectrum.WindWaves.SpectrumDirectionality;
            var cachedSpectra = _TargetSpectrum.WindWaves.SpectrumResolver.GetCachedSpectraDirect();
            int resolutionSqr = _Resolution * _Resolution;
            int halfResolution = _Resolution >> 1;
            int originalSpectrumResolution = _TargetSpectrum.WindWaves.FinalResolution;

            Vector2 windDirection = _TargetSpectrum.WindWaves.SpectrumResolver.WindDirection;

            for (int i = 0; i < resolutionSqr; ++i)
            {
                directional[i].x = 0.0f;
                directional[i].y = 0.0f;
            }

            lock (cachedSpectra)
            {
                var enumerator = cachedSpectra.Values.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var spectrum = enumerator.Current;
                    if (spectrum == null) { continue; }

                    float w = spectrum.Weight;

                    if (spectrum.GetStandardDeviation() * w <= 0.003f)
                        continue;

                    int index = 0;
                    int index2 = 0;
                    var omnidirectional = spectrum.SpectrumValues[scaleIndex];

                    for (int x = 0; x < _Resolution; ++x)
                    {
                        if (x == halfResolution)
                            index2 += (originalSpectrumResolution - _Resolution) * originalSpectrumResolution;

                        for (int y = 0; y < _Resolution; ++y)
                        {
                            if (y == halfResolution)
                                index2 += originalSpectrumResolution - _Resolution;

                            float nkx = kMap[index].x;
                            float nky = kMap[index].y;

                            if (nkx == 0.0f && nky == 0.0f)
                            {
                                nkx = windDirection.x;
                                nky = windDirection.y;
                            }

                            float dp = windDirection.x * nkx + windDirection.y * nky;
                            float directionalFactor = Mathf.Sqrt(1.0f + omnidirectional[index2].z * (2.0f * dp * dp - 1.0f)); // 2.0f * dp * dp - 1.0f = cos(2.0 * acos(dp))

                            if (dp < 0)
                                directionalFactor *= directionality;

                            float t = directionalFactor * w;
                            directional[index].x += omnidirectional[index2].x * t;
                            directional[index++].y += omnidirectional[index2++].y * t;
                        }
                    }
                }
                enumerator.Dispose();
            }

            var overlayedSpectra = _TargetSpectrum.WindWaves.SpectrumResolver.GetOverlayedSpectraDirect();

            lock (overlayedSpectra)
            {
                for (int i = overlayedSpectra.Count - 1; i >= 0; --i)
                {
                    var spectrum = overlayedSpectra[i];
                    float w = spectrum.Weight;

                    windDirection = spectrum.WindDirection;

                    if (spectrum.GetStandardDeviation() * w <= 0.003f)
                        continue;

                    float offsetX = spectrum.WeatherSystemOffset.x;
                    float offsetZ = spectrum.WeatherSystemOffset.y;
                    float radius = spectrum.WeatherSystemRadius;
                    float distance = Mathf.Sqrt(offsetX * offsetX + offsetZ * offsetZ);

                    const float u10 = 10.0f;
                    float omegac = 0.84f * Mathf.Pow((float)System.Math.Tanh(Mathf.Pow(distance / 22000.0f, 0.4f)), -0.75f);
                    float kp = spectrum.Gravity * FastMath.Pow2(omegac / u10);

                    int index = 0;
                    int index2 = 0;
                    var omnidirectional = spectrum.SpectrumValues[scaleIndex];

                    for (int x = 0; x < _Resolution; ++x)
                    {
                        if (x == halfResolution)
                            index2 += (originalSpectrumResolution - _Resolution) * originalSpectrumResolution;

                        for (int y = 0; y < _Resolution; ++y)
                        {
                            if (y == halfResolution)
                                index2 += originalSpectrumResolution - _Resolution;

                            float nkx = kMap[index].x;
                            float nky = kMap[index].y;

                            if (nkx == 0.0f && nky == 0.0f)
                            {
                                nkx = windDirection.x;
                                nky = windDirection.y;
                            }

                            float dp = windDirection.x * nkx + windDirection.y * nky;
                            float directionalFactor = Mathf.Sqrt(1.0f + omnidirectional[index2].z * (2.0f * dp * dp - 1.0f)); // 2.0f * dp * dp - 1.0f = cos(2.0 * acos(dp))

                            if (dp < 0)
                                directionalFactor *= directionality;

                            float t = directionalFactor * w;

                            if (radius != 0.0f)
                            {
                                // distant weather systems
                                float k = kMap[index].z;
                                float b = -2.0f * nkx * offsetX + -2.0f * nky * offsetZ;
                                float c = offsetX * offsetX + offsetZ * offsetZ - radius * radius;

                                float sqrtarg = b * b - 4.0f * c;

                                if (sqrtarg < 0.0f)
                                {
                                    // if that wave wouldn't reach this place
                                    directional[index].x = 0.0f;
                                    directional[index++].y = 0.0f;
                                    index2++;
                                    continue;
                                }

                                float sqrt = Mathf.Sqrt(sqrtarg);
                                float t1 = (sqrt - b) * 0.5f;
                                float t2 = (-sqrt - b) * 0.5f;

                                if (t1 > 0.0f && t2 > 0.0f)
                                {
                                    // if it's in a wrong direction
                                    directional[index].x = 0.0f;
                                    directional[index++].y = 0.0f;
                                    index2++;
                                    continue;
                                }

                                Vector2 intersection1 = new Vector2(nkx * t1, nky * t1);
                                Vector2 intersection2 = new Vector2(nkx * t2, nky * t2);
                                float angularFactor = Vector2.Distance(intersection1, intersection2) / (radius * 2.0f);
                                t *= angularFactor;

                                if (t1 * t2 > 0.0f)
                                {
                                    float dist = Mathf.Min(-t1, -t2);
                                    float kDivKp = k / kp;
                                    float dissipationFactor = Mathf.Exp(-0.000001f * dist * kDivKp * kDivKp);
                                    t *= dissipationFactor;
                                }
                            }
                            // ---

                            directional[index].x += omnidirectional[index2].x * t;
                            directional[index++].y += omnidirectional[index2++].y * t;
                        }
                    }
                }
            }

            /*var overlays = targetSpectrum.windWaves.SpectrumResolver.GetOverlayedSpectraDirect();

            for (int i = overlays.Count - 1; i >= 0; --i)
            {
                float w = 1.0f;

                if(w <= 0.005f)
                    continue;

                int index = 0;
                int index2 = 0;
                var spectrum = overlays[i].GetSpectrumDataDirect(scaleIndex);

                for(int x = 0; x < resolution; ++x)
                {
                    if(x == halfResolution)
                        index2 += (originalSpectrumResolution - resolution) * originalSpectrumResolution;

                    for(int y = 0; y < resolution; ++y)
                    {
                        if(y == halfResolution)
                            index2 += originalSpectrumResolution - resolution;

                        directional[index].x += spectrum[index2].x * w;
                        directional[index++].y += spectrum[index2++].y * w;
                    }
                }
            }*/
        }

        private void ComputeTimedSpectra(Vector2[] directional, float[] timed, Vector3[] kMap)
        {
            Vector2 windDirection = _TargetSpectrum.WindWaves.SpectrumResolver.WindDirection;
            float gravity = _TargetSpectrum.Water.Gravity;
            int index = 0;
            int index3 = 0;

            for (int y = 0; y < _Resolution; ++y)
            {
                for (int x = 0; x < _Resolution; ++x)
                {
                    float nkx = kMap[index].x;
                    float nky = kMap[index].y;
                    float k = kMap[index].z;

                    if (nkx == 0.0f && nky == 0.0f)
                    {
                        nkx = windDirection.x;
                        nky = windDirection.y;
                    }

                    int index2 = _Resolution * ((_Resolution - y) % _Resolution) + (_Resolution - x) % _Resolution;

                    Vector2 s1 = directional[index];
                    Vector2 s2 = directional[index2];

                    float t = _Time * Mathf.Sqrt(gravity * k);

                    float s = Mathf.Sin(t);
                    float c = Mathf.Cos(t);
                    //int icx = ((int)(t * 325.949f) & 2047);
                    //float s = FastMath.sines[icx];
                    //float c = FastMath.cosines[icx];

                    float sx = (s1.x + s2.x) * c - (s1.y + s2.y) * s;
                    float sy = (s1.x - s2.x) * s + (s1.y - s2.y) * c;

                    timed[index3++] = sy * nkx;     // fx1
                    timed[index3++] = sy * nky;     // fz1
                    timed[index3++] = -sx;          // fy1
                    timed[index3++] = sy;           // dy1

                    timed[index3++] = sx * nkx;     // fx0
                    timed[index3++] = sx * nky;     // fz0
                    timed[index3++] = sy;           // fy0
                    timed[index3++] = sx;           // dy0

                    timed[index3++] = sy * nkx;     // dx0
                    timed[index3++] = -sx * nkx;    // dx1
                    timed[index3++] = sy * nky;     // dz0
                    timed[index3++] = -sx * nky;    // dz1

                    ++index;
                }
            }
        }

        private void ComputeFFT(float[] data, Vector2[] displacements, vector4[] forceAndHeight, int[][] indices, float[][] weights, float[] pingPongA, float[] pingPongB)
        {
            int resolutionx12 = pingPongA.Length;
            int index = 0;

            for (int y = _Resolution - 1; y >= 0; --y)
            {
                System.Array.Copy(data, index, pingPongA, 0, resolutionx12);

                FFT(indices, weights, ref pingPongA, ref pingPongB);

                System.Array.Copy(pingPongA, 0, data, index, resolutionx12);
                index += resolutionx12;
            }

            index = _Resolution * (_Resolution + 1) * 12;

            for (int x = _Resolution - 1; x >= 0; --x)
            {
                index -= 12;

                int index2 = index;

                for (int y = resolutionx12 - 12; y >= 0; y -= 12)
                {
                    index2 -= resolutionx12;

                    for (int i = 0; i < 12; ++i)
                        pingPongA[y + i] = data[index2 + i];
                }

                FFT(indices, weights, ref pingPongA, ref pingPongB);

                index2 = index / 12;

                for (int y = resolutionx12 - 12; y >= 0; y -= 12)
                {
                    index2 -= _Resolution;

                    forceAndHeight[index2] = new vector4(pingPongA[y], pingPongA[y + 2], pingPongA[y + 1], pingPongA[y + 7]);
                    displacements[index2] = new Vector2(pingPongA[y + 8], pingPongA[y + 10]);
                }
            }
        }

        private void FFT(int[][] indices, float[][] weights, ref float[] pingPongA, ref float[] pingPongB)
        {
            int numButterflies = weights.Length;

            for (int butterflyIndex = 0; butterflyIndex < numButterflies; ++butterflyIndex)
            {
                var localIndices = indices[numButterflies - butterflyIndex - 1];
                var localWeights = weights[butterflyIndex];
                int i12 = (_Resolution - 1) * 12;

                for (int i2 = localIndices.Length - 2; i2 >= 0; i2 -= 2)
                {
                    int ix = localIndices[i2];
                    int iy = localIndices[i2 + 1];

#if WATER_SIMD
                    float wyf = localWeights[i2 + 1];
                    vector4 wx = new vector4(localWeights[i2]);
                    vector4 wy = new vector4(wyf);

                    pingPongB.SetVector(pingPongA.GetVector(ix) + wy * pingPongA.GetVector(iy + 4) + wx * pingPongA.GetVector(iy), i12);
                    pingPongB.SetVector(pingPongA.GetVector(ix + 4) + wx * pingPongA.GetVector(iy + 4) - wy * pingPongA.GetVector(iy), i12 + 4);

                    iy += 8;
                    wy = new vector4(-wyf, wyf, -wyf, wyf);
                    pingPongB.SetVector(pingPongA.GetVector(ix + 8) + wx * pingPongA.GetVector(iy) + wy * pingPongA.GetVector(iy).Shuffle(ShuffleSel.XFromY | ShuffleSel.YFromX | ShuffleSel.ZFromW | ShuffleSel.WFromZ), i12 + 8);

                    i12 -= 12;
#else
                    float wx = localWeights[i2];
                    float wy = localWeights[i2 + 1];
                    int iy4 = iy + 4;

                    pingPongB[i12++] = pingPongA[ix++] + wy * pingPongA[iy4++] + wx * pingPongA[iy++];
                    pingPongB[i12++] = pingPongA[ix++] + wy * pingPongA[iy4++] + wx * pingPongA[iy++];
                    pingPongB[i12++] = pingPongA[ix++] + wy * pingPongA[iy4++] + wx * pingPongA[iy++];
                    pingPongB[i12++] = pingPongA[ix++] + wy * pingPongA[iy4] + wx * pingPongA[iy++];

                    iy4 = iy;
                    iy -= 4;

                    pingPongB[i12++] = pingPongA[ix++] + wx * pingPongA[iy4++] - wy * pingPongA[iy++];
                    pingPongB[i12++] = pingPongA[ix++] + wx * pingPongA[iy4++] - wy * pingPongA[iy++];
                    pingPongB[i12++] = pingPongA[ix++] + wx * pingPongA[iy4++] - wy * pingPongA[iy++];
                    pingPongB[i12++] = pingPongA[ix++] + wx * pingPongA[iy4++] - wy * pingPongA[iy];

                    iy = iy4;

                    pingPongB[i12++] = pingPongA[ix++] + wx * pingPongA[iy4++] - wy * pingPongA[iy + 1];
                    pingPongB[i12++] = pingPongA[ix++] + wx * pingPongA[iy4++] + wy * pingPongA[iy];
                    pingPongB[i12++] = pingPongA[ix++] + wx * pingPongA[iy4++] - wy * pingPongA[iy + 3];
                    pingPongB[i12] = pingPongA[ix] + wx * pingPongA[iy4] + wy * pingPongA[iy + 2];

                    i12 -= 23;
#endif
                }

                var t = pingPongA;
                pingPongA = pingPongB;
                pingPongB = t;
            }
        }
        #endregion Private Methods
    }
}