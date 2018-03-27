namespace UltimateWater
{
    using UnityEngine;
    using System.Collections.Generic;

    /// <summary>
    /// Allows for adding custom wave forces
    /// independently of the depth-based generation
    /// </summary>
    [AddComponentMenu("Ultimate Water/Dynamic/Water Force")]
    public sealed class WaterForce : MonoBehaviour
    {
        #region Public Types
        public struct Data
        {
            public Vector3 Position;
            public float Force;
        }
        #endregion Public Types

        #region Public Variables
        [Tooltip("Force affecting water surface")]
        public float Force = 0.01f;

        [Tooltip("Area of water displacement")]
        public float Radius = 1.0f;
        #endregion Public Variables

        #region Private Variables
        private static readonly List<Data> _ForceData = new List<Data>(1);
        #endregion Private Variables

        #region Unity Messages
        private void FixedUpdate()
        {
            Data data;
            data.Position = transform.position;
            data.Force = Force * Time.fixedDeltaTime;

            _ForceData.Clear();
            _ForceData.Add(data);

            WaterRipples.AddForce(_ForceData, Radius);
        }
        private void OnDrawGizmos()
        {
            // draw gizmo's at the position, where the force will be added
            Gizmos.color = Color.green * 0.8f + Color.gray * 0.2f;
            Gizmos.DrawLine(transform.position + Vector3.up * 2.0f, transform.position - Vector3.up * 2.0f);
            Gizmos.DrawLine(transform.position + (Vector3.forward + Vector3.left), transform.position - (Vector3.forward + Vector3.left));
            Gizmos.DrawLine(transform.position + (Vector3.forward - Vector3.left), transform.position - (Vector3.forward - Vector3.left));
        }
        #endregion Unity Messages
    }
}