using UnityEditor;

namespace UltimateWater.Editors
{
    [CustomEditor(typeof(ComplexWavesEmitter))]
    public class WavesEmitterEditor : WaterEditor
    {
        public override void OnInspectorGUI()
        {
            PropertyField("_WavesParticleSystem");

            var wavesSourceProp = PropertyField("_WavesSource");
            var wavesSource = (ComplexWavesEmitter.WavesSource)wavesSourceProp.enumValueIndex;

            switch (wavesSource)
            {
                case ComplexWavesEmitter.WavesSource.CustomWaveFrequency:
                    {
                        PropertyField("_Wavelength");
                        PropertyField("_Amplitude");
                        PropertyField("_EmissionRate");
                        PropertyField("_Width");
                        PropertyField("_WaveShapeIrregularity");
                        PropertyField("_Lifetime");
                        PropertyField("_ShoreWaves");
                        break;
                    }

                case ComplexWavesEmitter.WavesSource.WindWavesSpectrum:
                    {
                        PropertyField("_SpectrumCoincidenceRange");
                        PropertyField("_SpectrumWavesCount");
                        PropertyField("_Span");
                        PropertyField("_WaveShapeIrregularity");
                        PropertyField("_Lifetime");
                        PropertyField("_EmissionFrequencyScale");
                        break;
                    }

                case ComplexWavesEmitter.WavesSource.Shoaling:
                    {
                        PropertyField("_BoundsSize");
                        PropertyField("_Span");
                        PropertyField("_Lifetime");
                        PropertyField("_WaveShapeIrregularity");
                        PropertyField("_SpawnDepth");
                        PropertyField("_EmissionFrequencyScale");
                        PropertyField("_SpawnPointsDensity");
                        serializedObject.FindProperty("_ShoreWaves").boolValue = true;

                        break;
                    }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}