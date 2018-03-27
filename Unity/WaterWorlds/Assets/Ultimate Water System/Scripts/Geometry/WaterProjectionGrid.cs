namespace UltimateWater.Internal
{
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class WaterProjectionGrid : WaterPrimitiveBase
    {
        #region Public Methods
        public override Mesh[] GetTransformedMeshes(Camera camera, out Matrix4x4 matrix, int vertexCount, bool volume)
        {
            int pixelWidth = camera.pixelWidth;
            int pixelHeight = camera.pixelHeight;

            CachedMeshSet cachedMeshSet;
            int hash = pixelHeight | (pixelWidth << 16);
            Vector3 cameraPosition = camera.transform.position;
            matrix = Matrix4x4.identity;
            matrix.m03 = cameraPosition.x;
            matrix.m13 = 0.0f;
            matrix.m23 = cameraPosition.z;

            float verticesPerPixel = (float)vertexCount / (pixelWidth * pixelHeight);

            _Water.Renderer.PropertyBlock.SetMatrix("_InvViewMatrix", camera.cameraToWorldMatrix);

            if (!_Cache.TryGetValue(hash, out cachedMeshSet))
                _Cache[hash] = cachedMeshSet = new CachedMeshSet(CreateMeshes(Mathf.RoundToInt(pixelWidth * verticesPerPixel), Mathf.RoundToInt(pixelHeight * verticesPerPixel)));

            return cachedMeshSet.Meshes;
        }
        #endregion Public Methods

        #region Private Variables
        private const string _ProjectionGridKeyword = "_PROJECTION_GRID";
        #endregion Private Variables

        #region Private Methods
        private Mesh[] CreateMeshes(int verticesX, int verticesY)
        {
            List<Mesh> meshes = new List<Mesh>();

            List<Vector3> vertices = new List<Vector3>();
            List<int> indices = new List<int>();
            int vertexIndex = 0;
            int meshIndex = 0;

            for (int y = 0; y < verticesY; ++y)
            {
                float fy = (float)y / (verticesY - 1);

                for (int x = 0; x < verticesX; ++x)
                {
                    float fx = (float)x / (verticesX - 1);

                    vertices.Add(new Vector3(fx, fy, 0.0f));

                    if (x != 0 && y != 0 && vertexIndex > verticesX)
                    {
                        indices.Add(vertexIndex);
                        indices.Add(vertexIndex - 1);
                        indices.Add(vertexIndex - verticesX - 1);
                        indices.Add(vertexIndex - verticesX);
                    }

                    ++vertexIndex;

                    if (vertexIndex == 65000)
                    {
                        var mesh = CreateMesh(vertices.ToArray(), indices.ToArray(), string.Format("Projection Grid {0}x{1} - {2:00}", verticesX, verticesY, meshIndex));
                        mesh.bounds = new Bounds(Vector3.zero, new Vector3(100000.0f, 0.2f, 100000.0f));
                        meshes.Add(mesh);

                        --x; --y;

                        fy = (float)y / (verticesY - 1);

                        vertexIndex = 0;
                        vertices.Clear();
                        indices.Clear();

                        ++meshIndex;
                    }
                }
            }

            if (vertexIndex != 0)
            {
                var mesh = CreateMesh(vertices.ToArray(), indices.ToArray(), string.Format("Projection Grid {0}x{1} - {2:00}", verticesX, verticesY, meshIndex));
                mesh.bounds = new Bounds(Vector3.zero, new Vector3(100000.0f, 0.2f, 100000.0f));
                meshes.Add(mesh);
            }

            return meshes.ToArray();
        }
        protected override Matrix4x4 GetMatrix(Camera camera)
        {
            throw new System.InvalidOperationException();
        }
        protected override Mesh[] CreateMeshes(int vertexCount, bool volume)
        {
            throw new System.InvalidOperationException();
        }
        internal override void OnEnable(Water water)
        {
            base.OnEnable(water);
            _Water = water;
        }
        internal override void AddToMaterial(Water water)
        {
            water.Materials.SetKeyword(_ProjectionGridKeyword, true);
        }
        internal override void RemoveFromMaterial(Water water)
        {
            water.Materials.SetKeyword(_ProjectionGridKeyword, false);
        }
        #endregion Private Methods
    }
}