using UnityEngine.Serialization;

namespace UltimateWater
{
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class WaterCustomSurfaceMeshes
    {
        #region Public Variables
        /// <summary>
        /// Retrieves auto-generated boundary meshes used for underwater detection etc.
        /// </summary>
        public Mesh[] VolumeMeshes
        {
            get
            {
                if (_VolumeMeshes == null)
                {
                    var usedMeshes = _UsedMeshes;
                    var list = new List<Mesh>();

                    foreach (var mesh in usedMeshes)
                    {
                        list.Add(mesh);
                        list.Add(CreateBoundaryMesh(mesh));
                    }

                    _VolumeMeshes = list.ToArray();
                }

                return _VolumeMeshes;
            }
        }
        public bool Triangular
        {
            get { return _CustomMeshes == null || _UsedMeshes.Length == 0 || _UsedMeshes[0].GetTopology(0) == MeshTopology.Triangles; }
        }
        public Mesh[] Meshes
        {
            get { return _CustomMeshes; }
            set
            {
                _CustomMeshes = value;
                _UsedMeshesCache = null;
                _VolumeMeshes = null;
            }
        }
        #endregion Public Variables

        #region Public Methods
        public Mesh[] GetTransformedMeshes(Camera camera, out Matrix4x4 matrix, bool volume)
        {
            matrix = _Water.transform.localToWorldMatrix;

            if (volume)
                return VolumeMeshes;
            else
                return _UsedMeshes;
        }

        public void Dispose()
        {
            if (_VolumeMeshes != null)
            {
                for (int i = 1; i < _VolumeMeshes.Length; i += 2)
                    _VolumeMeshes[i].Destroy();

                _VolumeMeshes = null;
            }

            _UsedMeshesCache = null;
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField, FormerlySerializedAs("customMeshes")]
        private Mesh[] _CustomMeshes;
        #endregion Inspector Variables

        #region Private Types
        private struct Edge
        {
            public int VertexIndex0;
            public int VertexIndex1;

            public int FaceIndex0;
            public int FaceIndex1;
        }
        #endregion Private Types

        #region Private Variables
        private Mesh[] _UsedMeshes
        {
            get
            {
                if (_UsedMeshesCache == null)
                {
                    var list = new List<Mesh>();

                    foreach (var mesh in _CustomMeshes)
                    {
                        if (mesh != null)
                            list.Add(mesh);
                    }

                    _UsedMeshesCache = list.ToArray();
                }

                return _UsedMeshesCache;
            }
        }
        private Water _Water;
        private Mesh[] _UsedMeshesCache;
        private Mesh[] _VolumeMeshes;
        #endregion Private Variables

        #region Private Methods
        private Mesh CreateBoundaryMesh(Mesh sourceMesh)
        {
            var volumeMesh = new Mesh
            {
                hideFlags = HideFlags.DontSave
            };

            Vector3[] sourceVertices = sourceMesh.vertices;

            List<Vector3> targetVertices = new List<Vector3>();
            List<int> targetIndices = new List<int>();

            var edges = BuildManifoldEdges(sourceMesh);

            Vector3 center = new Vector3();
            int centerIndex = edges.Length * 4;

            for (int i = 0; i < edges.Length; ++i)
            {
                int vertexIndex = targetVertices.Count;

                Vector3 vertexA = sourceVertices[edges[i].VertexIndex0];
                Vector3 vertexB = sourceVertices[edges[i].VertexIndex1];

                targetVertices.Add(vertexA);
                targetVertices.Add(vertexB);

                // TODO: fix this by using vertex shader snapping to far plane and some big values here
                vertexA.y -= 1000.0f;
                vertexB.y -= 1000.0f;

                targetVertices.Add(vertexA);
                targetVertices.Add(vertexB);

                targetIndices.Add(vertexIndex + 3);
                targetIndices.Add(vertexIndex + 2);
                targetIndices.Add(vertexIndex);

                targetIndices.Add(vertexIndex + 3);
                targetIndices.Add(vertexIndex);
                targetIndices.Add(vertexIndex + 1);

                targetIndices.Add(vertexIndex + 3);
                targetIndices.Add(vertexIndex + 2);
                targetIndices.Add(centerIndex);

                center += vertexA;
                center += vertexB;
            }

            int divider = targetVertices.Count / 2;
            center /= divider;

            targetVertices.Add(center);

            volumeMesh.vertices = targetVertices.ToArray();
            volumeMesh.SetIndices(targetIndices.ToArray(), MeshTopology.Triangles, 0);

            return volumeMesh;
        }
        /// Source: Unity Technologies "Procedural Examples" from the Asset Store
        /// Builds an array of edges that connect to only one triangle.
        /// In other words, the outline of the mesh
        private static Edge[] BuildManifoldEdges(Mesh mesh)
        {
            // Build a edge list for all unique edges in the mesh
            Edge[] edges = BuildEdges(mesh.vertexCount, mesh.triangles);

            // We only want edges that connect to a single triangle
            List<Edge> culledEdges = new List<Edge>();
            foreach (Edge edge in edges)
            {
                if (edge.FaceIndex0 == edge.FaceIndex1)
                {
                    culledEdges.Add(edge);
                }
            }

            return culledEdges.ToArray();
        }
        /// Source: Unity Technologies "Procedural Examples" from the Asset Store
        /// Builds an array of unique edges
        /// This requires that your mesh has all vertices welded. However on import, Unity has to split
        /// vertices at uv seams and normal seams. Thus for a mesh with seams in your mesh you
        /// will get two edges adjoining one triangle.
        /// Often this is not a problem but you can fix it by welding vertices
        /// and passing in the triangle array of the welded vertices.
        private static Edge[] BuildEdges(int vertexCount, int[] triangleArray)
        {
            int maxEdgeCount = triangleArray.Length;
            int[] firstEdge = new int[vertexCount + maxEdgeCount];
            int nextEdge = vertexCount;
            int triangleCount = triangleArray.Length / 3;

            for (int a = 0; a < vertexCount; a++)
                firstEdge[a] = -1;

            // First pass over all triangles. This finds all the edges satisfying the
            // condition that the first vertex index is less than the second vertex index
            // when the direction from the first vertex to the second vertex represents
            // a counterclockwise winding around the triangle to which the edge belongs.
            // For each edge found, the edge index is stored in a linked list of edges
            // belonging to the lower-numbered vertex index i. This allows us to quickly
            // find an edge in the second pass whose higher-numbered vertex index is i.
            Edge[] edgeArray = new Edge[maxEdgeCount];

            int edgeCount = 0;
            for (int a = 0; a < triangleCount; a++)
            {
                int i1 = triangleArray[a * 3 + 2];
                for (int b = 0; b < 3; b++)
                {
                    int i2 = triangleArray[a * 3 + b];
                    if (i1 < i2)
                    {
                        Edge newEdge = new Edge
                        {
                            VertexIndex0 = i1,
                            VertexIndex1 = i2,
                            FaceIndex0 = a,
                            FaceIndex1 = a
                        };
                        edgeArray[edgeCount] = newEdge;

                        int edgeIndex = firstEdge[i1];
                        if (edgeIndex == -1)
                        {
                            firstEdge[i1] = edgeCount;
                        }
                        else
                        {
                            while (true)
                            {
                                int index = firstEdge[nextEdge + edgeIndex];
                                if (index == -1)
                                {
                                    firstEdge[nextEdge + edgeIndex] = edgeCount;
                                    break;
                                }

                                edgeIndex = index;
                            }
                        }

                        firstEdge[nextEdge + edgeCount] = -1;
                        edgeCount++;
                    }

                    i1 = i2;
                }
            }

            // Second pass over all triangles. This finds all the edges satisfying the
            // condition that the first vertex index is greater than the second vertex index
            // when the direction from the first vertex to the second vertex represents
            // a counterclockwise winding around the triangle to which the edge belongs.
            // For each of these edges, the same edge should have already been found in
            // the first pass for a different triangle. Of course we might have edges with only one triangle
            // in that case we just add the edge here
            // So we search the list of edges
            // for the higher-numbered vertex index for the matching edge and fill in the
            // second triangle index. The maximum number of comparisons in this search for
            // any vertex is the number of edges having that vertex as an endpoint.

            for (int a = 0; a < triangleCount; a++)
            {
                int i1 = triangleArray[a * 3 + 2];
                for (int b = 0; b < 3; b++)
                {
                    int i2 = triangleArray[a * 3 + b];
                    if (i1 > i2)
                    {
                        bool foundEdge = false;
                        for (int edgeIndex = firstEdge[i2]; edgeIndex != -1; edgeIndex = firstEdge[nextEdge + edgeIndex])
                        {
                            Edge edge = edgeArray[edgeIndex];
                            if ((edge.VertexIndex1 == i1) && (edge.FaceIndex0 == edge.FaceIndex1))
                            {
                                edgeArray[edgeIndex].FaceIndex1 = a;
                                foundEdge = true;
                                break;
                            }
                        }

                        if (!foundEdge)
                        {
                            var newEdge = new Edge
                            {
                                VertexIndex0 = i1,
                                VertexIndex1 = i2,
                                FaceIndex0 = a,
                                FaceIndex1 = a
                            };
                            edgeArray[edgeCount] = newEdge;
                            edgeCount++;
                        }
                    }

                    i1 = i2;
                }
            }

            Edge[] compactedEdges = new Edge[edgeCount];
            for (int e = 0; e < edgeCount; e++)
                compactedEdges[e] = edgeArray[e];

            return compactedEdges;
        }
        internal virtual void OnEnable(Water water)
        {
            _Water = water;
        }

        internal virtual void OnDisable()
        {
            Dispose();
        }
        #endregion Private Methods
    }
}