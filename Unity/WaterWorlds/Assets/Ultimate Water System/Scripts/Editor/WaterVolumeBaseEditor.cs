using UltimateWater.Utils;

namespace UltimateWater.Editors
{
    using UnityEditor;
    using UnityEngine;
    using UltimateWater;

    [CustomEditor(typeof(WaterVolumeBase), true)]
    public class WaterVolumeBaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var target = (WaterVolumeBase)this.target;

            var collider = target.GetComponent<Collider>();
            if (collider == null)
            {
                InspectorWarningUtility.WarningField("Collider component not found in GameObject\nAdd Collider component", InspectorWarningAttribute.InfoType.Error);
                return;
            }

            DrawDefaultInspector();
        }
    }
}