using UltimateWater.Utils;

namespace UltimateWater.Editors
{
    using UnityEngine;
    using UnityEditor;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(WaterPhysics))]
    public class WaterPhysicsEditor : WaterEditorBase
    {
        public override void OnInspectorGUI()
        {
            var physics = (WaterPhysics)target;

            var rigidbody = physics.GetComponent<Rigidbody>();
            var collider = physics.GetComponent<Collider>();

            if (rigidbody == null || collider == null)
            {
                var info = string.Empty;
                if (rigidbody == null) info += "Missing Rigidbody component\n";
                if (collider == null) info += "Missing Collider component";

                InspectorWarningUtility.WarningField(info, InspectorWarningAttribute.InfoType.Error);
                return;
            }

            PropertyField("sampleCount");
            PropertyField("dragCoefficient");
            PropertyField("precision");
            PropertyField("buoyancyIntensity");
            PropertyField("flowIntensity");

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            PropertyField("useImprovedDragAndFlowForces");
            GUILayout.Label("If you would like to use the improved drag force, ensure that:\n1. The attached collider is a MeshCollider.\n2. It's mesh is composed of 10-100 polygons.\n3. If it's a ship, ensure that the bow is perfectly symmetrical or\nyour ship won't be moving perfectly forward.");
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            float totalBuoyancy = physics.GetTotalBuoyancy();
            EditorGUILayout.LabelField(new GUIContent("Gravity Balance", "Buoyancy stated as a percent of the gravity force."), new GUIContent((100.0f * totalBuoyancy / Physics.gravity.magnitude).ToString("0.00") + "%"));

            if (GUILayout.Button("Set Mass to obtain 100% Gravity Balance"))
            {
                rigidbody = physics.GetComponent<Rigidbody>();
                rigidbody.mass = physics.GetEquilibriumMass();
            }
        }
    }
}