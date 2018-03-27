namespace UltimateWater.Internal
{
    using UnityEngine;

    public class Reflection
    {
        #region Public Methods
        public static Camera CreateCamera(Camera camera)
        {
            var obj = new GameObject("Reflection Camera");
            obj.transform.SetParent(camera.transform);

            var reflection = obj.AddComponent<Camera>();
            reflection.renderingPath = RenderingPath.Forward;
            reflection.stereoTargetEye = StereoTargetEyeMask.None;
            reflection.enabled = false;
            reflection.depthTextureMode = DepthTextureMode.None;

            return reflection;
        }
        #endregion Public Methods

        #region Helper Methods
        public static Camera GetReflectionCamera(Camera camera)
        {
            var waterCamera = camera.gameObject.GetComponent<WaterCamera>();
            if (waterCamera == null)
            {
                Debug.LogError("WaterCamera not found");
                return null;
            }

            if (waterCamera.ReflectionCamera == null)
            {
                waterCamera.ReflectionCamera = CreateCamera(camera);
            }
            return waterCamera.ReflectionCamera;
        }

        public static Matrix4x4 CalculateReflectionMatrix(Matrix4x4 reflectionMat, Vector4 plane)
        {
            reflectionMat.m00 = (1.0f - 2.0f * plane.x * plane.x);
            reflectionMat.m01 = (-2.0f * plane.x * plane.y);
            reflectionMat.m02 = (-2.0f * plane.x * plane.z);
            reflectionMat.m03 = (-2.0f * plane.w * plane.x);

            reflectionMat.m10 = (-2.0f * plane.y * plane.x);
            reflectionMat.m11 = (1.0f - 2.0f * plane.y * plane.y);
            reflectionMat.m12 = (-2.0f * plane.y * plane.z);
            reflectionMat.m13 = (-2.0f * plane.w * plane.y);

            reflectionMat.m20 = (-2.0f * plane.z * plane.x);
            reflectionMat.m21 = (-2.0f * plane.z * plane.y);
            reflectionMat.m22 = (1.0f - 2.0f * plane.z * plane.z);
            reflectionMat.m23 = (-2.0f * plane.w * plane.z);

            reflectionMat.m30 = 0.0f;
            reflectionMat.m31 = 0.0f;
            reflectionMat.m32 = 0.0f;
            reflectionMat.m33 = 1.0f;

            return reflectionMat;
        }

        public static Matrix4x4 CalculateObliqueMatrix(Matrix4x4 projection, Vector4 clipPlane)
        {
            Vector4 q = projection.inverse * new Vector4(Mathf.Sign(clipPlane.x), Mathf.Sign(clipPlane.y), 1.0f, 1.0f);

            Vector4 c = clipPlane * (2.0f / (Vector4.Dot(clipPlane, q)));
            projection[2] = c.x - projection[3];
            projection[6] = c.y - projection[7];
            projection[10] = c.z - projection[11];
            projection[14] = c.w - projection[15];

#if UNITY_5_4_0
// it seems that there is some bug in Unity 5.4.0 code that does something weird with projection matrix when any of these components is set to non-zero
            if(UnityEngine.VR.VRSettings.enabled)
            {
                projection[2] = 0.0f;
                projection[6] = 0.0f;
            }
#endif

            return projection;
        }

        public static Vector4 CameraSpacePlane(Camera camera, Vector3 position, Vector3 normal, float clipPlaneOffset, float sideSign)
        {
            Vector3 offsetPos = position + normal * clipPlaneOffset;
            Matrix4x4 m = camera.worldToCameraMatrix;
            Vector3 cpos = m.MultiplyPoint(offsetPos);
            Vector3 cnormal = m.MultiplyVector(normal).normalized * sideSign;

            return new Vector4(cnormal.x, cnormal.y, cnormal.z, -Vector3.Dot(cpos, cnormal));
        }
        #endregion Helper Methods
    }
}