namespace UltimateWater.Utils
{
    using UnityEngine;
    using UnityEditor;
    using Internal;

    [CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
    public class MinMaxRangeDrawer : PropertyDrawer
    {
        #region Public Methods
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + 16;
        }

        public override void OnGUI(Rect r, SerializedProperty property, GUIContent label)
        {
            var range = attribute as MinMaxRangeAttribute;
            if (range == null)
            {
                WaterLogger.Error("MinMaxRangeDrawer", "OnGUI", "Attribute is not of MinMaxRangeAttribute type");
                return;
            }

            var min = property.FindPropertyRelative("MinValue");
            var max = property.FindPropertyRelative("MaxValue");

            var left = new Rect()
            {
                xMin = r.xMin + r.width * 0.4f,
                xMax = r.xMin + r.width * 0.4f + _IndicatorSize,
                yMin = r.yMin,
                yMax = r.yMax - r.height * 0.5f
            };

            var slider = new Rect()
            {
                xMin = left.xMax + _Spacing,
                xMax = r.xMax - _IndicatorSize - _Spacing,
                yMin = r.yMin,
                yMax = r.yMax - r.height * 0.5f
            };
            var right = new Rect()
            {
                xMin = slider.xMax + _Spacing,
                xMax = r.xMax,
                yMin = slider.yMin,
                yMax = slider.yMax
            };

            var minValue = min.floatValue;
            var maxValue = max.floatValue;

            minValue = EditorGUI.FloatField(left, minValue);
            maxValue = EditorGUI.FloatField(right, maxValue);

            EditorGUI.LabelField(r, property.name);
            EditorGUI.MinMaxSlider(slider, ref minValue, ref maxValue, range.MinValue, range.MaxValue);

            min.floatValue = minValue;
            max.floatValue = maxValue;
        }
        #endregion Public Methods

        #region Private Variables
        private const int _IndicatorSize = 50;
        private const int _Spacing = 6;
        #endregion Private Variables
    }
}