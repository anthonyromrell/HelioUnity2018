using UnityEngine.Serialization;

namespace UltimateWater.Internal
{
    using UnityEngine;

    /// <summary>
    ///     Manages water primitives.
    /// </summary>
    [System.Serializable]
    public class WaterGeometry
    {
        #region Public Types
        public enum Type
        {
            RadialGrid,
            ProjectionGrid,
            UniformGrid,
            CustomMeshes
        }
        #endregion Public Types

        #region Public Variables
        public Type GeometryType
        {
            get { return _Type; }
        }

        public int VertexCount
        {
            get { return _BaseVertexCount; }
        }

        public int TesselatedBaseVertexCount
        {
            get { return _TesselatedBaseVertexCount; }
        }

        public bool AdaptToResolution
        {
            get { return _AdaptToResolution; }
        }

        public bool Triangular
        {
            get { return _Type == Type.CustomMeshes && _CustomSurfaceMeshes.Triangular; }
        }

        public WaterCustomSurfaceMeshes CustomSurfaceMeshes
        {
            get { return _CustomSurfaceMeshes; }
        }
        #endregion Public Variables

        #region Public Methods
        public Mesh[] GetMeshes(WaterGeometryType geometryType, int vertexCount, bool volume)
        {
            if (geometryType == WaterGeometryType.ProjectionGrid)
                throw new System.InvalidOperationException("Projection grid needs camera to be retrieved. Use GetTransformedMeshes instead.");

            // water represented by custom meshes can't use any other primitives
            if (_Type == Type.CustomMeshes)
                geometryType = WaterGeometryType.Auto;

            Matrix4x4 matrix;

            switch (geometryType)
            {
                case WaterGeometryType.Auto:
                    {
                        switch (_Type)
                        {
                            case Type.RadialGrid: return _RadialGrid.GetTransformedMeshes(null, out matrix, vertexCount, volume);
                            case Type.ProjectionGrid: return _ProjectionGrid.GetTransformedMeshes(null, out matrix, vertexCount, volume);
                            case Type.UniformGrid: return _UniformGrid.GetTransformedMeshes(null, out matrix, vertexCount, volume);
                            case Type.CustomMeshes: return _CustomSurfaceMeshes.GetTransformedMeshes(null, out matrix, volume);
                            default: throw new System.InvalidOperationException("Unknown water geometry type.");
                        }
                    }

                case WaterGeometryType.RadialGrid: return _RadialGrid.GetTransformedMeshes(null, out matrix, vertexCount, volume);
                case WaterGeometryType.ProjectionGrid: return _ProjectionGrid.GetTransformedMeshes(null, out matrix, vertexCount, volume);
                case WaterGeometryType.UniformGrid: return _UniformGrid.GetTransformedMeshes(null, out matrix, vertexCount, volume);
                case WaterGeometryType.QuadGeometry: return _QuadGeometry.GetTransformedMeshes(null, out matrix, vertexCount, volume);
                default: throw new System.InvalidOperationException("Unknown water geometry type.");
            }
        }

        public Mesh[] GetTransformedMeshes(Camera camera, out Matrix4x4 matrix, WaterGeometryType geometryType, bool volume, int vertexCount = 0)
        {
            if (vertexCount == 0)
                vertexCount = ComputeVertexCountForCamera(camera);

            // water represented by custom meshes can't use any other primitives
            if (_Type == Type.CustomMeshes)
                geometryType = WaterGeometryType.Auto;

            switch (geometryType)
            {
                case WaterGeometryType.Auto:
                    {
                        switch (_Type)
                        {
                            case Type.RadialGrid: return _RadialGrid.GetTransformedMeshes(camera, out matrix, vertexCount, volume);
                            case Type.ProjectionGrid: return _ProjectionGrid.GetTransformedMeshes(camera, out matrix, vertexCount, volume);
                            case Type.UniformGrid: return _UniformGrid.GetTransformedMeshes(camera, out matrix, vertexCount, volume);
                            case Type.CustomMeshes: return _CustomSurfaceMeshes.GetTransformedMeshes(null, out matrix, volume);
                            default: throw new System.InvalidOperationException("Unknown water geometry type.");
                        }
                    }

                case WaterGeometryType.RadialGrid: return _RadialGrid.GetTransformedMeshes(camera, out matrix, vertexCount, volume);
                case WaterGeometryType.ProjectionGrid: return _ProjectionGrid.GetTransformedMeshes(camera, out matrix, vertexCount, volume);
                case WaterGeometryType.UniformGrid: return _UniformGrid.GetTransformedMeshes(camera, out matrix, vertexCount, volume);
                case WaterGeometryType.QuadGeometry: return _QuadGeometry.GetTransformedMeshes(camera, out matrix, vertexCount, volume);
                default: throw new System.InvalidOperationException("Unknown water geometry type.");
            }
        }

        public int ComputeVertexCountForCamera(Camera camera)
        {
            return _AdaptToResolution ? (int)(_ThisSystemVertexCount * ((camera.pixelWidth * camera.pixelHeight) / 2073600.0f) + 0.5f) : _ThisSystemVertexCount;
        }
        #endregion Public Methods

        #region Inspector Variables
        [Tooltip("Geometry type used for display.")]
        [SerializeField, FormerlySerializedAs("type")]
        private Type _Type = Type.RadialGrid;

        [Tooltip("Water geometry vertex count at 1920x1080.")]
        [SerializeField, FormerlySerializedAs("baseVertexCount")]
        private int _BaseVertexCount = 500000;

        [Tooltip("Water geometry vertex count at 1920x1080 on systems with tessellation support. Set it a bit lower as tessellation will place additional, better distributed vertices in shader.")]
        [SerializeField, FormerlySerializedAs("tesselatedBaseVertexCount")]
        private int _TesselatedBaseVertexCount = 16000;

        [SerializeField, FormerlySerializedAs("adaptToResolution")]
        private bool _AdaptToResolution = true;

        // sub-classes managing their primitive types

        [SerializeField, FormerlySerializedAs("radialGrid")]
        private WaterRadialGrid _RadialGrid;

        [SerializeField, FormerlySerializedAs("projectionGrid")]
        private WaterProjectionGrid _ProjectionGrid;

        [SerializeField, FormerlySerializedAs("uniformGrid")]
        private WaterUniformGrid _UniformGrid;

        [SerializeField, FormerlySerializedAs("customSurfaceMeshes")]
        private WaterCustomSurfaceMeshes _CustomSurfaceMeshes;
        #endregion Inspector Variables

        #region Private Variables
        private Water _Water;
        private Type _PreviousType;
        private int _PreviousTargetVertexCount;
        private int _ThisSystemVertexCount;
        private int _FrameCount;
        private readonly WaterQuadGeometry _QuadGeometry = new WaterQuadGeometry();
        #endregion Private Variables

        #region Private Methods
        private void UpdateVertexCount()
        {
            _ThisSystemVertexCount = SystemInfo.supportsComputeShaders ?
                Mathf.Min(_TesselatedBaseVertexCount, WaterQualitySettings.Instance.MaxTesselatedVertexCount) :
                Mathf.Min(_BaseVertexCount, WaterQualitySettings.Instance.MaxVertexCount);
        }
        internal void Awake(Water water)
        {
            _Water = water;
        }

        internal void OnEnable()
        {
            OnValidate();
            UpdateVertexCount();

            _RadialGrid.OnEnable(_Water);
            _ProjectionGrid.OnEnable(_Water);
            _UniformGrid.OnEnable(_Water);
            _CustomSurfaceMeshes.OnEnable(_Water);
        }

        internal void OnDisable()
        {
            if (_RadialGrid != null) _RadialGrid.OnDisable();
            if (_ProjectionGrid != null) _ProjectionGrid.OnDisable();
            if (_UniformGrid != null) _UniformGrid.OnDisable();
            if (_CustomSurfaceMeshes != null) _CustomSurfaceMeshes.OnDisable();
        }

        internal void OnValidate()
        {
            if (_RadialGrid == null) _RadialGrid = new WaterRadialGrid();
            if (_ProjectionGrid == null) _ProjectionGrid = new WaterProjectionGrid();
            if (_UniformGrid == null) _UniformGrid = new WaterUniformGrid();
            if (_CustomSurfaceMeshes == null) _CustomSurfaceMeshes = new WaterCustomSurfaceMeshes();

            // if geometry type changed
            if (_PreviousType != _Type)
            {
                switch (_PreviousType)
                {
                    case Type.RadialGrid: _RadialGrid.RemoveFromMaterial(_Water); break;
                    case Type.ProjectionGrid: _ProjectionGrid.RemoveFromMaterial(_Water); break;
                    case Type.UniformGrid: _UniformGrid.RemoveFromMaterial(_Water); break;
                }

                switch (_Type)
                {
                    case Type.RadialGrid: _RadialGrid.AddToMaterial(_Water); break;
                    case Type.ProjectionGrid: _ProjectionGrid.AddToMaterial(_Water); break;
                    case Type.UniformGrid: _UniformGrid.AddToMaterial(_Water); break;
                }

                _PreviousType = _Type;
            }

            UpdateVertexCount();

            if (_PreviousTargetVertexCount != _ThisSystemVertexCount)
            {
                _RadialGrid.Dispose();
                _UniformGrid.Dispose();
                _ProjectionGrid.Dispose();
                _PreviousTargetVertexCount = _ThisSystemVertexCount;
            }
        }

        internal void Update()
        {
            // clean up unused geometries
            if (++_FrameCount > 8)
                _FrameCount = 0;

            switch (_FrameCount)
            {
                case 0:
                    {
                        _RadialGrid.Update();
                        break;
                    }

                case 3:
                    {
                        _ProjectionGrid.Update();
                        break;
                    }

                case 6:
                    {
                        _UniformGrid.Update();
                        break;
                    }
            }
        }
        #endregion Private Methods
    }
}