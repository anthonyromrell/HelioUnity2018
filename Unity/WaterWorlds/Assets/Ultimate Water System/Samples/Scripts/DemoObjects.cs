// [YOU SHALL NOT!]
// use this in production code;
// all the code in Samples namespace can and will be edited/deleted in later versions

namespace UltimateWater.Samples
{
    using UnityEngine;
    using System.Collections.Generic;

    public class DemoObjects : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] private float _Radius = 1.0f;
        [SerializeField] private float _Speed = 1.0f;
        [SerializeField] private Vector3 _Center;
        [SerializeField] private Type _Type;
        [SerializeField] private float _Force;
        [SerializeField] private float _DisableTime = 1024.0f;
        #endregion Inspector Variables

        #region Unity Messages
        private void Awake()
        {
            _Center = transform.position;
            Invoke("Disable", _DisableTime);
        }
        private void FixedUpdate()
        {
            switch (_Type)
            {
                case Type.Boat: Boat(); break;
                case Type.FishingRod: FishingRod(); break;
                case Type.UserInput: UserInput(); break;
                case Type.Generator: Generator(); break;
            }
        }
        #endregion Unity Messages

        #region Private Types
        private enum Type
        {
            FishingRod,
            Boat,
            UserInput,
            Generator
        }
        #endregion Private Types

        #region Private Variables
        private float _Angle;
        #endregion Private Variables

        #region Private Methods
        private void Disable()
        {
            enabled = false;
        }
        private void FishingRod()
        {
            _Angle += _Speed * Time.deltaTime;
            transform.position = _Center + _Radius * new Vector3(Mathf.Cos(_Angle), 0.0f, Mathf.Sin(_Angle));
            transform.eulerAngles = new Vector3(0.0f, -_Angle * Mathf.Rad2Deg, 0.0f);
        }
        private void Boat()
        {
            transform.position += transform.forward * _Speed * Time.deltaTime;
        }
        private void UserInput()
        {
            float x = Input.GetAxis("Horizontal") * _Speed * Time.deltaTime;
            float y = Input.GetAxis("Vertical") * _Speed * Time.deltaTime;

            transform.position += transform.forward * y;
            transform.position += transform.right * x;

            if (Input.GetKey(KeyCode.Q))
            {
                transform.position -= transform.up * _Speed * Time.deltaTime * 0.33f;
            }
            if (Input.GetKey(KeyCode.E))
            {
                transform.position += transform.up * _Speed * Time.deltaTime * 0.33f;
            }
        }
        private void Generator()
        {
            var data = new List<WaterForce.Data>(){
                new WaterForce.Data() {
                    Position = transform.position,
                    Force = _Force * Mathf.Cos(Time.timeSinceLevelLoad * _Speed)
                }
            };

            WaterRipples.AddForce(data, _Radius);
        }
        #endregion Private Methods
    }
}