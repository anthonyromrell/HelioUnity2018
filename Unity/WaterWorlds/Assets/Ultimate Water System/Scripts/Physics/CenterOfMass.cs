namespace UltimateWater
{
    using UnityEngine;

    /// <summary>
    /// Forces center of mass of a parent rigid body to be positioned at this transform position.
    /// </summary>
    public sealed class CenterOfMass : MonoBehaviour
    {
        #region Public Methods
        public void Apply()
        {
            var rigidBody = GetComponentInParent<Rigidbody>();
            if (rigidBody != null)
            {
                rigidBody.centerOfMass = rigidBody.transform.worldToLocalMatrix.MultiplyPoint3x4(transform.position);
            }
        }
        #endregion Public Methods

        #region Unity Messages
        private void OnEnable()
        {
            Apply();
        }
        #endregion Unity Messages

        #region Editor Methods
#if UNITY_EDITOR
        [UnityEditor.MenuItem("CONTEXT/Rigidbody/Materialize Center of Mass")]
        public static void MaterializeCenterOfMass()
        {
            var rigidBody = UnityEditor.Selection.activeTransform.GetComponent<Rigidbody>();

            var comGo = new GameObject("Center of Mass");
            comGo.transform.SetParent(rigidBody.transform);
            comGo.transform.position = rigidBody.worldCenterOfMass;
            comGo.transform.localRotation = Quaternion.identity;
            comGo.transform.localScale = Vector3.one;

            comGo.AddComponent<CenterOfMass>();
        }
#endif
        #endregion Editor Methods
    }
}