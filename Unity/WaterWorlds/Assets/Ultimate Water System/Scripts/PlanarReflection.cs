namespace UltimateWater
{
    using System.Collections.Generic;
    using UnityEngine;
    using Internal;

    public sealed class PlanarReflection : WaterModule
    {
        #region Public Types
        [System.Serializable]
        public class Data
        {
            public LayerMask ReflectionMask = int.MaxValue;
            public bool ReflectSkybox = true;
            public bool RenderShadows = true;

            [Range(0.0f, 1.0f)]
            public float Resolution = 0.5f;

            [Range(0.0f, 1.0f)]
            [Tooltip("Allows you to use more rational resolution of planar reflections on screens with very high dpi. Planar reflections should be blurred anyway.")]
            public float RetinaResolution = 0.333f;
        }
        #endregion Public Types

        #region Public Variables
        public float Resolution
        {
            get { return _Data.Resolution; }
            set
            {
                _Data.Resolution = value;
                CalculateResolutionMultiplier();
            }
        }
        public float RetinaResolution
        {
            get { return _Data.RetinaResolution; }
            set
            {
                _Data.RetinaResolution = value;
                CalculateResolutionMultiplier();
            }
        }

        public bool ReflectSkybox
        {
            get { return _Data.ReflectSkybox; }
            set { _Data.ReflectSkybox = value; }
        }
        public bool RenderShadows
        {
            get { return _Data.RenderShadows; }
            set { _Data.RenderShadows = value; }
        }

        public LayerMask ReflectionMask
        {
            get { return _Data.ReflectionMask; }
            set { _Data.ReflectionMask = value; }
        }
        #endregion Public Variables

        #region Public Methods
        public PlanarReflection(Water water, Data data)
        {
            _Water = water;
            _Data = data;

            _SystemSupportsHdr = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);

            Validate();

            water.ProfilesManager.Changed.AddListener(OnProfilesChanged);
            OnProfilesChanged(water);
        }
        internal override void OnWaterPostRender(WaterCamera waterCamera)
        {
            var camera = waterCamera.CameraComponent;

            TemporaryRenderTexture renderTexture;

            if (_TemporaryTargets.TryGetValue(camera, out renderTexture))
            {
                _TemporaryTargets.Remove(camera);
                renderTexture.Dispose();
            }
        }
        #endregion Public Methods

        #region Private Variables
        private readonly Data _Data;
        private readonly Water _Water;
        private readonly bool _SystemSupportsHdr;
        private readonly Dictionary<Camera, TemporaryRenderTexture> _TemporaryTargets =
            new Dictionary<Camera, TemporaryRenderTexture>();

        private TemporaryRenderTexture _CurrentTarget;
        private float _FinalResolutionMultiplier;
        private bool _RenderPlanarReflections;
        private Material _UtilitiesMaterial;
        private Shader _UtilitiesShader;

        private const float _ClipPlaneOffset = 0.07f;
        #endregion Private Variables

        #region Private Methods

        internal override void Start(Water water)
        {
        }
        internal override void Enable()
        {
        }
        internal override void Disable()
        {
        }

        internal override void Validate()
        {
            if (_UtilitiesShader == null)
                _UtilitiesShader = Shader.Find("UltimateWater/Utilities/PlanarReflection - Utilities");

            _Data.Resolution = Mathf.Clamp01(Mathf.RoundToInt(_Data.Resolution * 10.0f) * 0.1f);
            _Data.RetinaResolution = Mathf.Clamp01(Mathf.RoundToInt(_Data.RetinaResolution * 10.0f) * 0.1f);

            CalculateResolutionMultiplier();
        }

        internal override void Destroy()
        {
            ClearRenderTextures();
        }

        internal override void Update()
        {
            ClearRenderTextures();
        }

        internal override void OnWaterRender(WaterCamera waterCamera)
        {
            var camera = waterCamera.CameraComponent;

            if (!camera.enabled || !_RenderPlanarReflections)
                return;

            if (!_TemporaryTargets.TryGetValue(camera, out _CurrentTarget))
            {
                var reflectionCamera = Reflection.GetReflectionCamera(camera);

                RenderReflection(camera, reflectionCamera);
                UpdateRenderProperties(reflectionCamera);
            }
        }

        private void CalculateResolutionMultiplier()
        {
            var final = Screen.dpi <= 220 ? _Data.Resolution : _Data.RetinaResolution;

            if (_FinalResolutionMultiplier != final)
            {
                _FinalResolutionMultiplier = final;
                ClearRenderTextures();
            }
        }

        private void RenderReflection(Camera camera, Camera reflectionCamera)
        {
            reflectionCamera.cullingMask = _Data.ReflectionMask;

            SetCameraSettings(camera, reflectionCamera);

            _CurrentTarget = GetRenderTexture(camera.pixelWidth, camera.pixelHeight, reflectionCamera);
            _TemporaryTargets[camera] = _CurrentTarget;

            var target = RenderTexturesCache.GetTemporary(_CurrentTarget.Texture.width, _CurrentTarget.Texture.height, 16, _CurrentTarget.Texture.format, true, false);
            reflectionCamera.targetTexture = target;

            reflectionCamera.transform.eulerAngles = CalculateReflectionAngles(camera);
            reflectionCamera.transform.position = CalculateReflectionPosition(camera);

            float d = -_Water.transform.position.y - _ClipPlaneOffset;
            Vector4 reflectionPlane = new Vector4(0, 1, 0, d);

            Matrix4x4 reflection = Matrix4x4.zero;
            reflection = Reflection.CalculateReflectionMatrix(reflection, reflectionPlane);
            Vector3 newpos = reflection.MultiplyPoint(camera.transform.position);

            reflectionCamera.worldToCameraMatrix = camera.worldToCameraMatrix * reflection;

            Vector4 clipPlane = Reflection.CameraSpacePlane(reflectionCamera, _Water.transform.position, new Vector3(0, 1, 0), _ClipPlaneOffset, 1.0f);

            var matrix = camera.projectionMatrix;
            matrix = Reflection.CalculateObliqueMatrix(matrix, clipPlane);
            reflectionCamera.projectionMatrix = matrix;

            reflectionCamera.transform.position = newpos;
            Vector3 angles = camera.transform.eulerAngles;
            reflectionCamera.transform.eulerAngles = new Vector3(-angles.x, angles.y, angles.z);

#if SKYBOX_CLEAR_BUG_FIX
            Graphics.SetRenderTarget(target);
            GL.Clear(true, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
#endif

            reflectionCamera.clearFlags = _Data.ReflectSkybox ? CameraClearFlags.Skybox : CameraClearFlags.SolidColor;

            if (_Data.RenderShadows)
            {
                GL.invertCulling = true;
                reflectionCamera.Render();
                GL.invertCulling = false;
            }
            else
            {
#if UNITY_5_5_OR_NEWER
                var originalShadowQuality = QualitySettings.shadows;
                QualitySettings.shadows = ShadowQuality.Disable;
# else
                var shadowResolution = QualitySettings.shadowResolution;
                QualitySettings.shadowResolution = 0;
#endif

                GL.invertCulling = true;
                reflectionCamera.Render();
                GL.invertCulling = false;

#if UNITY_5_5_OR_NEWER
                QualitySettings.shadows = originalShadowQuality;
#else
                QualitySettings.shadowResolution = shadowResolution;
#endif
            }

            reflectionCamera.targetTexture = null;

            if (_UtilitiesMaterial == null)
                _UtilitiesMaterial = new Material(_UtilitiesShader) { hideFlags = HideFlags.DontSave };

            Graphics.Blit(target, _CurrentTarget, _UtilitiesMaterial, 0);
            target.Dispose();
        }

        private void UpdateRenderProperties(Camera reflectionCamera)
        {
            var block = _Water.Renderer.PropertyBlock;
            block.SetTexture(ShaderVariables.PlanarReflectionTex, _CurrentTarget);
            block.SetMatrix("_PlanarReflectionProj",
                (Matrix4x4.TRS(new Vector3(0.5f, 0.5f, 0.0f), Quaternion.identity, new Vector3(0.5f, 0.5f, 1.0f)) *
                 reflectionCamera.projectionMatrix * reflectionCamera.worldToCameraMatrix));
            block.SetFloat("_PlanarReflectionMipBias", -Mathf.Log(1.0f / _FinalResolutionMultiplier, 2));
        }

        private TemporaryRenderTexture GetRenderTexture(int width, int height, Camera reflectionCamera)
        {
            int adaptedWidth = Mathf.ClosestPowerOfTwo(Mathf.RoundToInt(width * _FinalResolutionMultiplier));
            int adaptedHeight = Mathf.ClosestPowerOfTwo(Mathf.RoundToInt(height * _FinalResolutionMultiplier));
#if UNITY_5_6_OR_NEWER
            bool hdr = reflectionCamera.allowHDR;
#else
            bool hdr = reflectionCamera.hdr;
#endif

            var renderTexture = RenderTexturesCache.GetTemporary(adaptedWidth, adaptedHeight, 0,
                hdr && _SystemSupportsHdr && WaterProjectSettings.Instance.AllowFloatingPointMipMaps ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.ARGB32, true, false, true);

            renderTexture.Texture.filterMode = FilterMode.Trilinear;
            renderTexture.Texture.wrapMode = TextureWrapMode.Clamp;

            return renderTexture;
        }

        private void ClearRenderTextures()
        {
            var enumerator = _TemporaryTargets.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var entry = enumerator.Current;
                entry.Value.Dispose();
            }
            enumerator.Dispose();

            _TemporaryTargets.Clear();
        }

        private void OnProfilesChanged(Water water)
        {
            var profiles = water.ProfilesManager.Profiles;

            if (profiles == null)
                return;

            var intensity = 0.0f;

            for (int i = profiles.Length - 1; i >= 0; --i)
            {
                var weightedProfile = profiles[i];

                var profile = weightedProfile.Profile;
                var weight = weightedProfile.Weight;

                intensity += profile.PlanarReflectionIntensity * weight;
            }

            _RenderPlanarReflections = intensity > 0.0f;
        }
        #endregion Private Methods

        #region Helper Methods
        private void SetCameraSettings(Camera source, Camera destination)
        {
            destination.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            destination.fieldOfView = source.fieldOfView;
            destination.aspect = source.aspect;

#if UNITY_5_6_OR_NEWER
            destination.allowHDR = _SystemSupportsHdr && source.allowHDR;
#else
            destination.hdr = _SystemSupportsHdr && source.hdr;
#endif
        }

        private Vector3 CalculateReflectionPosition(Camera camera)
        {
            Vector3 position = camera.transform.position;
            position.y = _Water.transform.position.y - position.y;
            return position;
        }
        private static Vector3 CalculateReflectionAngles(Camera camera)
        {
            var angles = camera.transform.eulerAngles;
            return new Vector3(-angles.x, angles.y, angles.z);
        }
        #endregion Helper Methods
    }
}