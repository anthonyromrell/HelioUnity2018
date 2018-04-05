namespace UltimateWater
{
    using UnityEngine;
    using Internal;

    /// <summary>
    ///     Computes water data at a given point.
    /// </summary>
    public sealed class WaterSample
    {
        #region Public Variables
        public bool Finished
        {
            get { return _Finished; }
        }

        public Vector2 Position
        {
            get { return new Vector2(_X, _Z); }
        }
        #endregion Public Variables

        #region Public Methods
        public WaterSample(Water water, DisplacementMode displacementMode = DisplacementMode.Height, float precision = 1.0f)
        {
            if (water == null)
            {
                throw new System.ArgumentException("Argument 'water' is null.");
            }

            if (precision <= 0.0f || precision > 1.0f) throw new System.ArgumentException("Precision has to be between 0.0 and 1.0.");

            _Water = water;
            _DisplacementMode = displacementMode;
            _PreviousResult.x = float.NaN;
        }

        /// <summary>
        /// Starts water height computations.
        /// </summary>
        /// <param name="origin"></param>
        public void Start(Vector3 origin)
        {
            _Finished = true;
            _PreviousResult = _Displaced = origin;
            _PreviousForces = _Forces = new Vector3();
            GetAndReset(origin.x, origin.z);
        }

        /// <summary>
        /// Starts water height computations.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        public void Start(float x, float z)
        {
            _Finished = true;
            _PreviousResult = _Displaced = new Vector3(x, _Water.transform.position.y, z);
            _PreviousForces = _Forces = new Vector3();
            GetAndReset(x, z);
        }

        /// <summary>
        /// Retrieves recently computed displacement and restarts computations on a new position.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public Vector3 GetAndReset(Vector3 origin, ComputationsMode mode = ComputationsMode.Normal)
        {
            return GetAndReset(origin.x, origin.z, mode);
        }

        /// <summary>
        /// Retrieves recently computed displacement and restarts computations on a new position.
        /// </summary>
        /// <param name="x">World space coordinate.</param>
        /// <param name="z">World space coordinate.</param>
        /// <param name="mode">Determines if the computations should be completed on the current thread if necessary. May hurt performance, but setting it to false may cause some 'flickering'.</param>
        /// <returns></returns>
        public Vector3 GetAndReset(float x, float z, ComputationsMode mode = ComputationsMode.Normal)
        {
            Vector3 forces;
            return GetAndReset(x, z, mode, out forces);
        }

        /// <summary>
        /// Retrieves recently computed displacement and restarts computations on a new position.
        /// </summary>
        /// <param name="x">World space coordinate.</param>
        /// <param name="z">World space coordinate.</param>
        /// <param name="mode">Determines if the computations should be completed on the current thread if necessary. May hurt performance, but setting it to false may cause some 'flickering'.</param>
        /// <param name="forces">Wave force vector.</param>
        /// <returns></returns>
        public Vector3 GetAndReset(float x, float z, ComputationsMode mode, out Vector3 forces)
        {
            switch (mode)
            {
                case ComputationsMode.ForceCompletion:
                {
                    if (!_Finished)
                    {
                        _Finished = true;
                        ComputationStep(true);
                    }

                    break;
                }

                case ComputationsMode.Normal:
                {
                    if (!_Finished && !float.IsNaN(_PreviousResult.x))
                    {
                        forces = _PreviousForces;
                        return _PreviousResult;
                    }

                    _PreviousResult = _Displaced;
                    _PreviousForces = _Forces;

                    break;
                }
            }

            _Finished = true;

            if (!_Enqueued)
            {
                WaterAsynchronousTasks.Instance.AddWaterSampleComputations(this);
                _Enqueued = true;

                _Water.OnSamplingStarted();
            }

            var result = _Displaced;
            result.y += _Water.transform.position.y;
            forces = _Forces;

            _X = x;
            _Z = z;
            _Displaced.x = x;
            _Displaced.y = 0.0f;
            _Displaced.z = z;
            _Forces.x = 0.0f;
            _Forces.y = 0.0f;
            _Forces.z = 0.0f;
            _Time = _Water.Time;
            _Finished = false;

            return result;
        }

        /// <summary>
        /// Faster version of GetAndReset. Assumes HeightAndForces displacement mode and that computations were started earlier with a Start call.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="time"></param>
        /// <param name="result"></param>
        /// <param name="forces"></param>
        public void GetAndResetFast(float x, float z, float time, out Vector3 result, out Vector3 forces)
        {
            if (!_Finished)
            {
                forces = _PreviousForces;
                result = _PreviousResult;
                return;
            }

            _PreviousResult = _Displaced;
            _PreviousForces = _Forces;

            result = _Displaced;
            result.y += _Water.transform.position.y;
            forces = _Forces;

            _X = x;
            _Z = z;
            _Displaced.x = x;
            _Displaced.y = 0.0f;
            _Displaced.z = z;
            _Forces.x = 0.0f;
            _Forces.y = 0.0f;
            _Forces.z = 0.0f;
            _Time = time;
            _Finished = false;
        }

        /// <summary>
        /// Faster version of GetAndReset. Assumes HeightAndForces displacement mode and that computations were started earlier with a Start call.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="time"></param>
        /// <param name="result"></param>
        /// <param name="forces"></param>
        public void GetAndResetFast(float x, float z, float time, out float result, out Vector3 forces)
        {
            if (!_Finished)
            {
                forces = _PreviousForces;
                result = _PreviousResult.y;
                return;
            }

            _PreviousResult = _Displaced;
            _PreviousForces = _Forces;

            result = _Displaced.y + _Water.transform.position.y;
            forces = _Forces;

            _X = x;
            _Z = z;
            _Displaced.x = x;
            _Displaced.y = 0.0f;
            _Displaced.z = z;
            _Forces.x = 0.0f;
            _Forces.y = 0.0f;
            _Forces.z = 0.0f;
            _Time = time;
            _Finished = false;
        }

        public Vector3 Stop()
        {
            if (_Enqueued)
            {
                if (WaterAsynchronousTasks.HasInstance)
                    WaterAsynchronousTasks.Instance.RemoveWaterSampleComputations(this);

                _Enqueued = false;

                if (_Water != null)
                    _Water.OnSamplingStopped();
            }

            return _Displaced;
        }
        #endregion Public Methods

        #region Private Variables
        private readonly Water _Water;
        private float _X;
        private float _Z;
        private float _Time;

        private Vector3 _Displaced;
        private Vector3 _PreviousResult;
        private Vector3 _Forces;
        private Vector3 _PreviousForces;

        private bool _Finished;
        private bool _Enqueued;

        private readonly DisplacementMode _DisplacementMode;
        #endregion Private Variables

        #region Private Methods
        internal void ComputationStep(bool ignoreFinishedFlag = false)
        {
            if (!_Finished || ignoreFinishedFlag)
            {
                if (_DisplacementMode == DisplacementMode.Height || _DisplacementMode == DisplacementMode.HeightAndForces)
                {
                    _Water.CompensateHorizontalDisplacement(ref _X, ref _Z, _Time);

                    if (_DisplacementMode == DisplacementMode.Height)
                    {
                        // compute height at resultant point
                        float result = _Water.GetUncompensatedHeightAt(_X, _Z, _Time);
                        _Displaced.y += result;
                    }
                    else
                    {
                        Vector4 result = _Water.GetUncompensatedHeightAndForcesAt(_X, _Z, _Time);

                        _Displaced.y += result.w;
                        _Forces.x += result.x;
                        _Forces.y += result.y;
                        _Forces.z += result.z;
                    }
                }
                else
                {
                    // make computations only on enabled water
                    Vector3 result = _Water.WaterId != -1 ? _Water.GetUncompensatedDisplacementAt(_X, _Z, _Time) : new Vector3();
                    _Displaced += result;
                }

                _Finished = true;
            }
        }
        #endregion Private Methods

        #region Public Types
        public enum DisplacementMode
        {
            Height,
            Displacement,
            HeightAndForces
        }

        public enum ComputationsMode
        {
            Normal = 0,
            ForceCompletion = 2
        }
        #endregion Public Types
    }
}