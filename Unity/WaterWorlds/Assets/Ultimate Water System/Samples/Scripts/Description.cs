namespace UltimateWater.Utils
{
    using UnityEngine;

    [AddComponentMenu("Ultimate Water/Utils/Description")]
    public class Description : MonoBehaviour
    {
        #region Public Variables
        [Multiline]
        public string Text;
        #endregion Public Variables

        #region Static Variables
        public static bool Editable;
        #endregion Static Variables

        #region Private Methods
        [ContextMenu("Toggle Edit")]
        private void ToggleEdit()
        {
            Editable = !Editable;
        }
        #endregion Private Methods
    }
}