namespace UltimateWater
{
    using UnityEngine;

    public static class WaterUtilities
    {
        #region Public Methods
        public static bool IsSceneViewCamera(Camera camera)
        {
#if UNITY_EDITOR && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2             // use 5.3 API for this
            return camera.cameraType == CameraType.SceneView;
#elif UNITY_EDITOR                                                     // fallback
            var sceneViews = UnityEditor.SceneView.sceneViews;
            int numSceneViews = sceneViews.Count;

            for(int i = 0; i < numSceneViews; ++i)
            {
                if(((UnityEditor.SceneView)sceneViews[i]).camera == camera)
                    return true;
            }

            return false;
#else
            return false;
#endif
        }
        #endregion Public Methods
    }
}