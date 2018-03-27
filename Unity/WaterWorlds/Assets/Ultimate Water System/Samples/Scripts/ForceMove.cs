namespace UltimateWater.Samples
{
    using UnityEngine;

    public class ForceMove : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] private Rigidbody _Rigidbody;
        [SerializeField] private float _Force;
        #endregion Inspector Variables

        #region Unity Methods
        private void FixedUpdate()
        {
            _Rigidbody.AddForce(transform.forward * _Force);
        }
        #endregion Unity Methods
    }
}