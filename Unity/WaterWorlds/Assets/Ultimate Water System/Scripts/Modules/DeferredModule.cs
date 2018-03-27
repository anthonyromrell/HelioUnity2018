/*
namespace UltimateWater
{
using UnityEngine;
using UnityEngine.Rendering;
using Internal;

public class DeferredModule : IRenderModule
{
 #region Public Methods
 public void OnEnable(WaterCamera camera)
 {
     _Commands = new CommandBuffer { name = "[Water] WaterCamera Utility" };

     _RefractTexId = ShaderVariables.RefractTex;

     _Gbuffer0Mix = ShaderUtility.Instance.CreateMaterial(ShaderList.GBuffer0Mix, HideFlags.DontSave);
     _Gbuffer123Mix = ShaderUtility.Instance.CreateMaterial(ShaderList.GBuffer123Mix, HideFlags.DontSave);
     _FinalColorMix = ShaderUtility.Instance.CreateMaterial(ShaderList.FinalColorMix, HideFlags.DontSave);
 }
 public void OnDisable(WaterCamera camera)
 {
     TextureUtility.ReleaseTemporary(ref _Gbuffer0Tex);
     TextureUtility.ReleaseTemporary(ref _DepthTex);

     if (_Gbuffer0Mix != null)
     {
         _Gbuffer0Mix.Destroy();
         _Gbuffer0Mix = null;
     }
     if (_Gbuffer123Mix != null)
     {
         _Gbuffer123Mix.Destroy();
         _Gbuffer123Mix = null;
     }

     if (_FinalColorMix != null)
     {
         _FinalColorMix.Destroy();
         _FinalColorMix = null;
     }

     if (_Commands != null)
     {
         _Commands.Release();
         _Commands = null;
     }
 }

 public void OnValidate(WaterCamera camera)
 {
     var shaders = ShaderUtility.Instance;
     shaders.Use(ShaderList.GBuffer0Mix);
     shaders.Use(ShaderList.GBuffer123Mix);
     shaders.Use(ShaderList.FinalColorMix);

     shaders.Use(ShaderList.DepthCopy);
     shaders.Use(ShaderList.DeferredReflections);
     shaders.Use(ShaderList.DeferredShading);
 }

 public void Process(WaterCamera waterCamera)
 {
 }
 public void Render(WaterCamera waterCamera, RenderTexture source, RenderTexture destination)
 {
     var camera = waterCamera.CameraComponent;

#if UNITY_EDITOR && UNITY_5_5_OR_NEWER
     // that takes care of this warning: "OnRenderImage() possibly didn't write anything to the destination texture!"
     Graphics.SetRenderTarget(destination);
     GL.Clear(true, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
#endif

     int width = Mathf.RoundToInt(camera.pixelWidth * waterCamera.SuperSampling) + 1;
     int height = Mathf.RoundToInt(camera.pixelHeight * waterCamera.SuperSampling) + 1;

     // get a buffer that is slightly larger to ensure that this camera buffers won't be used later this frame
     var deferredRenderTarget = RenderTexture.GetTemporary(width, height, 16, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
     deferredRenderTarget.filterMode = FilterMode.Point;

     source.filterMode = FilterMode.Point;
     Shader.SetGlobalTexture(_RefractTexId, source);

     if (source.texelSize.y < 0.0f)
     {
         var srcTemp = TextureUtility.CreateTemporary(source);
         Graphics.Blit(source, srcTemp);
         Graphics.Blit(srcTemp, source);
         TextureUtility.ReleaseTemporary(ref srcTemp);

         var dstTemp = TextureUtility.CreateTemporary(source);

         RenderWater(waterCamera, deferredRenderTarget, dstTemp);
         Graphics.Blit(dstTemp, destination);
     }
     else
     {
         RenderWater(waterCamera, deferredRenderTarget, destination);
     }

     RenderTexture.ReleaseTemporary(deferredRenderTarget);
 }
 #endregion Public Methods

 #region Private Variables
 private int _RefractTexId;

 private Material _Gbuffer0Mix;
 private Material _Gbuffer123Mix;
 private Material _FinalColorMix;

 private RenderTexture _Gbuffer0Tex;
 private RenderTexture _DepthTex;

 private CommandBuffer _Commands;

 private static readonly RenderTargetIdentifier[] _DeferredTargets = {
     BuiltinRenderTextureType.GBuffer1,
     BuiltinRenderTextureType.GBuffer2,
     BuiltinRenderTextureType.Reflections
 };
 #endregion Private Variables

 #region Private Methods
 private void RenderWater(WaterCamera waterCamera, RenderTexture temporary, RenderTexture destination)
 {
     var camera = waterCamera.CameraComponent;
     var waterRenderCamera = waterCamera.WaterRenderCamera;
     waterRenderCamera.CopyFrom(camera);

     var mainWater = waterCamera.MainWater;

     _FinalColorMix.SetMatrix(ShaderVariables.UnityMatrixVPInverse),
         Matrix4x4.Inverse(GL.GetGPUProjectionMatrix(camera.projectionMatrix, true) * camera.worldToCameraMatrix));
     _Gbuffer123Mix.SetFloat(ShaderVariables.DepthClipMultiplier, -1.0f);

     if (_Gbuffer0Tex == null)
     {
         _Gbuffer0Tex = RenderTexture.GetTemporary(camera.pixelWidth + 1, camera.pixelHeight, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
         _Gbuffer0Tex.filterMode = FilterMode.Point;
     }

     if (_DepthTex == null)
     {
         _DepthTex = RenderTexture.GetTemporary(camera.pixelWidth + 1, camera.pixelHeight, 32, RenderTextureFormat.Depth, RenderTextureReadWrite.Linear);
         _DepthTex.filterMode = FilterMode.Point;
     }

     _Commands.Clear();
     _Commands.name = "[PW Water] Blend Deferred Results";

     // depth
     var depthBlitCopyMaterial = ShaderUtility.Instance.CreateMaterial(ShaderList.DepthCopy);
     _Commands.SetRenderTarget(_DepthTex);
     _Commands.DrawMesh(UltimateWater.Internal.Quads.BipolarXInversedY, Matrix4x4.identity, depthBlitCopyMaterial, 0, 5);

     // gbuffer 0, 1, 2, 3
     _Commands.Blit(BuiltinRenderTextureType.GBuffer0, _Gbuffer0Tex, _Gbuffer0Mix, 0);
     _Commands.SetRenderTarget(_DeferredTargets, BuiltinRenderTextureType.Reflections);
     _Commands.DrawMesh(UltimateWater.Internal.Quads.BipolarXY, Matrix4x4.identity, _Gbuffer123Mix, 0);

     // final color
     _Commands.SetRenderTarget(destination);
     _Commands.SetGlobalTexture("_WaterColorTex", BuiltinRenderTextureType.CameraTarget);
     _Commands.DrawMesh(UltimateWater.Internal.Quads.BipolarXInversedY, Matrix4x4.identity, _FinalColorMix, 0, 0, mainWater != null ? mainWater.Renderer.PropertyBlock : null);

     _Commands.SetGlobalTexture("_CameraDepthTexture", _DepthTex);
     _Commands.SetGlobalTexture("_CameraGBufferTexture0", _Gbuffer0Tex);
     _Commands.SetGlobalTexture("_CameraGBufferTexture1", BuiltinRenderTextureType.GBuffer1);
     _Commands.SetGlobalTexture("_CameraGBufferTexture2", BuiltinRenderTextureType.GBuffer2);
     _Commands.SetGlobalTexture("_CameraGBufferTexture3", BuiltinRenderTextureType.Reflections);
     _Commands.SetGlobalTexture("_CameraReflectionsTexture", BuiltinRenderTextureType.Reflections);

     waterRenderCamera.AddCommandBuffer(CameraEvent.AfterEverything, _Commands);

     var originalDeferredReflections = GraphicsSettings.GetCustomShader(BuiltinShaderType.DeferredReflections);
     var originalDeferredShading = GraphicsSettings.GetCustomShader(BuiltinShaderType.DeferredShading);
     var originalShaderModeReflections = GraphicsSettings.GetShaderMode(BuiltinShaderType.DeferredReflections);
     var originalShaderModeShading = GraphicsSettings.GetShaderMode(BuiltinShaderType.DeferredShading);

     var deferredReflections = ShaderUtility.Instance.Get(ShaderList.DeferredReflections);
     var deferredShading = ShaderUtility.Instance.Get(ShaderList.DeferredShading);

     GraphicsSettings.SetCustomShader(BuiltinShaderType.DeferredReflections, deferredReflections);
     GraphicsSettings.SetCustomShader(BuiltinShaderType.DeferredShading, deferredShading);
     GraphicsSettings.SetShaderMode(BuiltinShaderType.DeferredReflections, BuiltinShaderMode.UseCustom);
     GraphicsSettings.SetShaderMode(BuiltinShaderType.DeferredShading, BuiltinShaderMode.UseCustom);

     if (mainWater != null)
         Shader.SetGlobalVector("_MainWaterWrapSubsurfaceScatteringPack", mainWater.Renderer.PropertyBlock.GetVector("_WrapSubsurfaceScatteringPack"));

     var effectWaterCamera = waterRenderCamera.GetComponent<WaterCamera>();
     effectWaterCamera.CopyFrom(waterCamera);

     waterRenderCamera.enabled = false;
     waterRenderCamera.clearFlags = CameraClearFlags.Color;
     waterRenderCamera.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
     waterRenderCamera.depthTextureMode = DepthTextureMode.None;
     waterRenderCamera.renderingPath = RenderingPath.DeferredShading;
#if UNITY_5_6_OR_NEWER
     waterRenderCamera.allowHDR = true;
#else
     waterRenderCamera.hdr = true;
#endif
     waterRenderCamera.targetTexture = temporary;
     waterRenderCamera.cullingMask = (1 << WaterProjectSettings.Instance.WaterLayer);
     waterRenderCamera.Render();
     waterRenderCamera.targetTexture = null;

     GraphicsSettings.SetCustomShader(BuiltinShaderType.DeferredReflections, originalDeferredReflections);
     GraphicsSettings.SetCustomShader(BuiltinShaderType.DeferredShading, originalDeferredShading);
     GraphicsSettings.SetShaderMode(BuiltinShaderType.DeferredReflections, originalShaderModeReflections);
     GraphicsSettings.SetShaderMode(BuiltinShaderType.DeferredShading, originalShaderModeShading);

     Shader.SetGlobalTexture("_CameraDepthTexture", _DepthTex);

     waterRenderCamera.RemoveCommandBuffer(CameraEvent.AfterEverything, _Commands);
 }
 #endregion Private Methods
}
}
*/