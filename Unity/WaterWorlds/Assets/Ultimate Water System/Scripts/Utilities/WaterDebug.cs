using UnityEngine;

namespace UltimateWater
{
    public static class WaterDebug
    {
        public static void WriteAllMaps(Water water)
        {
#if DEBUG && WATER_DEBUG
			var windWaves = water.GetComponent<WindWaves>();
            var wavesFFT = windWaves.WaterWavesFFT;
			SaveTexture(wavesFFT.GetDisplacementMap(0), "UltimateWater - FFT Height Map 0.png");
			SaveTexture(wavesFFT.GetNormalMap(0), "UltimateWater - FFT Normal Map 0.png");
			SaveTexture(wavesFFT.GetDisplacementMap(1), "UltimateWater - FFT Height Map 1.png");
			SaveTexture(wavesFFT.GetNormalMap(1), "UltimateWater - FFT Normal Map 1.png");
			SaveTexture(wavesFFT.GetDisplacementMap(2), "UltimateWater - FFT Height Map 2.png");
			SaveTexture(wavesFFT.GetDisplacementMap(3), "UltimateWater - FFT Height Map 3.png");

			SaveTexture(windWaves.SpectrumResolver.RenderHeightSpectrumAt(Time.time), "UltimateWater - Timed Height Spectrum.png");

			SaveTexture(windWaves.SpectrumResolver.GetSpectrum(SpectrumResolver.SpectrumType.RawOmnidirectional), "UltimateWater - Spectrum Raw Omnidirectional.png");
			SaveTexture(windWaves.SpectrumResolver.GetSpectrum(SpectrumResolver.SpectrumType.RawDirectional), "UltimateWater - Spectrum Raw Directional.png");
			SaveTexture(windWaves.SpectrumResolver.GetSpectrum(SpectrumResolver.SpectrumType.Height), "UltimateWater - Spectrum Height.png");
			SaveTexture(windWaves.SpectrumResolver.GetSpectrum(SpectrumResolver.SpectrumType.Normal), "UltimateWater - Spectrum Normal.png");
			SaveTexture(windWaves.SpectrumResolver.GetSpectrum(SpectrumResolver.SpectrumType.Displacement), "UltimateWater - Spectrum Displacement.png");
#endif
        }

        public static void SaveTexture(Texture tex, string name)
        {
#if DEBUG && WATER_DEBUG
			if(tex == null)
				return;

			var shader = Shader.Find("UltimateWater/Editor/Inspect Texture");
			var material = new Material(shader);
			material.SetVector("_RangeR", new Vector2(0.0f, 0.1f));
			material.SetVector("_RangeG", new Vector2(0.0f, 0.1f));
			material.SetVector("_RangeB", new Vector2(0.0f, 0.1f));

			var tempRT = new RenderTexture(tex.width, tex.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			Graphics.Blit(tex, tempRT, material);

			RenderTexture.active = tempRT;

			var tex2d = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
			tex2d.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
			tex2d.Apply();

			RenderTexture.active = null;

			System.IO.File.WriteAllBytes(name, tex2d.EncodeToPNG());

			tex2d.Destroy();
			tempRT.Destroy();
#endif
        }
    }
}