namespace UltimateWater.Editors
{
    using System.Linq;
    using System.Reflection;
    using Utils;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(InspectorWarningAttribute))]
    public class InspectorWarningDrawer : PropertyDrawer
    {
        #region Private Variables
        private MethodInfo _Info;
        private string _Warning;

        #endregion Private Variables

        #region Unity Messages
        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            AssignReferences(prop);
            if (string.IsNullOrEmpty(_Warning))
            {
                return;
            }

            position.y += 20.0f;
            position.height -= 20.0f;

            var attr = attribute as InspectorWarningAttribute;
            if (attr == null) return;
            InspectorWarningUtility.WarningField(position, _Warning, attr.Type);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            AssignReferences(property);
            return string.IsNullOrEmpty(_Warning) ? 0.0f : (42.0f + _Warning.Count(x => x == '\n') * 14.0f);
        }
        #endregion Unity Messages

        #region Helper Methods
        private void AssignReferences(SerializedProperty prop)
        {
            _Warning = null;

            // get method data
            if (_Info == null)
            {
                _Info = GetType(prop).GetMethod(GetName(), BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            }

            // either call warning check of fail
            if (_Info != null)
            {
                _Warning = _Info.Invoke(prop.serializedObject.targetObject, null) as string;
            }
            else
            {
                Debug.LogWarning(string.Format("InspectorWarning: Unable to find method {0} in {1}", GetName(), GetType(prop)));
            }
        }
        private string GetName()
        {
            return ((InspectorWarningAttribute)attribute).MethodName;
        }
        private static System.Type GetType(SerializedProperty prop)
        {
            return prop.serializedObject.targetObject.GetType();
        }
        #endregion Helper Methods
    }
}