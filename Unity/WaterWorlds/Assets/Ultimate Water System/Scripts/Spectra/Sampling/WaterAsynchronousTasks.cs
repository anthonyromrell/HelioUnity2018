namespace UltimateWater.Internal
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using UnityEngine;

    public sealed class WaterAsynchronousTasks : MonoBehaviour
    {
        #region Public Variables
        public static WaterAsynchronousTasks Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = FindObjectOfType<WaterAsynchronousTasks>();

                    if (_Instance == null)
                    {
                        var go = new GameObject("Ultimate Water Spectrum Sampler") { hideFlags = HideFlags.HideInHierarchy };
                        _Instance = go.AddComponent<WaterAsynchronousTasks>();
                    }
                }

                return _Instance;
            }
        }

        public static bool HasInstance
        {
            get { return _Instance != null; }
        }
        #endregion Public Variables

        #region Public Methods
        public void AddWaterSampleComputations(WaterSample computation)
        {
            lock (_Computations)
            {
                _Computations.Add(computation);
            }
        }

        public void RemoveWaterSampleComputations(WaterSample computation)
        {
            lock (_Computations)
            {
                int index = _Computations.IndexOf(computation);

                if (index == -1) return;

                if (index < _ComputationIndex)
                    --_ComputationIndex;

                _Computations.RemoveAt(index);
            }
        }

        public void AddFFTComputations(WaterTileSpectrum scale)
        {
            lock (_FFTSpectra)
            {
                _FFTSpectra.Add(scale);
            }
        }

        public void RemoveFFTComputations(WaterTileSpectrum scale)
        {
            lock (_FFTSpectra)
            {
                int index = _FFTSpectra.IndexOf(scale);

                if (index == -1) return;

                if (index < _FFTSpectrumIndex)
                    --_FFTSpectrumIndex;

                _FFTSpectra.RemoveAt(index);
            }
        }
        #endregion Public Methods

        #region Unity Methods
        private void Awake()
        {
            _Run = true;

            if (_Instance == null)
            {
                _Instance = this;
            }
            else if (_Instance != this)
            {
                gameObject.Destroy();
                return;
            }

            if (!Application.isPlaying)
                return;

            DontDestroyOnLoad(gameObject);

            for (int i = 0; i < WaterProjectSettings.Instance.PhysicsThreads; ++i)
            {
                Thread thread = new Thread(RunSamplingTask) { Priority = WaterProjectSettings.Instance.PhysicsThreadsPriority };
                thread.Start();
            }

            {
                Thread thread = new Thread(RunFFTTask) { Priority = WaterProjectSettings.Instance.PhysicsThreadsPriority };
                thread.Start();
            }
        }

        private void OnDisable()
        {
            _Run = false;

            if (_ThreadException != null)
                UnityEngine.Debug.LogException(_ThreadException);
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (_ThreadException != null)
            {
                UnityEngine.Debug.LogException(_ThreadException);
                _ThreadException = null;
            }
        }
#endif
        #endregion Unity Methods

        #region Private Variables
        private static WaterAsynchronousTasks _Instance;

        private bool _Run;

        private readonly List<WaterTileSpectrum> _FFTSpectra = new List<WaterTileSpectrum>();
        private int _FFTSpectrumIndex;
        private float _FFTTimeStep = 0.2f;

        private readonly List<WaterSample> _Computations = new List<WaterSample>();
        private int _ComputationIndex;

        private System.Exception _ThreadException;
        #endregion Private Variables

        #region Private Methods
        private void RunSamplingTask()
        {
            try
            {
                while (_Run)
                {
                    WaterSample computation = null;

                    lock (_Computations)
                    {
                        if (_Computations.Count != 0)
                        {
                            if (_ComputationIndex >= _Computations.Count)
                                _ComputationIndex = 0;

                            computation = _Computations[_ComputationIndex++];
                        }
                    }

                    if (computation == null)
                    {
                        Thread.Sleep(2);
                        continue;
                    }

                    lock (computation)
                    {
                        computation.ComputationStep();
                    }
                }
            }
            catch (System.Exception e)
            {
                _ThreadException = e;
            }
        }

        private void RunFFTTask()
        {
            try
            {
                var fftTask = new CpuFFT();
                Stopwatch stopwatch = new Stopwatch();
                bool performanceProblems = false;

                while (_Run)
                {
                    WaterTileSpectrum spectrum = null;

                    lock (_FFTSpectra)
                    {
                        if (_FFTSpectra.Count != 0)
                        {
                            if (_FFTSpectrumIndex >= _FFTSpectra.Count)
                                _FFTSpectrumIndex = 0;

                            if (_FFTSpectrumIndex == 0)
                            {
                                if (stopwatch.ElapsedMilliseconds > _FFTTimeStep * 900.0f)
                                {
                                    if (performanceProblems)
                                        _FFTTimeStep += 0.05f;
                                    else
                                        performanceProblems = true;
                                }
                                else
                                {
                                    performanceProblems = false;

                                    if (_FFTTimeStep > 0.2f)
                                        _FFTTimeStep -= 0.001f;
                                }

                                stopwatch.Reset();
                                stopwatch.Start();
                            }

                            spectrum = _FFTSpectra[_FFTSpectrumIndex++];
                        }
                    }

                    if (spectrum == null)
                    {
                        stopwatch.Reset();
                        Thread.Sleep(6);
                        continue;
                    }

                    bool didWork = false;

                    //lock (spectrum)
                    {
                        var spectrumResolver = spectrum.WindWaves.SpectrumResolver;

                        if (spectrumResolver == null)
                            continue;

                        int recentResultIndex = spectrum.RecentResultIndex;
                        int slotIndexPlus2 = (recentResultIndex + 2) % spectrum.ResultsTiming.Length;
                        int slotIndexPlus1 = (recentResultIndex + 1) % spectrum.ResultsTiming.Length;
                        float recentSlotTime = spectrum.ResultsTiming[recentResultIndex];
                        float slotPlus2Time = spectrum.ResultsTiming[slotIndexPlus2];
                        float slotPlus1Time = spectrum.ResultsTiming[slotIndexPlus1];
                        float currentTime = spectrumResolver.LastFrameTime;

                        if (slotPlus2Time <= currentTime || slotPlus1Time > currentTime)
                        {
                            float loopDuration = spectrum.WindWaves.LoopDuration;
                            float computedSnapshotTime;

                            if (loopDuration != 0.0f)
                                computedSnapshotTime = Mathf.Round((recentSlotTime % loopDuration + 0.2f) / 0.2f) * 0.2f;
                            else if (slotPlus1Time > currentTime)
                                computedSnapshotTime = currentTime + _FFTTimeStep;
                            else
                                computedSnapshotTime = Mathf.Max(recentSlotTime, currentTime) + _FFTTimeStep;

                            if (computedSnapshotTime != slotPlus1Time)
                            {
                                fftTask.Compute(spectrum, computedSnapshotTime, slotIndexPlus1);
                                spectrum.ResultsTiming[slotIndexPlus1] = computedSnapshotTime;

                                didWork = true;
                            }

                            spectrum.RecentResultIndex = slotIndexPlus1;
                        }
                    }

                    if (!didWork)
                    {
                        stopwatch.Reset();
                        Thread.Sleep(3);
                    }
                }
            }
            catch (System.Exception e)
            {
                _ThreadException = e;
            }
        }
        #endregion Private Methods
    }
}