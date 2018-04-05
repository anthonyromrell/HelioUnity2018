using UnityEngine;

namespace UltimateWater.Internal
{
    /// <summary>
    /// An utility class that creates and caches some quads used for water rendering.
    /// </summary>
    public class Quads
    {
        #region Public Variables
        public static Mesh BipolarXY
        {
            get
            {
                if (!_Initialized)
                    CreateMeshes();

                return _BipolarXY;
            }
        }

        public static Mesh BipolarXInversedY
        {
            get
            {
                if (!_Initialized)
                    CreateMeshes();

                return _BipolarXInversedY;
            }
        }

        public static Mesh BipolarXZ
        {
            get
            {
                if (!_Initialized)
                    CreateMeshes();

                return _BipolarXZ;
            }
        }
        #endregion Public Variables

        #region Private Variables
        private static Mesh _BipolarXY;
        private static Mesh _BipolarXInversedY;
        private static Mesh _BipolarXZ;
        private static bool _Initialized;
        #endregion Private Variables

        #region Private Methods
        private static void CreateMeshes()
        {
            _BipolarXY = CreateBipolarXY(false);
            _BipolarXInversedY = CreateBipolarXY(SystemInfo.graphicsDeviceVersion.Contains("Direct3D"));
            _BipolarXZ = CreateBipolarXZ();
            _Initialized = true;
        }

        private static Mesh CreateBipolarXY(bool inversedY)
        {
            var mesh = new Mesh
            {
                hideFlags = HideFlags.DontSave,
                vertices = new[]
                {
                    new Vector3(-1.0f, -1.0f, 0.0f),
                    new Vector3(1.0f, -1.0f, 0.0f),
                    new Vector3(1.0f, 1.0f, 0.0f),
                    new Vector3(-1.0f, 1.0f, 0.0f)
                },
                uv = new[]
                {
                    new Vector2(0.0f, inversedY ? 1.0f : 0.0f),
                    new Vector2(1.0f, inversedY ? 1.0f : 0.0f),
                    new Vector2(1.0f, inversedY ? 0.0f : 1.0f),
                    new Vector2(0.0f, inversedY ? 0.0f : 1.0f)
                }
            };

            mesh.SetTriangles(new[] { 0, 1, 2, 0, 2, 3 }, 0);
            mesh.UploadMeshData(true);
            return mesh;
        }

        private static Mesh CreateBipolarXZ()
        {
            var quadMesh = new Mesh
            {
                name = "Shoreline Quad Mesh",
                hideFlags = HideFlags.DontSave,
                vertices = new[]
                {
                    new Vector3(-1.0f, 0.0f, -1.0f),
                    new Vector3(-1.0f, 0.0f, 1.0f),
                    new Vector3(1.0f, 0.0f, 1.0f),
                    new Vector3(1.0f, 0.0f, -1.0f)
                },
                uv = new[]
                {
                    new Vector2(0.0f, 0.0f),
                    new Vector2(0.0f, 1.0f),
                    new Vector2(1.0f, 1.0f),
                    new Vector2(1.0f, 0.0f)
                }
            };

            quadMesh.SetIndices(new[] { 0, 1, 2, 3 }, MeshTopology.Quads, 0);
            quadMesh.UploadMeshData(true);

            return quadMesh;
        }
        #endregion Private Methods
    }
}