namespace UltimateWater.Internal
{
    using UnityEngine;
    using UnityEngine.Rendering;

    public static class GraphicsUtilities
    {
        #region Public Methods
        public static void Blit(Texture source, RenderTexture target, Material material, int shaderPass, MaterialPropertyBlock properties)
        {
            if (_CommandBuffer == null)
            {
                _CommandBuffer = new CommandBuffer { name = "UltimateWater Custom Blit" };
            }

            GL.PushMatrix();
            GL.modelview = Matrix4x4.identity;
            GL.LoadProjectionMatrix(Matrix4x4.identity);

            material.mainTexture = source;
            _CommandBuffer.SetRenderTarget(target);
            _CommandBuffer.DrawMesh(Quads.BipolarXY, Matrix4x4.identity, material, 0, shaderPass, properties);

            Graphics.ExecuteCommandBuffer(_CommandBuffer);

            // setting target permanently seems to be the default behaviour of regular Graphics.Blit and without it some problems arise
            RenderTexture.active = target;
            GL.PopMatrix();

            _CommandBuffer.Clear();
        }
        #endregion Public Methods

        #region Private Variables
        private static CommandBuffer _CommandBuffer;
        #endregion Private Variables
    }
}