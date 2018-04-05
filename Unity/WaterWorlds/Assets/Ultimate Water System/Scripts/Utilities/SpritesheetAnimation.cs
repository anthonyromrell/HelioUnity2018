namespace UltimateWater
{
    using UnityEngine;
    using UnityEngine.Serialization;

    /// <summary>
    /// Simple spritesheet texture animator. Used for small billboard splashes.
    /// </summary>
    public sealed class SpritesheetAnimation : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField, FormerlySerializedAs("horizontal")]
        private int _Horizontal = 2;

        [SerializeField, FormerlySerializedAs("vertical")]
        private int _Vertical = 2;

        [SerializeField, FormerlySerializedAs("timeStep")]
        private float _TimeStep = 0.06f;

        [SerializeField, FormerlySerializedAs("loop")]
        private bool _Loop;

        [SerializeField, FormerlySerializedAs("destroyGo")]
        private bool _DestroyGo;
        #endregion Inspector Variables

        #region Unity Methods
        private void Start()
        {
            var rendererComponent = GetComponent<Renderer>();
            _Material = rendererComponent.material;
            _Material.mainTextureScale = new Vector2(1.0f / _Horizontal, 1.0f / _Vertical);
            _Material.mainTextureOffset = new Vector2(0.0f, 0.0f);

            _NextChangeTime = Time.time + _TimeStep;
        }

        private void Update()
        {
            if (Time.time >= _NextChangeTime)
            {
                _NextChangeTime += _TimeStep;

                if (_X == _Horizontal - 1 && _Y == _Vertical - 1)
                {
                    if (_Loop)
                    {
                        _X = 0;
                        _Y = 0;
                    }
                    else
                    {
                        if (_DestroyGo)
                            Destroy(gameObject);
                        else
                            enabled = false;

                        return;
                    }
                }
                else
                {
                    ++_X;

                    if (_X >= _Horizontal)
                    {
                        _X = 0;
                        ++_Y;
                    }
                }

                _Material.mainTextureOffset = new Vector2(_X / (float)_Horizontal, 1.0f - (_Y + 1) / (float)_Vertical);
            }
        }

        private void OnDestroy()
        {
            if (_Material != null)
            {
                Destroy(_Material);
                _Material = null;
            }
        }
        #endregion Unity Methods

        #region Private Variable
        private Material _Material;

        private float _NextChangeTime;
        private int _X, _Y;
        #endregion Private Variable
    }
}