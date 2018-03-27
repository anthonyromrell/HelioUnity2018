namespace UltimateWater.Editors
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.AnimatedValues;

    [CustomEditor(typeof(Spray))]
    public class WaterSprayEditor : WaterEditorBase
    {
        #region Public Methods
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            UpdateGui();

            _UseFoldouts = true;

            if (BeginGroup("Profiling", _ProfileFoldout))
            {
                var spray = target as Spray;
                if (spray != null)
                {
                    GUILayout.Label("Draw Calls: " + Mathf.CeilToInt(spray.MaxParticles / 65535.0f));
                    GUILayout.Label("Spawned Particles: " + spray.SpawnedParticles);
                }
            }

            EndGroup();
        }
        #endregion Public Methods

        #region Private Variables
        private readonly AnimBool _ProfileFoldout = new AnimBool(false);
        #endregion Private Variables
    }
}