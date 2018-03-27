namespace UltimateWater
{
    using System.Diagnostics;
    using UnityEngine;
    using Internal;

    public sealed class WaveParticlesQuadtree : Quadtree<WaveParticle>
    {
        #region Public Variables
        public bool DebugMode
        {
            get { return _DebugMode; }
            set { _DebugMode = value; }
        }
        #endregion Public Variables

        #region Public methods
        public WaveParticlesQuadtree(Rect rect, int maxElementsPerNode, int maxTotalElements) : base(rect, maxElementsPerNode, maxTotalElements)
        {
            _Qroot = this;
            _ParticleGroups = new WaveParticlesGroup[maxElementsPerNode >> 3];
            CreateMesh();
        }
        public void UpdateSimulation(float time)
        {
            if (_Qa != null)
            {
                // update sub-nodes
                _Qa.UpdateSimulation(time);
                _Qb.UpdateSimulation(time);
                _Qc.UpdateSimulation(time);
                _Qd.UpdateSimulation(time);
            }
            else if (_NumElements != 0)
            {
                // update local particles
                UpdateParticles(time);
            }
        }
        public void UpdateSimulation(float time, float maxExecutionTimeExp)
        {
            if (_Stopwatch == null)
                _Stopwatch = new Stopwatch();

            _Stopwatch.Reset();
            _Stopwatch.Start();

            UpdateSimulation(time);

            float duration = _Stopwatch.ElapsedTicks / 10000.0f;

            if (duration > 50.0f)
                duration = 50.0f;

            _Stress = _Stress * 0.98f + (Mathf.Exp(duration) - maxExecutionTimeExp) * 0.04f;            // execution time of more than 0.5 milisecond adds to stress

            if (_Stress < 1.0f) _Stress = 1.0f;
            if (!(_Stress < 20.0f)) _Stress = 20.0f;          // handles NaNs
        }
        public void Render(Rect renderRect)
        {
            if (!Rect.Overlaps(renderRect))
                return;

            if (_Qa != null)
            {
                _Qa.Render(renderRect);
                _Qb.Render(renderRect);
                _Qc.Render(renderRect);
                _Qd.Render(renderRect);
            }
            else if (_NumElements != 0)
            {
                _Mesh.vertices = _Vertices;

                if (_TangentsPackChanged)
                {
                    _Mesh.tangents = _TangentsPack;
                    _TangentsPackChanged = false;
                }

                if (_Qroot._DebugMode)
                    _Mesh.normals = _DebugData;

                Graphics.DrawMeshNow(_Mesh, Matrix4x4.identity, 0);
            }
        }
        public override void Destroy()
        {
            base.Destroy();

            if (_Mesh != null)
            {
                _Mesh.Destroy();
                _Mesh = null;
            }

            _Vertices = null;
            _TangentsPack = null;
        }
        #endregion Public methods

        #region Private Variables
        private Mesh _Mesh;
        private Vector3[] _Vertices;
        private Vector4[] _TangentsPack;
        private Vector3[] _DebugData;                // passed as normals
        private WaveParticlesGroup[] _ParticleGroups;
        private int _NumParticleGroups;
        private int _LastGroupIndex = -1;
        private float _Stress = 1.0f;
        private bool _TangentsPackChanged;
        private Stopwatch _Stopwatch;

        private int _LastUpdateIndex;
        private bool _DebugMode;

        private WaveParticlesQuadtree _Qa, _Qb, _Qc, _Qd;
        private readonly WaveParticlesQuadtree _Qroot;
        #endregion Private Variables

        #region Private Methods
        private WaveParticlesQuadtree(WaveParticlesQuadtree root, Rect rect, int maxElementsPerNode) : this(rect, maxElementsPerNode, 0)
        {
            _Qroot = root;
        }
        private void UpdateParticles(float time)
        {
            var enabledWaterCameras = WaterCamera.EnabledWaterCameras;
            int numEnabledWaterCameras = enabledWaterCameras.Count;
            bool isVisible = false;

            for (int i = 0; i < numEnabledWaterCameras; ++i)
            {
                if (Rect.Overlaps(enabledWaterCameras[i].LocalMapsRect))
                {
                    isVisible = true;
                    break;
                }
            }

            int startIndex, endIndex, vertexIndex;

            if (!isVisible)
            {
                startIndex = _LastUpdateIndex;
                endIndex = _LastUpdateIndex + 8;
                vertexIndex = startIndex << 2;

                if (endIndex >= _Elements.Length)
                {
                    endIndex = _Elements.Length;
                    _LastUpdateIndex = 0;
                }
                else
                    _LastUpdateIndex = endIndex;
            }
            else
            {
                startIndex = 0;
                endIndex = _Elements.Length;
                vertexIndex = 0;
            }

            WaveParticlesQuadtree rootQuadtree = isVisible ? _Qroot : null;
            float updateDelay = isVisible ? 0.01f : 1.5f;
            float costlyUpdateDelay = isVisible ? 0.4f : 8.0f;
            bool didCostlyUpdate = false;

            updateDelay *= _Qroot._Stress;
            costlyUpdateDelay *= _Qroot._Stress;

            for (int i = 0; _ParticleGroups != null && i < _ParticleGroups.Length; ++i)
            {
                var group = _ParticleGroups[i];

                if (group != null)
                {
                    if (group.LeftParticle == null || !group.LeftParticle.IsAlive)
                    {
                        --_NumParticleGroups;
                        _ParticleGroups[i] = null;
                        continue;
                    }

                    if (time >= group.LastUpdateTime + updateDelay)
                    {
                        if (time >= group.LastCostlyUpdateTime + costlyUpdateDelay && !didCostlyUpdate)
                        {
                            if (!RectContainsParticleGroup(group))
                            {
                                --_NumParticleGroups;
                                _ParticleGroups[i] = null;
                                continue;
                            }

                            group.CostlyUpdate(rootQuadtree, time);
                            didCostlyUpdate = true;

                            if (group.LeftParticle == null || !group.LeftParticle.IsAlive)
                            {
                                --_NumParticleGroups;
                                _ParticleGroups[i] = null;
                                continue;
                            }
                        }

                        group.Update(time);
                    }
                }
            }

            if (_Elements != null)
            {
                for (int i = startIndex; i < endIndex; ++i)
                {
                    var particle = _Elements[i];

                    if (particle != null)
                    {
                        if (particle.IsAlive)
                        {
                            if (_MarginRect.Contains(particle.Position))
                            {
                                var vertexData = particle.VertexData;
                                var particleData = particle.PackedParticleData;
                                _Vertices[vertexIndex] = vertexData;
                                _TangentsPack[vertexIndex++] = particleData;
                                _Vertices[vertexIndex] = vertexData;
                                _TangentsPack[vertexIndex++] = particleData;
                                _Vertices[vertexIndex] = vertexData;
                                _TangentsPack[vertexIndex++] = particleData;
                                _Vertices[vertexIndex] = vertexData;
                                _TangentsPack[vertexIndex++] = particleData;
                                _TangentsPackChanged = true;

#if UNITY_EDITOR
                                if (_Qroot._DebugMode)
                                {
                                    if (_DebugData == null)
                                        _DebugData = new Vector3[_Vertices.Length];

                                    var particleDebugData = particle.DebugData;
                                    _DebugData[vertexIndex - 4] = particleDebugData;
                                    _DebugData[vertexIndex - 3] = particleDebugData;
                                    _DebugData[vertexIndex - 2] = particleDebugData;
                                    _DebugData[vertexIndex - 1] = particleDebugData;
                                }
#endif
                            }
                            else
                            {
                                // re-add particle
                                base.RemoveElementAt(i);

                                _Vertices[vertexIndex++].x = float.NaN;
                                _Vertices[vertexIndex++].x = float.NaN;
                                _Vertices[vertexIndex++].x = float.NaN;
                                _Vertices[vertexIndex++].x = float.NaN;
                                _Qroot.AddElement(particle);
                            }
                        }
                        else
                        {
                            // remove particle
                            base.RemoveElementAt(i);

                            _Vertices[vertexIndex++].x = float.NaN;
                            _Vertices[vertexIndex++].x = float.NaN;
                            _Vertices[vertexIndex++].x = float.NaN;
                            _Vertices[vertexIndex++].x = float.NaN;
                            particle.AddToCache();
                        }
                    }
                    else
                        vertexIndex += 4;
                }
            }
        }
        private bool HasParticleGroup(WaveParticlesGroup group)
        {
            for (int i = 0; i < _ParticleGroups.Length; ++i)
            {
                if (_ParticleGroups[i] == group)
                    return true;
            }

            return false;
        }
        private void AddParticleGroup(WaveParticlesGroup group)
        {
            if (_ParticleGroups.Length == _NumParticleGroups)
                System.Array.Resize(ref _ParticleGroups, _NumParticleGroups << 1);

            for (++_LastGroupIndex; _LastGroupIndex < _ParticleGroups.Length; ++_LastGroupIndex)
            {
                if (_ParticleGroups[_LastGroupIndex] == null)
                {
                    ++_NumParticleGroups;
                    _ParticleGroups[_LastGroupIndex] = group;
                    return;
                }
            }

            for (_LastGroupIndex = 0; _LastGroupIndex < _ParticleGroups.Length; ++_LastGroupIndex)
            {
                if (_ParticleGroups[_LastGroupIndex] == null)
                {
                    ++_NumParticleGroups;
                    _ParticleGroups[_LastGroupIndex] = group;
                    return;
                }
            }
        }
        private bool RectContainsParticleGroup(WaveParticlesGroup group)
        {
            var particle = group.LeftParticle;

            if (!particle.IsAlive)
                return false;

            do
            {
                if (_MarginRect.Contains(particle.Position))
                    return true;

                particle = particle.RightNeighbour;
            }
            while (particle != null);

            return false;
        }
        protected override void AddElementAt(WaveParticle particle, int index)
        {
            base.AddElementAt(particle, index);

            if (!HasParticleGroup(particle.Group))
                AddParticleGroup(particle.Group);
        }
        protected override void RemoveElementAt(int index)
        {
            base.RemoveElementAt(index);

            int vertexIndex = index << 2;
            _Vertices[vertexIndex++].x = float.NaN;
            _Vertices[vertexIndex++].x = float.NaN;
            _Vertices[vertexIndex++].x = float.NaN;
            _Vertices[vertexIndex].x = float.NaN;
        }
        protected override void SpawnChildNodes()
        {
            _Mesh.Destroy();
            _Mesh = null;

            float halfWidth = Rect.width * 0.5f;
            float halfHeight = Rect.height * 0.5f;
            _A = _Qa = new WaveParticlesQuadtree(_Qroot, new Rect(Rect.xMin, _Center.y, halfWidth, halfHeight), _Elements.Length);
            _B = _Qb = new WaveParticlesQuadtree(_Qroot, new Rect(_Center.x, _Center.y, halfWidth, halfHeight), _Elements.Length);
            _C = _Qc = new WaveParticlesQuadtree(_Qroot, new Rect(Rect.xMin, Rect.yMin, halfWidth, halfHeight), _Elements.Length);
            _D = _Qd = new WaveParticlesQuadtree(_Qroot, new Rect(_Center.x, Rect.yMin, halfWidth, halfHeight), _Elements.Length);

            _Vertices = null;
            _TangentsPack = null;
            _ParticleGroups = null;
            _NumParticleGroups = 0;
        }
        private void CreateMesh()
        {
            int numVertices = _Elements.Length << 2;
            _Vertices = new Vector3[numVertices];

            for (int i = 0; i < _Vertices.Length; ++i)
                _Vertices[i].x = float.NaN;

            _TangentsPack = new Vector4[numVertices];

            var uvs = new Vector2[numVertices];

            for (int i = 0; i < uvs.Length;)
            {
                uvs[i++] = new Vector2(0.0f, 0.0f);
                uvs[i++] = new Vector2(0.0f, 1.0f);
                uvs[i++] = new Vector2(1.0f, 1.0f);
                uvs[i++] = new Vector2(1.0f, 0.0f);
            }

            var indices = new int[numVertices];

            for (int i = 0; i < indices.Length; ++i)
                indices[i] = i;

            _Mesh = new Mesh
            {
                hideFlags = HideFlags.DontSave,
                name = "Wave Particles",
                vertices = _Vertices,
                uv = uvs,
                tangents = _TangentsPack
            };
            _Mesh.SetIndices(indices, MeshTopology.Quads, 0);
        }
        #endregion Private Methods
    }
}