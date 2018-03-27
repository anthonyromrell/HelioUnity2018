namespace UltimateWater.Internal
{
    using System.Text.RegularExpressions;
    using UnityEngine;

#if UNITY_EDITOR

    using UnityEditor;

#endif

    /// <summary>
    /// Checks the Unity version, and informs user
    /// when due to difficult-to-solve Unity bugs, Ultimate Water is not supported
    /// </summary>
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class VersionCompatibility
    {
        #region Public Variables
        /// <summary>
        /// returns an int representing current unity version
        /// takes into consideration only major, minor and update numbers
        ///
        /// 5.4.5f1    => 545
        /// 5.6.1p2    => 561
        /// 2017.1.3f1 => 201713
        /// </summary>
        public static int Version
        {
            get
            {
                // if version was not calculate, calculate and cache
                if (_Version == -1)
                {
                    _Version = CalculateVersion();
                }

                return _Version;
            }
        }
        #endregion Public Variables

        #region Public Methods
        static VersionCompatibility()
        {
            CalculateVersion();
            CheckVersion();
        }

        #endregion Public Methods

        #region Private Variables
        private static int _Version = -1;

        private static readonly string[] _Unsupported = new string[]
        {
            "5.0.0", "5.0.1", "5.0.2", "5.0.3", "5.0.4",
            "5.1.0", "5.1.1", "5.1.2", "5.1.3", "5.1.4", "5.1.5",
            "5.2.0", "5.2.1", "5.2.2", "5.2.3", "5.2.4", "5.2.5",
            "5.3.0", "5.3.1", "5.3.2", "5.3.3", "5.3.4", "5.3.5",
            "5.4.0", "5.4.1", "5.4.2", "5.4.4"
        };

        private static readonly string[] _Bugged = new string[]
        {
            "5.6.0",
            "5.6.1"
        };
        private static readonly string[] _BugInfo = new string[]
        {
            "This Unity version introduces bugs in depth computations, please use earlier (5.5.x-) or later (5.6.2+) versions",
            "This Unity version introduces bugs in depth computations, please use earlier (5.5.x-) or later (5.6.2+) versions"
        };
        #endregion Private Variables

        #region Private Methods
        private static int CalculateVersion()
        {
            string unity = Application.unityVersion;

            // replace all patch identifiers "f, p, rc, ...", and subsequent patch version numbers
            unity = Regex.Replace(unity, "[^0-9.]+[0-9]*", string.Empty);

            // split into major/minor/update
            var version = unity.Split('.');

            int major = int.Parse(version[0]);
            int minor = int.Parse(version[1]);
            int update = int.Parse(version[2]);

            // calculate version integer
            return major * 100 + minor * 10 + update;
        }

        private static void CheckVersion()
        {
#if UNITY_EDITOR
            var version = Application.unityVersion;
            if (!IsSupported(version))
            {
                EditorUtility.DisplayDialog("Ultimate Water",
       "This Unity version (" + version + ") is not supported, the first supported Unity version: 5.4.5f1.", "Ok");
                return;
            }

            if (IsBuggy(version))
            {
                EditorUtility.DisplayDialog("Ultimate Water", BugInfo(version), "Ok");
            }
#endif
        }

        private static bool IsSupported(string version)
        {
            foreach (var entry in _Unsupported)
            {
                if (entry.Contains(version))
                {
                    return false;
                }
            }
            return true;
        }
        private static bool IsBuggy(string version)
        {
            foreach (var entry in _Bugged)
            {
                if (entry.Contains(version))
                {
                    return true;
                }
            }
            return false;
        }
        private static string BugInfo(string version)
        {
            for (int i = 0; i < _Bugged.Length; ++i)
            {
                if (_Bugged[i].Contains(version))
                {
                    return _BugInfo[i];
                }
            }

            return string.Empty;
        }
        #endregion Private Methods
    }
}