namespace UltimateWater.Internal
{
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public abstract class WaterPrimitiveBase
    {
        #region Public Methods
        public virtual Mesh[] GetTransformedMeshes(Camera camera, out Matrix4x4 matrix, int vertexCount, bool volume)
        {
            matrix = (camera != null) ? GetMatrix(camera) : Matrix4x4.identity;

            CachedMeshSet cachedMeshSet;
            int hash = vertexCount;

            if (volume) hash = -hash;

            if (!_Cache.TryGetValue(hash, out cachedMeshSet))
                _Cache[hash] = cachedMeshSet = new CachedMeshSet(CreateMeshes(vertexCount, volume));
            else
                cachedMeshSet.Update();

            return cachedMeshSet.Meshes;
        }
        public void Dispose()
        {
            foreach (var cachedMeshSet in _Cache.Values)
            {
                foreach (var mesh in cachedMeshSet.Meshes)
                {
                    if (Application.isPlaying)
                        Object.Destroy(mesh);
                    else
                        Object.DestroyImmediate(mesh);
                }
            }

            _Cache.Clear();
        }
        #endregion Public Methods

        #region Private Types
        protected class CachedMeshSet
        {
            #region Public Variables
            public Mesh[] Meshes;
            public int LastFrameUsed;
            #endregion Public Variables

            #region Public Methods
            public CachedMeshSet(Mesh[] meshes)
            {
                Meshes = meshes;

                Update();
            }

            public void Update()
            {
                LastFrameUsed = Time.frameCount;
            }
            #endregion Public Methods
        }
        #endregion Private Types

        #region Private Variables
        protected Water _Water;
        protected Dictionary<int, CachedMeshSet> _Cache = new Dictionary<int, CachedMeshSet>(Int32EqualityComparer.Default);
        private List<int> _KeysToRemove;
        #endregion Private Variables

        #region Private Methods
        protected abstract Matrix4x4 GetMatrix(Camera camera);
        protected abstract Mesh[] CreateMeshes(int vertexCount, bool volume);
        protected Mesh CreateMesh(Vector3[] vertices, int[] indices, string name, bool triangular = false)
        {
            var mesh = new Mesh
            {
                hideFlags = HideFlags.DontSave,
                name = name,
                vertices = vertices
            };
            mesh.SetIndices(indices, triangular ? MeshTopology.Triangles : MeshTopology.Quads, 0);
            mesh.RecalculateBounds();
            mesh.UploadMeshData(true);

            return mesh;
        }
        internal void Update()
        {
            int currentFrame = Time.frameCount;

            if (_KeysToRemove == null)
                _KeysToRemove = new List<int>();

            var enumerator = _Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var kv = enumerator.Current;

                if (currentFrame - kv.Value.LastFrameUsed > 27)         // waterprimitivebase updates run every 9 frame
                {
                    _KeysToRemove.Add(kv.Key);

                    foreach (var mesh in kv.Value.Meshes)
                    {
                        if (Application.isPlaying)
                            Object.Destroy(mesh);
                        else
                            Object.DestroyImmediate(mesh);
                    }
                }
            }
            enumerator.Dispose();

            for (int i = 0; i < _KeysToRemove.Count; ++i)
                _Cache.Remove(_KeysToRemove[i]);

            _KeysToRemove.Clear();
        }
        internal virtual void OnEnable(Water water)
        {
            _Water = water;
        }

        internal virtual void OnDisable()
        {
            Dispose();
        }

        internal virtual void AddToMaterial(Water water)
        {
        }

        internal virtual void RemoveFromMaterial(Water water)
        {
        }
        #endregion Private Methods
    }
}