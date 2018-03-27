namespace UltimateWater.Editors
{
    using UnityEngine;
    using UnityEditor;

    [CustomPropertyDrawer(typeof(ResolutionAttribute))]
    public class ResolutionPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attribute = (ResolutionAttribute)this.attribute;
            int selectedValue = property.intValue;

            string[] labels = new string[attribute.Resolutions.Length];
            int selectedIndex = -1;

            for (int i = 0; i < labels.Length; ++i)
            {
                int resolution = attribute.Resolutions[i];
                labels[i] = resolution.ToString();

                if (resolution == selectedValue)
                    selectedIndex = i;

                if (resolution == attribute.RecommendedResolution)
                    labels[i] += " (Default)";
            }

            int newSelectedIndex = EditorGUI.Popup(position, property.displayName, selectedIndex, labels);

            if (selectedIndex != newSelectedIndex)
                property.intValue = attribute.Resolutions[newSelectedIndex];
        }
    }
}