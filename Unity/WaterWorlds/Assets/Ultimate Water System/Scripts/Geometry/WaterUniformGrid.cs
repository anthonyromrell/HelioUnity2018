namespace UltimateWater.Internal
{
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class WaterUniformGrid : WaterPrimitiveBase
    {
        protected override Mesh[] CreateMeshes(int vertexCount, bool volume)
        {
            int dim = Mathf.RoundToInt(Mathf.Sqrt(vertexCount));
            var meshes = new List<Mesh>();
            var vertices = new List<Vector3>();
            var indices = new List<int>();
            int vertexIndex = 0;
            int meshIndex = 0;

            for (int y = 0; y < dim; ++y)
            {
                float fy = (float)y / (dim - 1) * 2.0f - 1.0f;

                for (int x = 0; x < dim; ++x)
                {
                    float fx = (float)x / (dim - 1) * 2.0f - 1.0f;

                    if (volume && (x == 0 || y == 0 || x == dim - 1 || y == dim - 1))
                        vertices.Add(new Vector3(0.0f, -0.2f, 0.0f));
                    else
                        vertices.Add(new Vector3(fx, 0.0f, fy));

                    if (x != 0 && y != 0 && vertexIndex > dim)
                    {
                        indices.Add(vertexIndex);
                        indices.Add(vertexIndex - dim);
                        indices.Add(vertexIndex - dim - 1);
                        indices.Add(vertexIndex - 1);
                    }

                    ++vertexIndex;

                    if (vertexIndex == 65000)
                    {
                        var mesh = CreateMesh(vertices.ToArray(), indices.ToArray(), string.Format("Uniform Grid {0}x{1} - {2:00}", dim, dim, meshIndex));
                        meshes.Add(mesh);

                        --x; --y;

                        fy = (float)y / (dim - 1) * 2.0f - 1.0f;

                        vertexIndex = 0;
                        vertices.Clear();
                        indices.Clear();

                        ++meshIndex;
                    }
                }
            }

            if (vertexIndex != 0)
            {
                var mesh = CreateMesh(vertices.ToArray(), indices.ToArray(), string.Format("Uniform Grid {0}x{1} - {2:00}", dim, dim, meshIndex));
                meshes.Add(mesh);
            }

            return meshes.ToArray();
        }

        protected override Matrix4x4 GetMatrix(Camera camera)
        {
            Transform cameraTransform = camera.transform;
            float waterPositionY = _Water.transform.position.y;
            Vector3 center, scale;

            if (camera.orthographic)
            {
                Vector3 position = cameraTransform.position;
                Vector3 forward = cameraTransform.forward;
                float d = (waterPositionY - position.y) / forward.y;

                if (d > 0)
                {
                    center = position + forward * d;
                }
                else
                {
                    center = position;
                    center.y = waterPositionY;
                }

                float orthographicSize = camera.orthographicSize;
                float maxHorizontalDisplacement = _Water.MaxHorizontalDisplacement;
                float scaleWithMargin = orthographicSize + maxHorizontalDisplacement;

                scale = new Vector3(scaleWithMargin * 2.0f, scaleWithMargin * 2.0f, orthographicSize / -forward.y + maxHorizontalDisplacement);
            }
            else
            {
                center = cameraTransform.position;
                center.y = waterPositionY;
                scale = new Vector3(camera.farClipPlane * Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad), camera.farClipPlane, camera.farClipPlane);
            }

            return Matrix4x4.TRS(center, Quaternion.AngleAxis(cameraTransform.eulerAngles.y, Vector3.up), scale);
        }
    }
}