namespace UltimateWater.Internal
{
    using UnityEngine;

    public class ApplicationSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Public Variables
        public static T Instance
        {
            get
            {
                // if the instance is assigned, return it
                if (_Instance != null) { return _Instance; }

                // try to find it on scene
                _Instance = (T)FindObjectOfType(typeof(T));
                if (_Instance != null) { return _Instance; }

                // if the application is quitting, do not atempt to create new one
                if (_Quiting) { return null; }

                // create new gameobject and add the required component
                var obj = new GameObject("[" + typeof(T).Name + "]" + " - instance") { hideFlags = HideFlags.HideInHierarchy };

                // disable object destruction
                DontDestroyOnLoad(obj);

                _Instance = obj.AddComponent<T>();
                return _Instance;
            }
        }
        #endregion Public Variables

        #region Unity Methods
        protected virtual void OnApplicationQuit()
        {
            _Quiting = true;
        }
        protected virtual void OnDestroy()
        {
            _Quiting = true;
        }
        #endregion Unity Methods

        #region Private Variables
        private static T _Instance;

        // ReSharper disable once StaticMemberInGenericType - the one static variable per generic instantiation behavior is desired here
        private static bool _Quiting;
        #endregion Private Variables
    }
}