namespace UltimateWater
{
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.Rendering;

    using Internal;
    using UnityEngine.Serialization;

    /// <summary>
    ///     Renders water.
    ///     <seealso cref="Water.Renderer" />
    /// </summary>
    [System.Serializable]
    public class WaterRenderer
    {
        #region Public Variables
        public int MaskCount
        {
            get { return _Masks.Count; }
        }

        public MaterialPropertyBlock PropertyBlock
        {
            get { return _PropertyBlock != null ? _PropertyBlock : (_PropertyBlock = new MaterialPropertyBlock()); }
        }

        public Transform ReflectionProbeAnchor
        {
            get { return _ReflectionProbeAnchor; }
            set { _ReflectionProbeAnchor = value; }
        }

        public void AddMask(WaterSimpleMask mask)
        {
            mask.Renderer.enabled = false;

            int priority = mask.RenderQueuePriority;

            for (int i = _Masks.Count - 1; i >= 0; --i)
            {
                var mask1 = _Masks[i];

                if (mask1.RenderQueuePriority <= priority)
                {
                    _Masks.Insert(i + 1, mask);
                    return;
                }
            }

            _Masks.Insert(0, mask);
        }

        public void RemoveMask(WaterSimpleMask mask)
        {
            _Masks.Remove(mask);
        }
        #endregion Public Variables

        #region Public Methods
        public void RenderEffects(WaterCamera waterCamera)
        {
            var camera = waterCamera.CameraComponent;
            if (!_Water.isActiveAndEnabled || (camera.cullingMask & (1 << _Water.gameObject.layer)) == 0)
                return;

            if ((!_Water.Volume.Boundless && _Water.Volume.HasRenderableAdditiveVolumes && !waterCamera.RenderVolumes))
                return;

            _Water.OnWaterRender(waterCamera);
        }

        public void Render(Camera camera, WaterGeometryType geometryType, CommandBuffer commandBuffer = null, Shader shader = null)
        {
            if (!_Water.isActiveAndEnabled || (camera.cullingMask & (1 << _Water.gameObject.layer)) == 0)
                return;

            var waterCamera = WaterCamera.GetWaterCamera(camera);
            bool hasWaterCamera = (object)waterCamera != null;

            if (!hasWaterCamera || (!_Water.Volume.Boundless && _Water.Volume.HasRenderableAdditiveVolumes && !waterCamera.RenderVolumes))
                return;

            if (_Water.ShaderSet.ReceiveShadows)
            {
                Vector2 min = new Vector2(0.0f, 0.0f);
                Vector2 max = new Vector2(1.0f, 1.0f);
                waterCamera.ReportShadowedWaterMinMaxRect(min, max);
            }

            if (!_UseSharedMask)
                RenderMasks(camera, waterCamera, _PropertyBlock);

            Matrix4x4 matrix;
            var meshes = _Water.Geometry.GetTransformedMeshes(camera, out matrix, geometryType, false, waterCamera.ForcedVertexCount);

            if (commandBuffer == null)
            {
                Camera finalCamera = waterCamera.RenderMode != WaterRenderMode.DefaultQueue ? waterCamera._WaterRenderCamera : camera;

                for (int i = 0; i < meshes.Length; ++i)
                {
                    Graphics.DrawMesh(meshes[i], matrix, _Water.Materials.SurfaceMaterial, _Water.gameObject.layer, finalCamera, 0,
                        _PropertyBlock, _ShadowCastingMode, false, _ReflectionProbeAnchor != null ? _ReflectionProbeAnchor : _Water.transform,
                        false);

                    if (waterCamera.ContainingWater != null && waterCamera.Type == WaterCamera.CameraType.Normal)
                    {
                        Graphics.DrawMesh(meshes[i], matrix, _Water.Materials.SurfaceBackMaterial, _Water.gameObject.layer, finalCamera, 0,
                            _PropertyBlock, _ShadowCastingMode, false, _ReflectionProbeAnchor != null ? _ReflectionProbeAnchor : _Water.transform,
                            false);
                    }
                }
            }
            else
            {
                var material = UtilityShaderVariants.Instance.GetVariant(shader, _Water.Materials.UsedKeywords);

                for (int i = 0; i < meshes.Length; ++i)
                    commandBuffer.DrawMesh(meshes[i], matrix, material, 0, 0, _PropertyBlock);
            }
        }

        public void RenderVolumes(CommandBuffer commandBuffer, Shader shader, bool twoPass)
        {
            if (!_Water.enabled) return;

            var material = UtilityShaderVariants.Instance.GetVariant(shader, _Water.Materials.UsedKeywords);
            var boundingVolumes = _Water.Volume.GetVolumesDirect();
            RenderVolumes(commandBuffer, material, boundingVolumes, twoPass);

            var subtractiveVolumes = _Water.Volume.GetSubtractiveVolumesDirect();
            RenderVolumes(commandBuffer, material, subtractiveVolumes, twoPass);
        }

        public void RenderMasks(CommandBuffer commandBuffer)
        {
            for (int i = _Masks.Count - 1; i >= 0; --i)
            {
                commandBuffer.DrawMesh(_Masks[i].GetComponent<MeshFilter>().sharedMesh,
                    _Masks[i].transform.localToWorldMatrix, _Masks[i].Renderer.sharedMaterial, 0, 0);
            }
        }

        public void PostRender(WaterCamera waterCamera)
        {
            if (_Water != null)
                _Water.OnWaterPostRender(waterCamera);

            ReleaseTemporaryBuffers();
        }

        public void OnSharedSubtractiveMaskRender(ref bool hasSubtractiveVolumes, ref bool hasAdditiveVolumes, ref bool hasFlatMasks)
        {
            if (!_Water.enabled) return;

            var boundingVolumes = _Water.Volume.GetVolumesDirect();
            int numBoundingVolumes = boundingVolumes.Count;

            for (int i = 0; i < numBoundingVolumes; ++i)
                boundingVolumes[i].DisableRenderers();

            var subtractiveVolumes = _Water.Volume.GetSubtractiveVolumesDirect();
            int numSubtractiveVolumes = subtractiveVolumes.Count;

            if (_UseSharedMask)
            {
                for (int i = 0; i < numSubtractiveVolumes; ++i)
                    subtractiveVolumes[i].EnableRenderers(false);

                hasSubtractiveVolumes = hasSubtractiveVolumes || _Water.Volume.GetSubtractiveVolumesDirect().Count != 0;
                hasAdditiveVolumes = hasAdditiveVolumes || numBoundingVolumes != 0;
                hasFlatMasks = hasFlatMasks || _Masks.Count != 0;
            }
            else
            {
                for (int i = 0; i < numSubtractiveVolumes; ++i)
                    subtractiveVolumes[i].DisableRenderers();
            }
        }

        public void OnSharedMaskAdditiveRender()
        {
            if (!_Water.enabled) return;

            if (_UseSharedMask)
            {
                var boundingVolumes = _Water.Volume.GetVolumesDirect();
                int numBoundingVolumes = boundingVolumes.Count;

                for (int i = 0; i < numBoundingVolumes; ++i)
                    boundingVolumes[i].EnableRenderers(false);

                var subtractiveVolumes = _Water.Volume.GetSubtractiveVolumesDirect();
                int numSubtractiveVolumes = subtractiveVolumes.Count;

                for (int i = 0; i < numSubtractiveVolumes; ++i)
                    subtractiveVolumes[i].DisableRenderers();
            }
        }

        public void OnSharedMaskPostRender()
        {
            if (!_Water.enabled) return;

            var boundingVolumes = _Water.Volume.GetVolumesDirect();
            int numBoundingVolumes = boundingVolumes.Count;

            for (int i = 0; i < numBoundingVolumes; ++i)
                boundingVolumes[i].EnableRenderers(true);

            var subtractiveVolumes = _Water.Volume.GetSubtractiveVolumesDirect();
            int numSubtractiveVolumes = subtractiveVolumes.Count;

            for (int i = 0; i < numSubtractiveVolumes; ++i)
                subtractiveVolumes[i].EnableRenderers(true);
        }
        #endregion Public Methods

        #region Inspector Variables
        [HideInInspector, SerializeField, FormerlySerializedAs("volumeFrontShader")] private Shader _VolumeFrontShader;
        [HideInInspector, SerializeField, FormerlySerializedAs("volumeFrontFastShader")] private Shader _VolumeFrontFastShader;
        [HideInInspector, SerializeField, FormerlySerializedAs("volumeBackShader")] private Shader _VolumeBackShader;

        [SerializeField, FormerlySerializedAs("reflectionProbeAnchor")] private Transform _ReflectionProbeAnchor;
        [SerializeField, FormerlySerializedAs("shadowCastingMode")] private ShadowCastingMode _ShadowCastingMode;
        [SerializeField, FormerlySerializedAs("useSharedMask")] private bool _UseSharedMask = true;
        #endregion Inspector Variables

        #region Private Variables
        private Water _Water;
        private MaterialPropertyBlock _PropertyBlock;
        private RenderTexture _AdditiveMaskTexture;
        private RenderTexture _SubtractiveMaskTexture;
        private readonly List<WaterSimpleMask> _Masks = new List<WaterSimpleMask>();
        #endregion Private Variables

        #region Private Methods
        internal void Awake(Water water)
        {
            _Water = water;
        }

        internal void OnValidate()
        {
            if (_VolumeFrontShader == null)
                _VolumeFrontShader = Shader.Find("UltimateWater/Volumes/Front");

            if (_VolumeFrontFastShader == null)
                _VolumeFrontFastShader = Shader.Find("UltimateWater/Volumes/Front Simple");

            if (_VolumeBackShader == null)
                _VolumeBackShader = Shader.Find("UltimateWater/Volumes/Back");
        }

        private static void RenderVolumes<T>(CommandBuffer commandBuffer, Material material, List<T> boundingVolumes, bool twoPass) where T : WaterVolumeBase
        {
            for (int i = boundingVolumes.Count - 1; i >= 0; --i)
            {
                var volumeRenderers = boundingVolumes[i].VolumeRenderers;
                var volumeFilters = boundingVolumes[i].VolumeFilters;

                if (volumeRenderers == null || volumeRenderers.Length == 0 || !volumeRenderers[0].enabled)
                    continue;

                if (!twoPass)
                {
                    int passIndex = material.passCount == 1 ? 0 : 1;

                    for (int ii = 0; ii < volumeRenderers.Length; ++ii)
                        commandBuffer.DrawMesh(volumeFilters[ii].sharedMesh, volumeRenderers[ii].transform.localToWorldMatrix, material, 0, passIndex);
                }
                else
                {
                    for (int ii = 0; ii < volumeRenderers.Length; ++ii)
                    {
                        var mesh = volumeFilters[ii].sharedMesh;
                        commandBuffer.DrawMesh(mesh, volumeRenderers[ii].transform.localToWorldMatrix, material, 0, 0);
                        commandBuffer.DrawMesh(mesh, volumeRenderers[ii].transform.localToWorldMatrix, material, 0, 1);
                    }
                }
            }
        }

        private void ReleaseTemporaryBuffers()
        {
            if (_AdditiveMaskTexture != null)
            {
                RenderTexture.ReleaseTemporary(_AdditiveMaskTexture);
                _AdditiveMaskTexture = null;
            }

            if (_SubtractiveMaskTexture != null)
            {
                RenderTexture.ReleaseTemporary(_SubtractiveMaskTexture);
                _SubtractiveMaskTexture = null;
            }
        }

        private void RenderMasks(Camera camera, WaterCamera waterCamera, MaterialPropertyBlock propertyBlock)
        {
            var subtractiveVolumes = _Water.Volume.GetSubtractiveVolumesDirect();
            var additiveVolumes = _Water.Volume.GetVolumesDirect();

            // ReSharper disable once RedundantCast.0
            if ((object)waterCamera == null || !waterCamera.RenderVolumes || (subtractiveVolumes.Count == 0 && additiveVolumes.Count == 0 && _Masks.Count == 0))
            {
                ReleaseTemporaryBuffers();
                return;
            }

            int tempLayer = WaterProjectSettings.Instance.WaterTempLayer;
            int waterLayer = WaterProjectSettings.Instance.WaterCollidersLayer;

            var effectsCamera = waterCamera.EffectsCamera;

            if (effectsCamera == null)
            {
                ReleaseTemporaryBuffers();
                return;
            }

            bool t1 = false, t2 = false, t3 = false;
            OnSharedSubtractiveMaskRender(ref t1, ref t2, ref t3);

            effectsCamera.CopyFrom(camera);
            effectsCamera.enabled = false;
            effectsCamera.GetComponent<WaterCamera>().enabled = false;
            effectsCamera.renderingPath = RenderingPath.Forward;
            effectsCamera.depthTextureMode = DepthTextureMode.None;
            effectsCamera.cullingMask = 1 << tempLayer;

            if (subtractiveVolumes.Count != 0)
            {
                if (_SubtractiveMaskTexture == null)
                    _SubtractiveMaskTexture = RenderTexture.GetTemporary(camera.pixelWidth, camera.pixelHeight, 24, SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat) ? RenderTextureFormat.ARGBFloat : RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear, 1);

                Graphics.SetRenderTarget(_SubtractiveMaskTexture);

                int numsubtractiveVolumes = subtractiveVolumes.Count;
                for (int i = 0; i < numsubtractiveVolumes; ++i)
                    subtractiveVolumes[i].SetLayer(tempLayer);

                var volumeFrontTexture = RenderTexturesCache.GetTemporary(camera.pixelWidth, camera.pixelHeight, 24, SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat) ? RenderTextureFormat.ARGBFloat : RenderTextureFormat.ARGBHalf, true, false);

                // render front pass of volumetric masks
                effectsCamera.clearFlags = CameraClearFlags.SolidColor;
                effectsCamera.backgroundColor = new Color(0.0f, 0.0f, 0.5f, 0.0f);
                effectsCamera.targetTexture = volumeFrontTexture;
                effectsCamera.RenderWithShader(_VolumeFrontShader, "CustomType");

                GL.Clear(true, true, new Color(0.0f, 0.0f, 0.0f, 0.0f), 0.0f);

                // render back pass of volumetric masks
                Shader.SetGlobalTexture("_VolumesFrontDepth", volumeFrontTexture);
                effectsCamera.clearFlags = CameraClearFlags.Nothing;
                effectsCamera.targetTexture = _SubtractiveMaskTexture;
                effectsCamera.RenderWithShader(_VolumeBackShader, "CustomType");

                volumeFrontTexture.Dispose();

                for (int i = 0; i < numsubtractiveVolumes; ++i)
                    subtractiveVolumes[i].SetLayer(waterLayer);

                propertyBlock.SetTexture(ShaderVariables.SubtractiveMask, _SubtractiveMaskTexture);
            }

            if (additiveVolumes.Count != 0)
            {
                OnSharedMaskAdditiveRender();

                if (_AdditiveMaskTexture == null)
                    _AdditiveMaskTexture = RenderTexture.GetTemporary(camera.pixelWidth, camera.pixelHeight, 16, SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat) ? RenderTextureFormat.ARGBFloat : RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear, 1);

                Graphics.SetRenderTarget(_AdditiveMaskTexture);
                GL.Clear(true, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));

                int numBoundingVolumes = additiveVolumes.Count;
                for (int i = 0; i < numBoundingVolumes; ++i)
                {
                    additiveVolumes[i].SetLayer(tempLayer);
                    additiveVolumes[i].EnableRenderers(false);
                }

                // render additive volumes
                effectsCamera.clearFlags = CameraClearFlags.Nothing;
                effectsCamera.targetTexture = _AdditiveMaskTexture;
                effectsCamera.RenderWithShader(waterCamera.IsInsideAdditiveVolume ? _VolumeFrontShader : _VolumeFrontFastShader, "CustomType");

                effectsCamera.clearFlags = CameraClearFlags.Nothing;
                effectsCamera.targetTexture = _AdditiveMaskTexture;
                effectsCamera.RenderWithShader(_VolumeBackShader, "CustomType");

                for (int i = 0; i < numBoundingVolumes; ++i)
                    additiveVolumes[i].SetLayer(waterLayer);

                propertyBlock.SetTexture(ShaderVariables.AdditiveMask, _AdditiveMaskTexture);
            }

            OnSharedMaskPostRender();

            effectsCamera.targetTexture = null;
        }
        #endregion Private Methods
    }
}