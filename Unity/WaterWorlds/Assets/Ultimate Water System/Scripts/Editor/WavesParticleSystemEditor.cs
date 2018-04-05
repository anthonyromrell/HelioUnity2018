using UnityEngine;
using UnityEditor;

namespace UltimateWater.Editors
{
    [CustomEditor(typeof(WaveParticleSystem))]
    public class WavesParticleSystemEditor : WaterEditor
    {
        public override void OnInspectorGUI()
        {
            var target = (WaveParticleSystem)this.target;

            PropertyField("_MaxParticles");
            PropertyField("_MaxParticlesPerTile");
            PropertyField("_PrewarmTime");
            PropertyField("_TimePerFrame");

            if (Application.isPlaying)
            {
                GUI.enabled = false;
                EditorGUILayout.IntField("Particle Count", target.ParticleCount);
                GUI.enabled = true;
            }

            serializedObject.ApplyModifiedProperties();
        }

        public override bool RequiresConstantRepaint()
        {
            return Application.isPlaying;
        }
    }
}