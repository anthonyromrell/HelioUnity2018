using UltimateWater.Internal;
using UnityEngine;

namespace UltimateWater
{
    using System;

    /// <summary>
    /// Event-hub for all global water-related messages,
    /// water-specific messages can be listened to via Water.Add/RemoveListiner
    /// </summary>
    public static class WaterEvents
    {
        #region Public Types
        public enum GlobalEventType
        {
            OnQualityChanged,
        }
        #endregion Public Types

        #region Public Methods
        public static void AddListener(Action action, GlobalEventType type)
        {
            switch (type)
            {
                case GlobalEventType.OnQualityChanged:
                    {
                        var instance = WaterQualitySettings.Instance;
                        if (instance == null) { return; }

                        WaterQualitySettings.Instance.Changed -= action;
                        WaterQualitySettings.Instance.Changed += action;
                        break;
                    }
            }
        }

        public static void RemoveListener(Action action, GlobalEventType type)
        {
            switch (type)
            {
                case GlobalEventType.OnQualityChanged:
                    {
                        var instance = WaterQualitySettings.Instance;
                        if (instance == null) { return; }

                        WaterQualitySettings.Instance.Changed -= action;
                        break;
                    }
            }
        }
        #endregion Public Methods
    }
}