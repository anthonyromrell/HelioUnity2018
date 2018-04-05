namespace UltimateWater.Editors
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(WaterVolumeProbe))]
    public class WaterVolumeProbeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var target = (WaterVolumeProbe)this.target;

            GUI.enabled = false;
            EditorGUILayout.ObjectField("Currently in: ", target.CurrentWater, typeof(Water), true);
            GUI.enabled = true;
        }
    }
}