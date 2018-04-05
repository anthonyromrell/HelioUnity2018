namespace UltimateWater
{
    using UnityEngine;
    using UnityEngine.Serialization;
    using Internal;

    /// <summary>
    /// Describes some external weather system (like a storm) that may travel around the current location or even cross it.
    /// - Forward vector of the transform is the weather system wind direction.
    /// - Position of the transform is the weather system position.
    /// </summary>
    public class WeatherSystem : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField, FormerlySerializedAs("water")] private Water _Water;
        [SerializeField, FormerlySerializedAs("profile")] private WaterProfile _Profile;

        [Tooltip("Describes how big the weather system is. Common values range from 10000 to 150000, assuming that the scene units are used as meters.")]
        [SerializeField, FormerlySerializedAs("radius")]
        private float _Radius = 10000;

        [Range(0.0f, 1.0f)]
        [SerializeField, FormerlySerializedAs("weight")]
        private float _Weight = 1.0f;
        #endregion Inspector Variables

        #region Unity Methods
        private void Start()
        {
            _SpectrumData = new WaterWavesSpectrumData(_Water, _Water.WindWaves, _Profile.Data.Spectrum);
            LateUpdate();
            _Water.WindWaves.SpectrumResolver.AddSpectrum(_SpectrumData);
        }

        private void OnEnable()
        {
            if (_SpectrumData != null && !_Water.WindWaves.SpectrumResolver.ContainsSpectrum(_SpectrumData))
                _Water.WindWaves.SpectrumResolver.AddSpectrum(_SpectrumData);
        }

        private void OnDisable()
        {
            _Water.WindWaves.SpectrumResolver.RemoveSpectrum(_SpectrumData);
        }

        private void LateUpdate()
        {
            Vector3 offset3D = _Water.transform.InverseTransformPoint(transform.position);
            Vector2 offset = new Vector2(offset3D.x, offset3D.z);

            Vector3 windDirection3D = transform.forward;
            Vector2 windDirection = new Vector2(windDirection3D.x, windDirection3D.z).normalized;

            if (windDirection != _LastWindDirection || offset != _LastOffset || _Radius != _LastRadius || _Weight != _LastWeight)
            {
                _SpectrumData.WindDirection = _LastWindDirection = windDirection;
                _SpectrumData.WeatherSystemOffset = _LastOffset = offset;
                _SpectrumData.WeatherSystemRadius = _LastRadius = _Radius;
                _SpectrumData.Weight = _LastWeight = _Weight;
                _Water.WindWaves.SpectrumResolver.SetDirectionalSpectrumDirty();
            }
        }
        #endregion Unity Methods

        #region Private Variables
        private WaterWavesSpectrumData _SpectrumData;
        private Vector2 _LastOffset;
        private Vector2 _LastWindDirection;
        private float _LastRadius;
        private float _LastWeight;
        #endregion Private Variables
    }
}