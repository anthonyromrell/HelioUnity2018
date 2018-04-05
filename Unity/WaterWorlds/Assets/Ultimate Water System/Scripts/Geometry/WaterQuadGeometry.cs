namespace UltimateWater
{
    using UnityEngine;
    using Internal;

    public class WaterQuadGeometry : WaterPrimitiveBase
    {
        #region Public Methods
        public override Mesh[] GetTransformedMeshes(Camera camera, out Matrix4x4 matrix, int vertexCount, bool volume)
        {
            matrix = GetMatrix(camera);
            return _Meshes != null ? _Meshes : (_Meshes = new[] { Quads.BipolarXZ });
        }
        #endregion Public Methods

        #region Private Variables
        private Mesh[] _Meshes;
        #endregion Private Variables

        #region Private Methods
        protected override Matrix4x4 GetMatrix(Camera camera)
        {
            Vector3 position = camera.transform.position;
            float farClipPlane = camera.farClipPlane;

            Matrix4x4 matrix = new Matrix4x4
            {
                m03 = position.x,
                m13 = position.y,
                m23 = position.z,
                m00 = farClipPlane,
                m11 = farClipPlane,
                m22 = farClipPlane
            };

            return matrix;
        }

        protected override Mesh[] CreateMeshes(int vertexCount, bool volume)
        {
            return new[] { Quads.BipolarXZ };
        }
        #endregion Private Methods
    }
}