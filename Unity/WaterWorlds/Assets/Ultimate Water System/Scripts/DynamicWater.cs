namespace UltimateWater
{
    using System.Collections.Generic;
    using Internal;
    using UnityEngine;
    using UnityEngine.Rendering;

    public interface IDynamicWaterEffects
    {
        void Enable();
        void Disable();
    }

    public sealed class DynamicWater : WaterModule
    {
        #region Public Types
        [System.Serializable]
        public class Data
        {
            public int Antialiasing = 1;
            public LayerMask CustomEffectsLayerMask = -1;
        }
        #endregion Public Types

        #region Public Variables
        public Water Water
        {
            get { return _Water; }
        }
        #endregion Public Variables

        #region Public Methods
        public DynamicWater(Water water, Data data)
        {
            _Water = water;
            _Data = data;

            CreateCommandBuffers();
            Validate();

            _MapLocalDisplacementsShader = Shader.Find("UltimateWater/Utility/Map Local Displacements");
        }

        public void ValidateWaterComponents()
        {
            _OverlayRenderers = _Water.GetComponentsInChildren<IOverlaysRenderer>();
            var priorities = new int[_OverlayRenderers.Length];

            for (int i = 0; i < priorities.Length; ++i)
            {
                var type = _OverlayRenderers[i].GetType();
                var attributes = type.GetCustomAttributes(typeof(OverlayRendererOrderAttribute), true);

                if (attributes.Length != 0)
                    priorities[i] = ((OverlayRendererOrderAttribute)attributes[0]).Priority;
            }

            System.Array.Sort(priorities, _OverlayRenderers);
        }

        public static void AddRenderer<T>(T renderer) where T : IDynamicWaterEffects
        {
            _Renderers.Add(renderer);
        }

        public static void RemoveRenderer<T>(T renderer) where T : IDynamicWaterEffects
        {
            _Renderers.Remove(renderer);
        }

        public void RenderTotalDisplacementMap(WaterCamera camera, RenderTexture renderTexture)
        {
            Rect rect = camera.LocalMapsRect;

            var effectsCamera = camera.EffectsCamera;
            effectsCamera.enabled = false;
            effectsCamera.stereoTargetEye = StereoTargetEyeMask.None;
            effectsCamera.depthTextureMode = DepthTextureMode.None;
            effectsCamera.renderingPath = RenderingPath.VertexLit;

            effectsCamera.orthographic = true;
            effectsCamera.orthographicSize = rect.width * 0.5f;
            effectsCamera.farClipPlane = 2000.0f;
            effectsCamera.ResetProjectionMatrix();
            effectsCamera.clearFlags = CameraClearFlags.Nothing;
#if UNITY_5_6_OR_NEWER
            effectsCamera.allowHDR = true;
#else
            effectsCamera.hdr = true;
#endif
            effectsCamera.transform.position = new Vector3(rect.center.x, 1000.0f, rect.center.y);
            effectsCamera.transform.rotation = Quaternion.LookRotation(new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f));

            var effectsWaterCamera = effectsCamera.GetComponent<WaterCamera>();
            effectsWaterCamera.enabled = true;
            effectsWaterCamera.GeometryType = WaterGeometryType.UniformGrid;
            effectsWaterCamera.ForcedVertexCount = (renderTexture.width * renderTexture.height) >> 2;
            effectsWaterCamera.RenderWaterWithShader("[Ultimate Water] Render Total Displacement Map", renderTexture, _MapLocalDisplacementsShader, _Water);
        }

        public DynamicWaterCameraData GetCameraOverlaysData(Camera camera, bool createIfNotExists = true)
        {
            DynamicWaterCameraData overlaysData;

            if (!_Buffers.TryGetValue(camera, out overlaysData) && createIfNotExists)
            {
                _Buffers[camera] = overlaysData = new DynamicWaterCameraData(this, WaterCamera.GetWaterCamera(camera), _Data.Antialiasing);

                DrawDisplacementMaskRenderers(overlaysData);
                overlaysData.SwapFoamMaps();

                for (int i = 0; i < _OverlayRenderers.Length; ++i)
                    _OverlayRenderers[i].RenderOverlays(overlaysData);
            }

            if (overlaysData != null)
                overlaysData._LastFrameUsed = Time.frameCount;

            return overlaysData;
        }
        #endregion Public Methods

        #region Private Types
        private class Renderers
        {
            #region Public Variables
            public readonly List<ILocalDisplacementRenderer> LocalDisplacement = new List<ILocalDisplacementRenderer>();
            public readonly List<ILocalDisplacementMaskRenderer> LocalDisplacementMask = new List<ILocalDisplacementMaskRenderer>();
            public readonly List<ILocalFoamRenderer> LocalFoam = new List<ILocalFoamRenderer>();
            public readonly List<ILocalDiffuseRenderer> LocalDiffuse = new List<ILocalDiffuseRenderer>();
            #endregion Public Variables

            #region Public Methods
            public void Add<T>(T renderer) where T : IDynamicWaterEffects
            {
                // find what interfaces are supported
                var displacement = renderer as ILocalDisplacementRenderer;
                var displacementMask = renderer as ILocalDisplacementMaskRenderer;
                var foam = renderer as ILocalFoamRenderer;
                var color = renderer as ILocalDiffuseRenderer;
                var interactive = renderer as IWavesInteractive;

                // add renderers to queue
                if (displacement != null) { LocalDisplacement.Add(displacement); }
                if (displacementMask != null) { LocalDisplacementMask.Add(displacementMask); }
                if (foam != null) { LocalFoam.Add(foam); }
                if (interactive != null) { Interactions.Add(interactive); }
                if (color != null) { LocalDiffuse.Add(color); }
            }

            public void Remove<T>(T renderer) where T : IDynamicWaterEffects
            {
                // find what interfaces are supported
                var displacement = renderer as ILocalDisplacementRenderer;
                var displacementMask = renderer as ILocalDisplacementMaskRenderer;
                var foam = renderer as ILocalFoamRenderer;
                var color = renderer as ILocalDiffuseRenderer;
                var interactive = renderer as IWavesInteractive;

                // remove renderers from queue
                if (displacement != null) { LocalDisplacement.Remove(displacement); }
                if (displacementMask != null) { LocalDisplacementMask.Remove(displacementMask); }
                if (foam != null) { LocalFoam.Remove(foam); }
                if (interactive != null) { Interactions.Remove(interactive); }
                if (color != null) { LocalDiffuse.Remove(color); }
            }
            #endregion Public Methods
        }
        #endregion Private Types

        #region Private Variables
        private IOverlaysRenderer[] _OverlayRenderers;
        private Material _MapLocalDisplacementsMaterial;

        private static readonly Renderers _Renderers = new Renderers();

        public static List<IWavesInteractive> Interactions = new List<IWavesInteractive>();

        private readonly Water _Water;
        private readonly Data _Data;
        private readonly Shader _MapLocalDisplacementsShader;

        private static CommandBuffer _RenderDisplacementBuffer;
        private static CommandBuffer _RenderDisplacementMaskBuffer;
        private static CommandBuffer _RenderFoamBuffer;
        private static CommandBuffer _RenderDiffuseBuffer;

        private readonly Dictionary<Camera, DynamicWaterCameraData> _Buffers = new Dictionary<Camera, DynamicWaterCameraData>();
        private readonly List<Camera> _LostCameras = new List<Camera>();
        private static readonly RenderTargetIdentifier[] _CustomEffectsBuffers = new RenderTargetIdentifier[2];
        #endregion Private Variables

        #region Private Methods
        internal override void OnWaterRender(WaterCamera waterCamera)
        {
            var camera = waterCamera.CameraComponent;

            if (waterCamera.Type != WaterCamera.CameraType.Normal || !Application.isPlaying)
                return;

            // do not render in scene view if scene view rendering is disabled
            if (!WaterProjectSettings.Instance.RenderInSceneView && WaterUtilities.IsSceneViewCamera(camera))
            {
                return;
            }

            Shader.SetGlobalVector("_TileSizesInv", _Water.WindWaves.TileSizesInv);

            var overlays = GetCameraOverlaysData(camera);
            overlays.SwapFoamMaps();
            overlays.ClearOverlays();

            DrawDisplacementMaskRenderers(overlays);
            DrawDisplacementRenderers(overlays);

            for (int i = 0; i < _OverlayRenderers.Length; ++i)
            {
                _OverlayRenderers[i].RenderOverlays(overlays);
            }

            if (_Water.Foam != null)
            {
                _Water.Foam.RenderOverlays(overlays);
            }

            for (int i = 0; i < _OverlayRenderers.Length; ++i)
            {
                _OverlayRenderers[i].RenderFoam(overlays);
            }

            int diffuseId = ShaderVariables.LocalDiffuseMap;

            var block = _Water.Renderer.PropertyBlock;
            block.SetTexture(ShaderVariables.LocalDisplacementMap, overlays.DynamicDisplacementMap);
            block.SetTexture(ShaderVariables.LocalNormalMap, overlays.NormalMap);
            block.SetTexture(ShaderVariables.TotalDisplacementMap, overlays.TotalDisplacementMap);
            block.SetTexture(ShaderVariables.DisplacementsMask, overlays.DisplacementsMask);
            block.SetTexture(diffuseId, overlays.DiffuseMap);
            Shader.SetGlobalTexture("DIFFUSE", overlays.DiffuseMap);

            var debugMap = overlays.GetDebugMap();

            if (debugMap != null)
                block.SetTexture("_LocalDebugMap", debugMap);

            if (waterCamera.MainWater == _Water)
                Shader.SetGlobalTexture(ShaderVariables.TotalDisplacementMap, overlays.TotalDisplacementMap);

            DrawFoamRenderers(overlays);
            DrawDiffuseRenderers(overlays);
        }

        internal override void Enable()
        {
            ValidateWaterComponents();

            if (_MapLocalDisplacementsMaterial == null)
                _MapLocalDisplacementsMaterial = new Material(_MapLocalDisplacementsShader) { hideFlags = HideFlags.DontSave };
        }
        internal override void Disable()
        {
            foreach (var entry in _Buffers)
            {
                entry.Value.Dispose();
            }
            _Buffers.Clear();
        }

        internal override void Destroy()
        {
            foreach (var entry in _Buffers)
            {
                entry.Value.Dispose();
            }
            _Buffers.Clear();
        }

        internal override void Validate()
        {
        }

        internal override void Update()
        {
            int frameIndex = Time.frameCount - 3;

            var enumerator = _Buffers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Value._LastFrameUsed < frameIndex)
                {
                    enumerator.Current.Value.Dispose();
                    _LostCameras.Add(enumerator.Current.Key);
                }
            }
            enumerator.Dispose();

            for (int i = 0; i < _LostCameras.Count; ++i)
            {
                _Buffers.Remove(_LostCameras[i]);
            }

            _LostCameras.Clear();
        }

        private static void DrawDisplacementRenderers(DynamicWaterCameraData overlays)
        {
            var effectsCamera = overlays.Camera.CameraComponent;

            _CustomEffectsBuffers[0] = overlays.DynamicDisplacementMap;
            _CustomEffectsBuffers[1] = overlays.NormalMap;

            GL.PushMatrix();
            GL.modelview = effectsCamera.worldToCameraMatrix;
            GL.LoadProjectionMatrix(effectsCamera.projectionMatrix);

            _RenderDisplacementBuffer.Clear();
            _RenderDisplacementBuffer.SetRenderTarget(_CustomEffectsBuffers, overlays.DynamicDisplacementMap);

            var renderers = _Renderers.LocalDisplacement;

            for (int i = renderers.Count - 1; i >= 0; --i)
            {
                renderers[i].RenderLocalDisplacement(_RenderDisplacementBuffer, overlays);
            }

            Graphics.ExecuteCommandBuffer(_RenderDisplacementBuffer);
            GL.PopMatrix();
        }
        private static void DrawDisplacementMaskRenderers(DynamicWaterCameraData overlays)
        {
            Shader.SetGlobalTexture("_CameraDepthTexture", DefaultTextures.Get(Color.white));

            var effectsCamera = overlays.Camera.CameraComponent;

            GL.PushMatrix();
            GL.modelview = effectsCamera.worldToCameraMatrix;
            GL.LoadProjectionMatrix(effectsCamera.projectionMatrix);

            _RenderDisplacementMaskBuffer.Clear();
            _RenderDisplacementMaskBuffer.SetRenderTarget(overlays.DisplacementsMask);

            var renderers = _Renderers.LocalDisplacementMask;

            for (int i = renderers.Count - 1; i >= 0; --i)
            {
                renderers[i].RenderLocalMask(_RenderDisplacementMaskBuffer, overlays);
            }

            Graphics.ExecuteCommandBuffer(_RenderDisplacementMaskBuffer);
            GL.PopMatrix();
        }
        private static void DrawFoamRenderers(DynamicWaterCameraData overlays)
        {
            var renderers = _Renderers.LocalFoam;
            for (int i = renderers.Count - 1; i >= 0; --i)
            {
                renderers[i].Enable();
            }

            Shader.SetGlobalTexture("_CameraDepthTexture", DefaultTextures.Get(Color.white));

            var effectsCamera = overlays.Camera.CameraComponent;

            GL.PushMatrix();
            GL.modelview = effectsCamera.worldToCameraMatrix;
            GL.LoadProjectionMatrix(effectsCamera.projectionMatrix);

            _RenderFoamBuffer.Clear();
            _RenderFoamBuffer.SetRenderTarget(overlays.FoamMap);

            for (int i = renderers.Count - 1; i >= 0; --i)
            {
                renderers[i].RenderLocalFoam(_RenderFoamBuffer, overlays);
            }

            Graphics.ExecuteCommandBuffer(_RenderFoamBuffer);
            GL.PopMatrix();

            for (int i = renderers.Count - 1; i >= 0; --i)
            {
                renderers[i].Disable();
            }
        }
        private static void DrawDiffuseRenderers(DynamicWaterCameraData overlays)
        {
            var renderers = _Renderers.LocalDiffuse;
            for (int i = renderers.Count - 1; i >= 0; --i)
            {
                renderers[i].Enable();
            }

            Shader.SetGlobalTexture("_CameraDepthTexture", DefaultTextures.Get(Color.white));

            var effectsCamera = overlays.Camera.CameraComponent;
            GL.PushMatrix();
            GL.modelview = effectsCamera.worldToCameraMatrix;
            GL.LoadProjectionMatrix(effectsCamera.projectionMatrix);

            _RenderDiffuseBuffer.Clear();
            _RenderDiffuseBuffer.SetRenderTarget(overlays.DiffuseMap);

            for (int i = renderers.Count - 1; i >= 0; --i)
            {
                renderers[i].RenderLocalDiffuse(_RenderDiffuseBuffer, overlays);
            }

            Graphics.ExecuteCommandBuffer(_RenderDiffuseBuffer);
            GL.PopMatrix();

            for (int i = renderers.Count - 1; i >= 0; --i)
            {
                renderers[i].Disable();
            }
        }

        private static void CreateCommandBuffers()
        {
            _RenderDisplacementBuffer = new CommandBuffer { name = "[Ultimate Water] Displacement Renderers" };
            _RenderDisplacementMaskBuffer = new CommandBuffer { name = "[Ultimate Water] Displacement Mask Renderers" };

            _RenderFoamBuffer = new CommandBuffer { name = "[Ultimate Water] Foam Renderers" };
            _RenderDiffuseBuffer = new CommandBuffer { name = "[Ultimate Water] Diffuse Renderers" };
        }
        private static void ReleaseCommandBuffers()
        {
            if (_RenderDisplacementBuffer != null) { _RenderDisplacementBuffer.Release(); }
            if (_RenderDisplacementMaskBuffer != null) { _RenderDisplacementMaskBuffer.Release(); }
            if (_RenderFoamBuffer != null) { _RenderFoamBuffer.Release(); }
            if (_RenderDiffuseBuffer != null) { _RenderDiffuseBuffer.Release(); }
        }
        #endregion Private Methods
    }
}