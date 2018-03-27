using UltimateWater.Utils;

namespace UltimateWater
{
#if UNITY_EDITOR

    using UnityEditor;

#endif

    using UnityEngine;
    using System.Collections.Generic;
    using UnityEngine.Rendering;
    using Internal;

    [RequireComponent(typeof(Camera))]
    public class WaterRaindropsIME : MonoBehaviour
    {
        #region Public Types
        [System.Serializable]
        public class FadeModule
        {
            [Tooltip("1 - no fade, 0 - instant fade")]
            [Range(0.5f, 1.0f)]
            public float Intensity = 0.99f;

            [Tooltip("Additional texture based fade")]
            public Texture2D Texture;
            [Range(0.0f, 1.0f)]
            public float Multiplier = 0.1f;
        }

        [System.Serializable]
        public class DistortionModule
        {
            public float Multiplier = 1.0f;
            [Range(0.001f, 0.008f)]
            public float NormalSpread = 0.002f;
        }

        [System.Serializable]
        public class TwirlModule
        {
            public Texture2D Texture;
            [Range(0.0f, 1.0f)]
            public float Multiplier = 0.1f;
        }

        [System.Serializable]
        public class TrackingModule
        {
            #region Inspector Variables
            [Header("Force multipliers")]
            [SerializeField]
            private float _Translation;
            [SerializeField]
            private float _Rotation;
            #endregion Inspector Variables

            #region Public Variables
            public Vector3 Force { get { return -_Velocity; } }
            #endregion Public Variables

            #region Private Variables
            private WaterRaindropsIME _Reference;
            private Vector3 _PreviousPosition;
            private Vector3 _Velocity;
            #endregion Private Variables

            #region Public Methods
            internal void Initialize(WaterRaindropsIME reference)
            {
                _Reference = reference;
                _PreviousPosition = _Reference.transform.forward * _Rotation + _Reference.transform.position * _Translation;
            }
            internal void Advance()
            {
                var position = _Reference.transform.forward * _Rotation + _Reference.transform.position * _Translation;

                _Velocity = _Reference.transform.worldToLocalMatrix * ((position - _PreviousPosition) / Time.fixedDeltaTime);
                _PreviousPosition = position;
            }
            #endregion Public Methods
        }
        #endregion Public Types

        #region Inspector Variables
        [SerializeField]
        [HideInInspector]
        private Cubemap _CubeMap;

        [Header("Settings")]
        public Vector3 Force = Vector3.down;
        public float VolumeLoss = 0.02f;

        [Range(0.1f, 1.0f)]
        public float Resolution = 0.5f;

        [Header("Friction")]
        [Range(0.0f, 1.0f)]
        [Tooltip("Air resistance causing raindrops to slow down")]
        public float AirFriction = 0.5f;

        [Range(0.0f, 10.0f)] public float WindowFrictionMultiplier = 0.5f;

        [Tooltip("Adds forces caused by lens imperfections")]
        public Texture2D WindowFriction;

        [Tooltip("How much the water bends light")]
        public DistortionModule Distortion;
        [Tooltip("Distorts water paths using custom texture")]
        public TwirlModule Twirl;
        [Tooltip("How much time is needed for raindrops to disappear")]
        public FadeModule Fade;
        [Tooltip("How much force is applied to raindrops from camera movement")]
        public TrackingModule Tracking;
        #endregion Inspector Variables

        #region Public Variables
        public Vector3 WorldForce
        {
            get { return transform.worldToLocalMatrix * Force; }
        }
        #endregion Public Variables

        #region Public Methods
        public void Spawn(Vector3 velocity, float volume, float life, float x, float y)
        {
            var position = new Vector2(x, y);

            Droplet droplet;
            droplet.Position = position;

            droplet.Volume = volume;
            droplet.Velocity = transform.worldToLocalMatrix * -velocity;
            droplet.Velocity.x = -droplet.Velocity.x;
            droplet.Life = life;

            _Droplets.Add(droplet);
        }
        #endregion Public Methods

        #region Unity Messages
        private void Awake()
        {
            CreateMaterials();

            CreateSimulation();
            SetMaterialProperties();
        }

        private void Start()
        {
            Tracking.Initialize(this);
        }

        private void OnPreCull()
        {
            Render();
        }
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            FadeMaps();

            _FinalMaterial.SetTexture("unity_Spec", _CubeMap);

            if (source.texelSize.y < 0.0f)
            {
                if (_InvertMaterial == null) _InvertMaterial = new Material(Shader.Find("Hidden/InvertY"));

                var temp = source.CreateTemporary();
                Graphics.Blit(source, temp);
                Graphics.Blit(temp, source);

                Graphics.Blit(source, temp, _FinalMaterial);
                Graphics.Blit(temp, destination, _InvertMaterial);

                temp.ReleaseTemporary();
            }
            else
            {
                Graphics.Blit(source, destination, _FinalMaterial);
            }
        }

        private void Update()
        {
            Tracking.Advance();

            Advance();
        }
        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (_Initialized)
            {
                SetMaterialProperties();
            }
        }

        private void OnDestroy()
        {
            TextureUtility.Release(ref _Target);
        }
        #endregion Unity Messages

        #region Private Types
        private struct Droplet
        {
            public Vector2 Position;
            public Vector2 Velocity;
            public float Volume;
            public float Life;
        }
        #endregion Private Types

        #region Private Variables
        private RenderTexture _Target;
        private Vector2[,] _Friction;

        private CommandBuffer _Buffer;
        private Mesh _Mesh;
        private Matrix4x4[] _Matrices;

        private readonly List<Droplet> _Droplets = new List<Droplet>();

        private bool _Initialized;

        private Material _FadeMaterial;
        private Material _DropletMaterial;
        private Material _FinalMaterial;

        private Material _InvertMaterial;
        #endregion Private Variables

        #region Private Methods
        private static bool IsVisible(Droplet droplet)
        {
            bool result = true;
            result &= (droplet.Position.x >= 0.0f) && (droplet.Position.x <= 1.0f);
            result &= (droplet.Position.y >= 0.0f) && (droplet.Position.y <= 1.0f);
            return result;
        }

        private void OnDropletUpdate(ref Droplet droplet)
        {
            var position = droplet.Position;
            var velocity = droplet.Velocity;

            droplet.Life -= Time.fixedDeltaTime;
            if (droplet.Volume < 0.2f)
            {
                return;
            }

            var localForce = transform.worldToLocalMatrix * Force;
            var localForce2 = new Vector2(localForce.x, -localForce.y);

            var localTracking2 = new Vector2(Tracking.Force.x, Tracking.Force.y);

            // calculate friction, if the window friction texture was selected
            Vector2 friction = Vector2.zero;
            if (_Friction != null)
            {
                friction = _Friction[(int)(position.x * (_Target.width - 1)),
                    (int)(position.y * (_Target.height - 1))];
            }

            // calculate forces and velocity
            float velocityMagnitude = velocity.magnitude;
            var velocityDirection = velocity.normalized;

            // add one to prevent lowering force with sqrt on a value < 1.0
            var airFrictionForce = -velocityDirection * ((velocityMagnitude + 1.0f) * (velocityMagnitude + 1.0f)) * AirFriction;
            var staticFrictionForce = -friction * WindowFrictionMultiplier;

            var force = localForce2 + localTracking2 + airFrictionForce + staticFrictionForce;

            var acceleration = force / droplet.Volume;

            // numerical integration (so easy, much wow ©2003MEMES)
            droplet.Velocity += acceleration * Time.deltaTime;
            droplet.Position += droplet.Velocity * Time.deltaTime;

            // traveled distance
            float distance = Vector3.Distance(droplet.Position, position);
            float volumeLoss = distance * VolumeLoss + 0.001f;

            // modify volume
            droplet.Volume -= volumeLoss;
        }

        private void Draw(int index, Vector3 position, float size, Vector2 velocity)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

            _Matrices[index] = Matrix4x4.TRS(
                (position * 2.0f) - new Vector3(1.0f, 1.0f, 0.0f),
                Quaternion.AngleAxis(angle, Vector3.forward),
                new Vector3(1.0f + velocity.magnitude * 10.0f, 1.0f, 1.0f) * size * 0.1f
            );
        }

        private void Advance()
        {
            _Droplets.RemoveAll(x => !IsVisible(x) || x.Life <= 0.0f || x.Volume <= 0.0f);

            for (int i = 0; i < _Droplets.Count; ++i)
            {
                var droplet = _Droplets[i];

                OnDropletUpdate(ref droplet);

                if (!IsVisible(droplet))
                {
                    _Droplets[i] = droplet;
                    continue;
                }

                Draw(i, droplet.Position, droplet.Volume, droplet.Velocity);
                _Droplets[i] = droplet;
            }
        }

        private void FadeMaps()
        {
            var temp = RenderTexture.GetTemporary(_Target.width, _Target.height, _Target.depth, _Target.format);
            temp.filterMode = _Target.filterMode;

            _FadeMaterial.SetVector("_Modulation_STX", Vector4.one * 10.0f);
            Graphics.Blit(_Target, temp, _FadeMaterial);
            Graphics.CopyTexture(temp, _Target);

            RenderTexture.ReleaseTemporary(temp);
        }

        private void Render()
        {
            _Buffer.Clear();
            _Buffer.SetRenderTarget(_Target);

            // [todo] : add instancing support in shaders
            //#if UNITY_5_5_OR_NEWER
            //            _Buffer.DrawMeshInstanced(_Mesh, 0, _DropletMaterial, 0, _Matrices, _Droplets.Count);
            //#else
            for (int i = 0; i < _Droplets.Count; ++i)
            {
                _Buffer.DrawMesh(_Mesh, _Matrices[i], _DropletMaterial);
            }
            //#endif
            Graphics.ExecuteCommandBuffer(_Buffer);
        }

        private void CreateSimulation()
        {
            _Initialized = true;
            CreateRenderTexture();

            _Matrices = new Matrix4x4[4096];
            _Buffer = new CommandBuffer();
            _Mesh = BuildQuad(1.0f, 1.0f);

            CreateFrictionMap();
        }

        private void CreateFrictionMap()
        {
            if (_Target == null || WindowFriction == null)
            {
                return;
            }

#if UNITY_EDITOR
            if (MakeTexture2DReadable(WindowFriction))
            {
                Debug.Log("Made WindowFriction texture readable (" + WindowFriction.name + ")");
            }
#endif

            _Friction = Sample(WindowFriction, _Target.width, _Target.height);
        }

        private void CreateRenderTexture()
        {
            var desc = new TextureUtility.RenderTextureDesc("[UWS] WaterRaindropsIME - Raindrops")
            {
                Height = (int)(Screen.height * Resolution),
                Width = (int)(Screen.width * Resolution),
                Format = RenderTextureFormat.RFloat,
                Filter = FilterMode.Bilinear,
            };

            _Target = desc.CreateRenderTexture();
            _Target.Clear();
        }

        private void CreateMaterials()
        {
            var shaders = ShaderUtility.Instance;
            _FinalMaterial = shaders.CreateMaterial(ShaderList.RaindropsFinal);
            _FadeMaterial = shaders.CreateMaterial(ShaderList.RaindropsFade);
            _DropletMaterial = shaders.CreateMaterial(ShaderList.RaindropsParticle);
        }

        private void SetMaterialProperties()
        {
            _FinalMaterial.SetTexture("_WaterDropsTex", _Target);

            // distortion
            _FinalMaterial.SetFloat("_NormalSpread", Distortion.NormalSpread);
            _FinalMaterial.SetFloat("_Distortion", Distortion.Multiplier);

            // twirl
            if (Twirl.Texture != null)
            {
                _FinalMaterial.SetTexture("_Twirl", Twirl.Texture);
                _FinalMaterial.SetFloat("_TwirlMultiplier", Twirl.Multiplier);
            }

            _FadeMaterial.SetFloat("_Value", Fade.Intensity);
            _FadeMaterial.SetFloat("_ModulationStrength", Fade.Multiplier);
            if (Fade.Texture != null)
            {
                _FadeMaterial.SetTexture("_Modulation", Fade.Texture);
            }
        }
        #endregion Private Methods

        #region Helpers
#if UNITY_EDITOR
        private static bool MakeTexture2DReadable(Texture2D texture)
        {
            string path = AssetDatabase.GetAssetPath(texture);
            var importer = (TextureImporter)AssetImporter.GetAtPath(path);
            if (importer.isReadable)
            {
                return false;
            }

            importer.isReadable = true;
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            return true;
        }

        #region Validation
        [InspectorWarning("Validation", InspectorWarningAttribute.InfoType.Note)]
        [SerializeField]
        private string _Validation;

        // ReSharper disable once UnusedMember.Local
        private string Validation()
        {
            string info = string.Empty;
            if (WindowFriction == null) { info += "note: select WindowFriction texture"; }

            return info;
        }
        #endregion Validation
#endif

        private static Mesh BuildQuad(float width, float height)
        {
            Mesh mesh = new Mesh();

            // Setup vertices
            Vector3[] newVertices = new Vector3[4];
            float halfHeight = height * 0.5f;
            float halfWidth = width * 0.5f;
            newVertices[0] = new Vector3(-halfWidth, -halfHeight, 0);
            newVertices[1] = new Vector3(-halfWidth, halfHeight, 0);
            newVertices[2] = new Vector3(halfWidth, -halfHeight, 0);
            newVertices[3] = new Vector3(halfWidth, halfHeight, 0);

            // Setup UVs
            Vector2[] newUVs = new Vector2[newVertices.Length];
            newUVs[0] = new Vector2(0, 0);
            newUVs[1] = new Vector2(0, 1);
            newUVs[2] = new Vector2(1, 0);
            newUVs[3] = new Vector2(1, 1);

            // Setup triangles
            int[] newTriangles = new int[] { 0, 1, 2, 3, 2, 1 };

            // Setup normals
            Vector3[] newNormals = new Vector3[newVertices.Length];
            for (int i = 0; i < newNormals.Length; i++)
            {
                newNormals[i] = Vector3.forward;
            }

            // Create quad
            mesh.vertices = newVertices;
            mesh.uv = newUVs;
            mesh.triangles = newTriangles;
            mesh.normals = newNormals;

            return mesh;
        }

        private Vector2[,] Sample(Texture texture, int width, int height, int step = 4)
        {
            var result = new Vector2[width, height];
            var pixels = WindowFriction.GetPixels();

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    // normalize to get UV's
                    float ux = x / (float)width;
                    float uy = y / (float)height;

                    // calculate pixel to sample
                    int px = (int)(ux * texture.width);
                    int py = (int)(uy * texture.height);

                    // smooth
                    int left = py * width + px - step;
                    int right = py * width + px + step;
                    int top = (py + step) * width + px;
                    int bottom = (py - step) * width + px;

                    float horizontal = 0.0f;
                    float vertical = 0.0f;

                    if (IsValidTextureIndex(left, width, height) && IsValidTextureIndex(right, width, height))
                    {
                        horizontal = pixels[right].r - pixels[left].r;
                    }
                    if (IsValidTextureIndex(top, width, height) && IsValidTextureIndex(bottom, width, height))
                    {
                        vertical = pixels[top].r - pixels[bottom].r;
                    }

                    result[x, y] = new Vector2(horizontal, vertical);
                }
            }

            return result;
        }

        private static bool IsValidTextureIndex(int index, int width, int height)
        {
            int count = width * height;
            return index >= 0 && index < count;
        }
        #endregion Helpers
    }
}