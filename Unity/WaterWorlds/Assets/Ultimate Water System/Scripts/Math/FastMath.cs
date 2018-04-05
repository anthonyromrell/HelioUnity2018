using UnityEngine;

#if WATER_SIMD
using Mono.Simd;
#endif

namespace UltimateWater
{
    /// <summary>
    ///     A collection of math utilities for the water.
    /// </summary>
    public class FastMath
    {
        #region Public Variables
        public static readonly float[] Sines;
        public static readonly float[] Cosines;
        public static readonly float[] PositiveTanhSqrt;
        public static readonly float[] PositiveTanhSqrtNoZero;
        #endregion Public Variables

        #region Public Methods
        static FastMath()
        {
            Sines = new float[2048];
            Cosines = new float[2048];
            PositiveTanhSqrt = new float[2048];
            PositiveTanhSqrtNoZero = new float[2048];

            PrecomputeFastSines();
        }

        private static void PrecomputeFastSines()
        {
            const float p = Mathf.PI * 2.0f / 2048.0f;

            for (int i = 0; i < 2048; ++i)
                Sines[i] = Mathf.Sin(i * p);

            for (int i = 0; i < 2048; ++i)
                Cosines[i] = Mathf.Cos(i * p);

            const float tanhScale = 4.0f / 2048.0f;

            for (int i = 0; i < 2048; ++i)
                PositiveTanhSqrt[i] = PositiveTanhSqrtNoZero[i] = Mathf.Sqrt((float)System.Math.Tanh(i * tanhScale));

            PositiveTanhSqrtNoZero[0] = 0.00002f;
        }

        /// <summary>
        /// A bit faster sine with lower precision.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Sin2048(float x)
        {
            int icx = ((int)(x * 325.949f) & 2047);

            return Sines[icx];
        }

        /// <summary>
        /// A bit faster cosine with lower precision.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Cos2048(float x)
        {
            int icx = ((int)(x * 325.949f) & 2047);

            return Cosines[icx];
        }

        /// <summary>
        /// Noticeably faster that calling Mathf.Sin and Mathf.Cos, but has lower precision.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="s"></param>
        /// <param name="c"></param>
        public static void SinCos2048(float x, out float s, out float c)
        {
            int icx = ((int)(x * 325.949f) & 2047);

            s = Sines[icx];
            c = Cosines[icx];
        }

        public static float TanhSqrt2048Positive(float x)
        {
            int icx = (int)(x * 512.0f);
            return icx >= 2048 ? 1.0f : PositiveTanhSqrt[icx];
        }

        /// <summary>
        /// Fast power of 2.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Pow2(float x)
        {
            return x * x;
        }

        /// <summary>
        /// Fast power of 4.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Pow4(float x)
        {
            float t = x * x;
            return t * t;
        }

        /// <summary>
        /// Faster floor to int.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static int FloorToInt(float f)
        {
            int x = (int)f; if (x > f) --x;
            return x;
        }

        /// <summary>
        /// Normalizes angle to range -180 to 180 degrees.
        /// </summary>
        /// <returns></returns>
        public static float Angle180(float f)
        {
            return f - (float)System.Math.Floor((f + 180.0f) / 360.0f) * 360.0f;
        }

        /// <summary>
        /// Normalizes angle to range 0 to 360 degrees.
        /// </summary>
        /// <returns></returns>
        public static float Angle360(float f)
        {
            f %= 360;
            return f >= 0 ? f : f + 360.0f;
        }

        /// <summary>
        /// Fast vector normalization. It's at least 2x faster than Vector3.Normalize() and Vector3.normalized.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void Normalize(ref Vector3 a, ref Vector3 b)
        {
            double mag = System.Math.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);

            if (mag > 9.99999974737875E-06)
            {
                float magInv = (float)(1.0 / mag);
                b.x = a.x * magInv;
                b.y = a.y * magInv;
                b.z = a.z * magInv;
            }
            else
            {
                b.x = 0.0f;
                b.y = 0.0f;
                b.z = 0.0f;
            }
        }

        /// <summary>
        /// Fast vector normalization. It's consistently faster than Vector3.Normalize() and Vector3.normalized.
        /// </summary>
        /// <param name="a"></param>
        public static void Normalize(ref Vector3 a)
        {
            double mag = System.Math.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);

            if (mag > 9.99999974737875E-06)
            {
                float magInv = (float)(1.0 / mag);
                a.x *= magInv;
                a.y *= magInv;
                a.z *= magInv;
            }
            else
            {
                a.x = 0.0f;
                a.y = 0.0f;
                a.z = 0.0f;
            }
        }

        /// <summary>
        /// Fast vector magnitude. It's consistently a bit faster than Vector3.magnitude.
        /// </summary>
        /// <param name="a"></param>
        public static float Magnitude(ref Vector3 a)
        {
            return (float)System.Math.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);
        }

        /// <summary>
        /// Fast triangle area.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public static float TriangleArea(ref Vector3 a, ref Vector3 b, ref Vector3 c)
        {
            double cx = c.x - a.x;
            double cy = c.y - a.y;
            double cz = c.z - a.z;

            double bx = b.x - a.x;
            double by = b.y - a.y;
            double bz = b.z - a.z;

            double t1 = by * cz - bz * cy;
            double t2 = bz * cx - bx * cz;
            double t3 = bx * cy - by * cx;

            return (float)(System.Math.Sqrt(t1 * t1 + t2 * t2 + t3 * t3) * 0.5);
        }

        /// <summary>
        /// Almost twice as fast as Matrix4x4.MultiplyVector.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void MultiplyVector(ref Matrix4x4 matrix, ref Vector3 a, ref Vector3 b)
        {
            b.x = matrix.m00 * a.x + matrix.m01 * a.y + matrix.m02 * a.z;
            b.y = matrix.m10 * a.x + matrix.m11 * a.y + matrix.m12 * a.z;
            b.z = matrix.m20 * a.x + matrix.m21 * a.y + matrix.m22 * a.z;
        }

        /// <summary>
        /// Almost twice as fast as Matrix4x4.MultiplyPoint3x4.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void MultiplyPoint(ref Matrix4x4 matrix, ref Vector3 a, ref Vector3 b)
        {
            b.x = matrix.m00 * a.x + matrix.m01 * a.y + matrix.m02 * a.z + matrix.m03;
            b.y = matrix.m10 * a.x + matrix.m11 * a.y + matrix.m12 * a.z + matrix.m13;
            b.z = matrix.m20 * a.x + matrix.m21 * a.y + matrix.m22 * a.z + matrix.m23;
        }

        /// <summary>
        /// Projects target on a-b line.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Vector2 ProjectOntoLine(Vector2 a, Vector2 b, Vector2 target)
        {
            Vector2 u = b - a;
            Vector2 v = target - a;

            return a + Vector2.Dot(u, v) * u / u.sqrMagnitude;
        }

        /// <summary>
        /// Computes distance from target to a-b line.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float DistanceToLine(Vector3 a, Vector3 b, Vector3 target)
        {
            Vector3 u = b - a;
            Vector3 v = target - a;

            Vector3 p = a + Vector3.Dot(u, v) * u / u.sqrMagnitude;

            return Vector3.Distance(p, target);
        }

        /// <summary>
        /// Computes distance from target to a-b segment.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float DistanceToSegment(Vector3 a, Vector3 b, Vector3 target)
        {
            Vector3 u = b - a;
            Vector3 v = target - a;

            Vector3 p = a + Vector3.Dot(u, v) * u / u.sqrMagnitude;

            if (Vector3.Dot((a - p).normalized, (b - p).normalized) < 0.0f)
                return Vector3.Distance(p, target);
            else
                return Mathf.Min(v.magnitude, Vector3.Distance(b, target));
        }

        /// <summary>
        /// Computes distance from target to a-b segment.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float DistanceToSegment(Vector2 a, Vector2 b, Vector2 target)
        {
            Vector2 u = b - a;
            Vector2 v = target - a;

            Vector2 p = a + Vector2.Dot(u, v) * u / u.sqrMagnitude;

            if (Vector2.Dot((a - p).normalized, (b - p).normalized) < 0.0f)
                return Vector2.Distance(p, target);
            else
                return Mathf.Min(v.magnitude, Vector2.Distance(b, target));
        }

        /// <summary>
        /// Finds the closest point to target on a-b segment.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Vector2 ClosestPointOnSegment(Vector2 a, Vector2 b, Vector2 target)
        {
            Vector2 u = b - a;
            Vector2 v = target - a;

            Vector2 p = a + Vector2.Dot(u, v) * u / u.sqrMagnitude;

            if (Vector2.Dot((a - p).normalized, (b - p).normalized) < 0.0f)
                return p;
            else if (Vector2.Distance(a, target) < Vector2.Distance(b, target))
                return a;
            else
                return b;
        }

        /// <summary>
        /// Checks if target is inside a-b-c triangle.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsPointInsideTriangle(Vector2 a, Vector2 b, Vector2 c, Vector2 target)
        {
            float diffx = target.x - a.x;
            float diffy = target.y - a.y;
            bool ab = (b.x - a.x) * diffy - (b.y - a.y) * diffx > 0;

            if ((c.x - a.x) * diffy - (c.y - a.y) * diffx > 0 == ab)
                return false;

            if ((c.x - b.x) * (target.y - b.y) - (c.y - b.y) * (target.x - b.x) > 0 != ab)
                return false;

            return true;
        }

        /// <summary>
        /// Returns random number with gaussian distribution.
        /// </summary>
        /// <returns></returns>
        public static float Gauss01()
        {
            return Mathf.Sqrt(-2.0f * Mathf.Log(Random.Range(0.000001f, 1.0f))) * Mathf.Sin(_PIx2 * Random.value);
        }

        /// <summary>
        /// Returns random number with gaussian distribution.
        /// </summary>
        /// <returns></returns>
        public static float Gauss(float mean, float stdDev)
        {
            return mean + stdDev * Mathf.Sqrt(-2.0f * Mathf.Log(Random.Range(0.000001f, 1.0f))) * Mathf.Sin(_PIx2 * Random.value);
        }

        /// <summary>
        /// Returns random number with gaussian distribution.
        /// </summary>
        /// <returns></returns>
        public static float Gauss01(float u1, float u2)
        {
            return Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(_PIx2 * u2);
        }

        public static float FracAdditive(float value)
        {
            return value - (int)value;
        }

        public static Vector2 Rotate(Vector2 vector, float angle)
        {
            float s, c;
            SinCos2048(angle, out s, out c);

            return new Vector2(
                    vector.x * c + vector.y * s,
                    vector.x * s + vector.y * c
                );
        }

        // Internal implementation of the function uses different naming scheme,
        // we'll not change it here
        // ReSharper disable InconsistentNaming
        public static float Interpolate(float a0, float a1, float a2, float a3, float b0, float b1, float b2, float b3, float fx, float invFx, float fy, float invFy, float t)
        {
            float A0 = a0 * fx + a1 * invFx;
            float A1 = a2 * fx + a3 * invFx;
            float A = A0 * fy + A1 * invFy;

            float B0 = b0 * fx + b1 * invFx;
            float B1 = b2 * fx + b3 * invFx;
            float B = B0 * fy + B1 * invFy;

            return A * (1.0f - t) + B * t;
        }

        // Passing by ref is noticeably faster in this case
        public static Vector2 Interpolate(ref Vector2 a0, ref Vector2 a1, ref Vector2 a2, ref Vector2 a3, ref Vector2 b0, ref Vector2 b1, ref Vector2 b2, ref Vector2 b3, float fx, float invFx, float fy, float invFy, float t)
        {
            if (fx != 0.0f)
            {
                float propX = invFx / fx;

                float A0x = a0.x + a1.x * propX;
                float A0y = a0.y + a1.y * propX;

                float A1x = a2.x + a3.x * propX;
                float A1y = a2.y + a3.y * propX;

                float Ax = A0x * fy + A1x * invFy;
                float Ay = A0y * fy + A1y * invFy;

                float B0x = b0.x + b1.x * propX;
                float B0y = b0.y + b1.y * propX;

                float B1x = b2.x + b3.x * propX;
                float B1y = b2.y + b3.y * propX;

                float Bx = B0x * fy + B1x * invFy;
                float By = B0y * fy + B1y * invFy;

                float invT = (1.0f - t) * fx;
                t *= fx;

                return new Vector2(Ax * invT + Bx * t, Ay * invT + By * t);
            }
            else
            {
                float Ax = a1.x * fy + a3.x * invFy;
                float Ay = a1.y * fy + a3.y * invFy;
                float Bx = b1.x * fy + b3.x * invFy;
                float By = b1.y * fy + b3.y * invFy;

                float invT = (1.0f - t);

                return new Vector2(Ax * invT + Bx * t, Ay * invT + By * t);
            }
        }

        public static Vector2 Interpolate(Vector2 a0, Vector2 a1, Vector2 a2, Vector2 a3, Vector2 b0, Vector2 b1, Vector2 b2, Vector2 b3, float fx, float invFx, float fy, float invFy, float t)
        {
            Vector2 A0 = a0 * fx + a1 * invFx;
            Vector2 A1 = a2 * fx + a3 * invFx;
            Vector2 A = A0 * fy + A1 * invFy;

            Vector2 B0 = b0 * fx + b1 * invFx;
            Vector2 B1 = b2 * fx + b3 * invFx;
            Vector2 B = B0 * fy + B1 * invFy;

            return A * (1.0f - t) + B * t;
        }

        public static Vector3 Interpolate(Vector3 a0, Vector3 a1, Vector3 a2, Vector3 a3, Vector3 b0, Vector3 b1, Vector3 b2, Vector3 b3, float fx, float invFx, float fy, float invFy, float t)
        {
            Vector3 A0 = a0 * fx + a1 * invFx;
            Vector3 A1 = a2 * fx + a3 * invFx;
            Vector3 A = A0 * fy + A1 * invFy;

            Vector3 B0 = b0 * fx + b1 * invFx;
            Vector3 B1 = b2 * fx + b3 * invFx;
            Vector3 B = B0 * fy + B1 * invFy;

            return A * (1.0f - t) + B * t;
        }

        public static Vector4 Interpolate(ref Vector4 a0, ref Vector4 a1, ref Vector4 a2, ref Vector4 a3, ref Vector4 b0, ref Vector4 b1, ref Vector4 b2, ref Vector4 b3, float fx, float invFx, float fy, float invFy, float t)
        {
            if (fx != 0.0f)
            {
                float propX = invFx / fx;

                float A0x = a0.x + a1.x * propX;
                float A0y = a0.y + a1.y * propX;
                float A0z = a0.z + a1.z * propX;
                float A0w = a0.w + a1.w * propX;

                float A1x = a2.x + a3.x * propX;
                float A1y = a2.y + a3.y * propX;
                float A1z = a2.z + a3.z * propX;
                float A1w = a2.w + a3.w * propX;

                float Ax = A0x * fy + A1x * invFy;
                float Ay = A0y * fy + A1y * invFy;
                float Az = A0z * fy + A1z * invFy;
                float Aw = A0w * fy + A1w * invFy;

                float B0x = b0.x + a1.x * propX;
                float B0y = b0.y + a1.y * propX;
                float B0z = b0.z + a1.z * propX;
                float B0w = b0.w + a1.w * propX;

                float B1x = b2.x + a3.x * propX;
                float B1y = b2.y + a3.y * propX;
                float B1z = b2.z + a3.z * propX;
                float B1w = b2.w + a3.w * propX;

                float Bx = B0x * fy + B1x * invFy;
                float By = B0y * fy + B1y * invFy;
                float Bz = B0z * fy + B1z * invFy;
                float Bw = B0w * fy + B1w * invFy;

                float invT = (1.0f - t) * fx;
                t *= fx;

                return new Vector4(Ax * invT + Bx * t, Ay * invT + By * t, Az * invT + Bz * t, Aw * invT + Bw * t);
            }
            else
            {
                float Ax = a1.x * fy + a3.x * invFy;
                float Ay = a1.y * fy + a3.y * invFy;
                float Az = a1.z * fy + a3.z * invFy;
                float Aw = a1.w * fy + a3.w * invFy;
                float Bx = b1.x * fy + b3.x * invFy;
                float By = b1.y * fy + b3.y * invFy;
                float Bz = b1.z * fy + b3.z * invFy;
                float Bw = b1.w * fy + b3.w * invFy;

                float invT = (1.0f - t) * fx;
                t *= fx;

                return new Vector4(Ax * invT + Bx * t, Ay * invT + By * t, Az * invT + Bz * t, Aw * invT + Bw * t);
            }
        }

        public static Vector4 Interpolate(Vector4 a0, Vector4 a1, Vector4 a2, Vector4 a3, Vector4 b0, Vector4 b1, Vector4 b2, Vector4 b3, float fx, float invFx, float fy, float invFy, float t)
        {
            if (fx != 0.0f)
            {
                float propX = invFx / fx;

                Vector4 A0 = a0 + a1 * propX;
                Vector4 A1 = a2 + a3 * propX;
                Vector4 A = A0 * fy + A1 * invFy;

                Vector4 B0 = b0 + b1 * propX;
                Vector4 B1 = b2 + b3 * propX;
                Vector4 B = B0 * fy + B1 * invFy;

                return A * ((1.0f - t) * fx) + B * (t * fx);
            }
            else
            {
                Vector4 A = a1 * fy + a3 * invFy;
                Vector4 B = b1 * fy + b3 * invFy;
                return A * (1.0f - t) + B * t;
            }
        }

#if WATER_SIMD
        static public Vector4f Interpolate(Vector4f a0, Vector4f a1, Vector4f a2, Vector4f a3, Vector4f b0, Vector4f b1, Vector4f b2, Vector4f b3, float fxf, float invFxf, float fyf, float invFyf, float tf)
        {
            if(fxf != 0.0f)
            {
                Vector4f propX = new Vector4f(invFxf / fxf);
                Vector4f fy = new Vector4f(fyf);
                Vector4f invFy = new Vector4f(invFyf);

                Vector4f A0 = a0 + a1 * propX;
                Vector4f A1 = a2 + a3 * propX;
                Vector4f A = A0 * fy + A1 * invFy;

                Vector4f B0 = b0 + b1 * propX;
                Vector4f B1 = b2 + b3 * propX;
                Vector4f B = B0 * fy + B1 * invFy;

                Vector4f t = new Vector4f(tf * fxf);
                Vector4f invT = new Vector4f((1.0f - tf) * fxf);

                return A * invT + B * t;
            }
            else
            {
                Vector4f fy = new Vector4f(fyf);
                Vector4f invFy = new Vector4f(invFyf);

                Vector4f A = a1 * fy + a3 * invFy;
                Vector4f B = b1 * fy + b3 * invFy;

                Vector4f t = new Vector4f(tf);
                Vector4f invT = new Vector4f(1.0f - tf);

                return A * invT + B * t;
            }
        }
#endif
        // ReSharper restore InconsistentNaming
        #endregion Public Methods

        #region Private Variables
        private const float _PIx2 = 2.0f * Mathf.PI;
        #endregion Private Variables
    }
}