using UnityEngine;
using UnityEngine.Networking;

namespace UltimateWater
{
    [AddComponentMenu("Ultimate Water/Network Synchronization", 2)]
    [RequireComponent(typeof(Water))]
    public class NetworkWater : NetworkBehaviour
    {
        #region Private Variables
        [SyncVar]
        private float _Time;
        private Water _Water;
        #endregion Private Variables

        #region Unity Messages
        private void Awake()
        {
            _Water = GetComponent<Water>();
            if (_Water == null)
            {
                enabled = false;
                Debug.LogWarning("[Water] component not assigned to [Network Water]");
            }
        }

        private void Update()
        {
            if (isServer)
            {
                _Time = Time.time;
            }
            else
            {
                _Time += Time.deltaTime;
            }

            _Water.Time = _Time;
        }
        #endregion Unity Messages
    }
}