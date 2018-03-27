using UnityEngine;

namespace UltimateWater
{
    public static class ButterflyFFTUtility
    {
        #region Private Methods
        // ReSharper disable once InconsistentNaming
        private static void BitReverse(int[] indices, int N, int n)
        {
            const int mask = 0x1;

            for (int j = 0; j < N; j++)
            {
                int val = 0x0;
                int temp = indices[j];

                for (int i = 0; i < n; i++)
                {
                    int t = (mask & temp);
                    val = (val << 1) | t;
                    temp = temp >> 1;
                }

                indices[j] = val;
            }
        }

        private static void ComputeWeights(Vector2[][] weights, int resolution, int numButterflies)
        {
            int groups = resolution >> 1;
            int numKs = 1;
            float invResolution = 1.0f / resolution;

            for (int i = 0; i < numButterflies; ++i)
            {
                int start = 0;
                int end = numKs;

                var weights2 = weights[i];

                for (int b = 0; b < groups; ++b)
                {
                    // ReSharper disable once InconsistentNaming
                    for (int k = start, K = 0; k < end; ++k, ++K)
                    {
                        float t = 2.0f * Mathf.PI * K * groups * invResolution;

                        float real = Mathf.Cos(t);
                        float im = -Mathf.Sin(t);

                        weights2[k].x = real;
                        weights2[k].y = im;
                        weights2[k + numKs].x = -real;
                        weights2[k + numKs].y = -im;
                    }

                    start += numKs << 1;
                    end = start + numKs;
                }

                groups = groups >> 1;
                numKs = numKs << 1;
            }
        }

        private static void ComputeWeights(float[][] weights, int resolution, int numButterflies)
        {
            int groups = resolution >> 1;
            int numKs = 2;
            float invResolution = 1.0f / resolution;

            for (int i = 0; i < numButterflies; ++i)
            {
                int start = 0;
                int end = numKs;

                var weights2 = weights[i];

                for (int b = 0; b < groups; ++b)
                {
                    // ReSharper disable once InconsistentNaming
                    for (int k = start, K = 0; k < end; k += 2, ++K)
                    {
                        float t = 2.0f * Mathf.PI * K * groups * invResolution;

                        float real = Mathf.Cos(t);
                        float im = -Mathf.Sin(t);

                        weights2[k] = real;
                        weights2[k + 1] = im;
                        weights2[k + numKs] = -real;
                        weights2[k + numKs + 1] = -im;
                    }

                    start += numKs << 1;
                    end = start + numKs;
                }

                groups = groups >> 1;
                numKs = numKs << 1;
            }
        }

        private static void ComputeIndices(int[][] indices, int resolution, int numButterflies)
        {
            int offset = resolution;
            int numIters = 1;

            for (int butterflyIndex = 0; butterflyIndex < numButterflies; ++butterflyIndex)
            {
                offset = offset >> 1;
                int step = offset << 1;

                int p = 0;
                int start = 0;
                int end = step;

                var indices2 = indices[butterflyIndex];

                for (int i = 0; i < numIters; ++i)
                {
                    for (int j = start, k = p, l = 0; j < end; j += 2, l += 2, ++k)
                    {
                        indices2[j] = k;
                        indices2[j + 1] = k + offset;
                        indices2[l + end] = k;
                        indices2[l + end + 1] = k + offset;
                    }

                    start += step << 1;
                    end += step << 1;
                    p += step;
                }

                numIters = numIters << 1;
            }

            BitReverse(indices[numButterflies - 1], resolution << 1, numButterflies);
        }
        #endregion Private Methods

        #region Public Methods
        public static void ComputeButterfly(int resolution, int numButterflies, out int[][] indices, out Vector2[][] weights)
        {
            indices = new int[numButterflies][];
            weights = new Vector2[numButterflies][];

            for (int i = 0; i < numButterflies; ++i)
            {
                indices[i] = new int[resolution << 1];
                weights[i] = new Vector2[resolution];
            }

            ComputeIndices(indices, resolution, numButterflies);
            ComputeWeights(weights, resolution, numButterflies);
        }

        public static void ComputeButterfly(int resolution, int numButterflies, out int[][] indices, out float[][] weights)
        {
            indices = new int[numButterflies][];
            weights = new float[numButterflies][];

            for (int i = 0; i < numButterflies; ++i)
            {
                indices[i] = new int[resolution << 1];
                weights[i] = new float[resolution << 1];
            }

            ComputeIndices(indices, resolution, numButterflies);
            ComputeWeights(weights, resolution, numButterflies);
        }
        #endregion Public Methods
    }
}