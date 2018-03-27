using System;
using UnityEngine;

namespace UltimateWater
{
    /// <summary>
    /// Water height sampling methods (moved from Water.cs)
    /// </summary>
    public sealed partial class Water
    {
        /// <summary>
        /// Computes water displacement vector at a given coordinates. WaterSample class does the same thing asynchronously and is recommended.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public Vector3 GetDisplacementAt(float x, float z)
        {
            Vector3 result = new Vector3();

            if (WindWaves != null)
            {
                CompensateHorizontalDisplacement(ref x, ref z);
                result = WindWaves.SpectrumResolver.GetDisplacementAt(x, z, _Time);
            }

            return result;
        }

        /// <summary>
        /// Computes water displacement vector at a given coordinates. WaterSample class does the same thing asynchronously and is recommended.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public Vector3 GetDisplacementAt(float x, float z, float time)
        {
            Vector3 result = new Vector3();

            if (WindWaves != null)
            {
                CompensateHorizontalDisplacement(ref x, ref z);
                result = WindWaves.SpectrumResolver.GetDisplacementAt(x, z, time);
            }

            return result;
        }

        /// <summary>
        /// Computes uncompensated water displacement vector at a given coordinates. WaterSample class does the same thing asynchronously and is recommended.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public Vector3 GetUncompensatedDisplacementAt(float x, float z, float time)
        {
            return WindWaves != null ? WindWaves.SpectrumResolver.GetDisplacementAt(x, z, time) : new Vector3();
        }

        /// <summary>
        /// Computes horizontal displacement vector at a given coordinates. WaterSample class does the same thing asynchronously and is recommended.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public Vector2 GetHorizontalDisplacementAt(float x, float z)
        {
            Vector2 result = new Vector2();

            if (WindWaves != null)
            {
                CompensateHorizontalDisplacement(ref x, ref z);
                result = WindWaves.SpectrumResolver.GetHorizontalDisplacementAt(x, z, _Time);
            }

            return result;
        }

        /// <summary>
        /// Computes horizontal displacement vector at a given coordinates. WaterSample class does the same thing asynchronously and is recommended.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public Vector2 GetHorizontalDisplacementAt(float x, float z, float time)
        {
            Vector2 result = new Vector2();

            if (WindWaves != null)
            {
                CompensateHorizontalDisplacement(ref x, ref z);
                result = WindWaves.SpectrumResolver.GetHorizontalDisplacementAt(x, z, time);
            }

            return result;
        }

        /// <summary>
        /// Computes uncompensated horizontal displacement vector at a given coordinates. WaterSample class does the same thing asynchronously and is recommended.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public Vector2 GetUncompensatedHorizontalDisplacementAt(float x, float z, float time)
        {
            return WindWaves != null ? WindWaves.SpectrumResolver.GetHorizontalDisplacementAt(x, z, time) : new Vector2();
        }

        /// <summary>
        /// Computes height at a given coordinates. WaterSample class does the same thing asynchronously and is recommended.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public float GetHeightAt(float x, float z)
        {
            float result = 0.0f;

            if (WindWaves != null)
            {
                CompensateHorizontalDisplacement(ref x, ref z);
                result = WindWaves.SpectrumResolver.GetHeightAt(x, z, _Time);
            }

            return result;
        }

        /// <summary>
        /// Computes compensated height at a given coordinates. WaterSample class does the same thing asynchronously and is recommended.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public float GetHeightAt(float x, float z, float time)
        {
            float result = 0.0f;

            if (WindWaves != null)
            {
                CompensateHorizontalDisplacement(ref x, ref z);
                result = WindWaves.SpectrumResolver.GetHeightAt(x, z, time);
            }

            return result;
        }

        /// <summary>
        /// Computes uncompensated height at a given coordinates. WaterSample class does the same thing asynchronously and is recommended.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public float GetUncompensatedHeightAt(float x, float z, float time)
        {
            return WindWaves != null ? WindWaves.SpectrumResolver.GetHeightAt(x, z, time) : 0.0f;
        }

        /// <summary>
        /// Computes compensated forces and height at a given coordinates. WaterSample class does the same thing asynchronously and is recommended.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public Vector4 GetHeightAndForcesAt(float x, float z)
        {
            Vector4 result = Vector4.zero;

            if (WindWaves != null)
            {
                CompensateHorizontalDisplacement(ref x, ref z);
                result = WindWaves.SpectrumResolver.GetForceAndHeightAt(x, z, _Time);
            }

            return result;
        }

        /// <summary>
        /// Computes forces and height at a given coordinates. WaterSample class does the same thing asynchronously and is recommended.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public Vector4 GetHeightAndForcesAt(float x, float z, float time)
        {
            Vector4 result = Vector4.zero;

            if (WindWaves != null)
            {
                CompensateHorizontalDisplacement(ref x, ref z);
                result = WindWaves.SpectrumResolver.GetForceAndHeightAt(x, z, time);
            }

            return result;
        }

        /// <summary>
        /// Computes uncompensated forces and height at a given coordinates. WaterSample class does the same thing asynchronously and is recommended.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public Vector4 GetUncompensatedHeightAndForcesAt(float x, float z, float time)
        {
            return WindWaves != null ? WindWaves.SpectrumResolver.GetForceAndHeightAt(x, z, time) : new Vector4();
        }

        [Obsolete("Use this overload instead: GetDisplacementAt(float x, float z, float time).")]
        public Vector3 GetDisplacementAt(float x, float z, float spectrumStart, float spectrumEnd, float time)
        {
            Vector3 result = new Vector3();

            if (WindWaves != null)
                result = WindWaves.SpectrumResolver.GetDisplacementAt(x, z, time);

            return result;
        }

        [Obsolete("Use this overload instead: GetHorizontalDisplacementAt(float x, float z, float time).")]
        public Vector2 GetHorizontalDisplacementAt(float x, float z, float spectrumStart, float spectrumEnd, float time)
        {
            Vector2 result = new Vector2();

            if (WindWaves != null)
                result = WindWaves.SpectrumResolver.GetHorizontalDisplacementAt(x, z, time);

            return result;
        }

        [Obsolete("Use this overload instead: GetHeightAt(float x, float z, float time).")]
        public float GetHeightAt(float x, float z, float spectrumStart, float spectrumEnd, float time)
        {
            float result = 0.0f;

            if (WindWaves != null)
                result = WindWaves.SpectrumResolver.GetHeightAt(x, z, time);

            return result;
        }

        [Obsolete("Use this overload instead: GetHeightAndForcesAt(float x, float z, float time).")]
        public Vector4 GetHeightAndForcesAt(float x, float z, float spectrumStart, float spectrumEnd, float time)
        {
            Vector4 result = Vector4.zero;

            if (WindWaves != null)
                result = WindWaves.SpectrumResolver.GetForceAndHeightAt(x, z, time);

            return result;
        }

        public void CompensateHorizontalDisplacement(ref float x, ref float z, float errorTolerance = 0.045f)
        {
            float originalx = x;
            float originalz = z;

            var spectrumResolver = WindWaves.SpectrumResolver;
            Vector2 offset = spectrumResolver.GetHorizontalDisplacementAt(x, z, _Time);

            x -= offset.x;
            z -= offset.y;

            if (offset.x > errorTolerance || offset.y > errorTolerance || offset.x < -errorTolerance || offset.y < -errorTolerance)
            {
                for (int i = 0; i < 14; ++i)
                {
                    offset = spectrumResolver.GetHorizontalDisplacementAt(x, z, _Time);

                    float dx = originalx - (x + offset.x);
                    float dz = originalz - (z + offset.y);
                    x += dx * _CompensationStepWeights[i];
                    z += dz * _CompensationStepWeights[i];

                    if (dx < errorTolerance && dz < errorTolerance && dx > -errorTolerance && dz > -errorTolerance)
                        break;
                }
            }
        }
    }
}