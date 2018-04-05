namespace UltimateWater.Utils
{
    using UnityEngine;

    public static class Math
    {
        public static bool IsNaN(this Vector3 vector)
        {
            return float.IsNaN(vector.x) || float.IsNaN(vector.y) || float.IsNaN(vector.z);
        }

        #region Public Methods
        public static Vector3 RaycastPlane(Camera camera, float planeHeight, Vector3 pos)
        {
            Ray ray = camera.ViewportPointToRay(pos);
            Vector3 direction = ray.direction;

            if (camera.transform.position.y > planeHeight)
            {
                if (direction.y > -0.01f)
                    direction.y = -direction.y - 0.02f;
            }
            else if (direction.y < 0.01f)
                direction.y = -direction.y + 0.02f;

            float t = -(ray.origin.y - planeHeight) / direction.y;
            float angle = -camera.transform.eulerAngles.y * Mathf.Deg2Rad;
            float s = Mathf.Sin(angle);
            float c = Mathf.Cos(angle);

            return new Vector3(
                t * (direction.x * c + direction.z * s),
                t * direction.y,
                t * (direction.x * s + direction.z * c)
            );
        }

        public static Vector3 ViewportWaterPerpendicular(Camera camera)
        {
            Vector3 down = camera.worldToCameraMatrix.MultiplyVector(new Vector3(0.0f, -1.0f, 0.0f));

            // normalize, mul 0.5
            float lenInv = 0.5f / Mathf.Sqrt(down.x * down.x + down.y * down.y);
            down.x = down.x * lenInv + 0.5f;
            down.y = down.y * lenInv + 0.5f;

            return down;
        }
        public static Vector3 ViewportWaterRight(Camera camera)
        {
            Vector3 right = camera.worldToCameraMatrix.MultiplyVector(camera.transform.right);

            // normalize, mul 0.5
            float lenInv = 0.5f / Mathf.Sqrt(right.x * right.x + right.y * right.y);
            right.x *= lenInv;
            right.y *= lenInv;

            return right;
        }

        public static float[] GaussianTerms(float sigma)
        {
            // hardcoded to prevent memory allocation
            int kernelSize = 3;
            var terms = _GaussianTerms;

            float sum = 0.0f;

            for (int i = 0; i < kernelSize; ++i)
            {
                terms[i] = Gaussian(i - kernelSize / 2, sigma);
                sum += terms[i];
            }

            for (int i = 0; i < kernelSize; ++i)
            {
                terms[i] /= sum;
            }

            return terms;
        }
        #endregion Public Methods

        #region Private Variables
        private static readonly float[] _GaussianTerms = new float[3];
        #endregion Private Variables

        #region Private Methods
        private static float Gaussian(int x, float sigma)
        {
            var c = 2.0f * sigma * sigma;
            return Mathf.Exp(-x * x / c) / (c * Mathf.PI);
        }
        #endregion Private Methods
    }
}