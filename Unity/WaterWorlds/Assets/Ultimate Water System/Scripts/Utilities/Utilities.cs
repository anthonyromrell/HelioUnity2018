namespace UltimateWater
{
    using UnityEngine;

    public static class Utilities
    {
        #region LayerMask Manipulation
        // Finds the [0,31] value of the provided mask
        // Asserts that only one mask is set
        public static int LayerMaskToInt(LayerMask mask)
        {
            for (int i = 0; i < 32; ++i)
            {
                if ((mask.value & (1 << i)) != 0)
                {
                    return i;
                }
            }

            return -1;
        }
        #endregion LayerMask Manipulation

        #region Misc
        public static Water GetWaterReference()
        {
            return FindRefenrece<Water>();
        }

        /// <summary>
        /// Helper method for checking if given object is not null.
        /// If the reference is null, disables the component and prints debug error
        /// </summary>
        /// <returns>Is the reference null</returns>
        public static bool IsNullReference<T>(this T obj, MonoBehaviour caller = null) where T : Object
        {
            if (obj != null)
            {
                return false;
            }

            string callerName = "";
            var mth = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod();
            if (mth != null && mth.ReflectedType != null)
            {
                callerName = "[" + mth.ReflectedType.Name + "]: ";
            }

            Debug.LogError("[Ultimate Water System]" + callerName + "reference to the: " + typeof(T) + " not set.");

            if (caller != null)
            {
                caller.enabled = false;
            }

            return true;
        }

        public static void Destroy(this Object obj)
        {
#if !UNITY_EDITOR
            Object.Destroy(obj);
#else
            if (Application.isPlaying)
                Object.Destroy(obj);
            else
                Object.DestroyImmediate(obj);
#endif
        }
        #endregion Misc

        #region Private Methods
        private static T FindRefenrece<T>() where T : MonoBehaviour
        {
            var references = Object.FindObjectsOfType(typeof(T));

            // If reference not found or multiple references found on scene
            if (references.Length == 0 || references.Length > 1)
            {
                // We cannot determine default reference
                return null;
            }

            return references[0] as T;
        }
        #endregion Private Methods
    }
}