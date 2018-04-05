namespace UltimateWater.Editors
{
    using System;
    using UnityEditor;
    using UnityEngine;

    public static class ListDrawer
    {
        #region Public Types
        [Flags]
        public enum EditorListOption
        {
            None = 0,

            ListSize = 1,
            ListLabel = 2,

            ElementLabels = 4,

            ButtonPlus = 8,
            ButtonMinus = 16,
            ButtonMove = 32,

            Border = 64,

            Buttons = ButtonPlus | ButtonMinus | ButtonMove,

            Default = ElementLabels | Border | ButtonPlus | ButtonMinus,
            NoElementLabels = ListLabel,
            All = Default | Buttons
        }
        #endregion Public Types

        #region Public Methods
        public static void Show(SerializedProperty array, EditorListOption options = EditorListOption.Default)
        {
            if (!array.isArray)
            {
                EditorGUILayout.HelpBox(array.name + " is neither an array nor a array!", MessageType.Error);
                return;
            }

            bool showBorder = (options & EditorListOption.Border) != 0;
            if (showBorder)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            }

            {
                DrawHeader(array, options);
                if (array.isExpanded)
                {
                    DrawElements(array, options);
                    DrawFooter(array, options);
                }
            }

            if (showBorder)
            {
                EditorGUILayout.EndVertical();
            }
        }
        #endregion Public Methods

        #region Private Variables
        private static readonly GUILayoutOption _MiniButtonWidth = GUILayout.Width(40f);

        private static readonly GUIContent _MoveButtonContent = new GUIContent("\u21b4", "move down");
        private static readonly GUIContent _DuplicateButtonContent = new GUIContent("+", "duplicate");
        private static readonly GUIContent _DeleteButtonContent = new GUIContent("-", "delete");
        private static readonly GUIContent _FoldoutButtonContent = new GUIContent("⇒", "foldout");
        private static readonly GUIContent _AddButtonContent = new GUIContent("+", "add element");

        private static int _DeleteIndex = -1;
        #endregion Private Variables

        #region Private Methods
        private static void DrawHeader(SerializedProperty array, EditorListOption options)
        {
            bool showListSize = (options & EditorListOption.ListSize) != 0;

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(_FoldoutButtonContent, EditorStyles.miniButton, _MiniButtonWidth))
                {
                    array.isExpanded = !array.isExpanded;
                }

                if (showListSize) { EditorGUILayout.PropertyField(array.FindPropertyRelative("Array.size")); }
                EditorGUILayout.LabelField(array.displayName);
            }
            EditorGUILayout.EndHorizontal();

            if (array.isExpanded)
            {
                EditorGUILayout.Space();
            }
        }
        private static void DrawFooter(SerializedProperty array, EditorListOption options)
        {
            bool showButtons = (options & EditorListOption.Buttons) != 0;
            if (!showButtons)
            {
                return;
            }

            if (GUILayout.Button(_AddButtonContent, EditorStyles.miniButton))
            {
                array.arraySize += 1;
            }
        }

        private static void DrawElements(SerializedProperty array, EditorListOption options)
        {
            bool showListLabel = (options & EditorListOption.ListLabel) != 0;
            bool showElements = !showListLabel || array.isExpanded;

            if (!showElements)
            {
                return;
            }

            for (int i = 0; i < array.arraySize; i++)
            {
                var element = array.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    DrawElementHeader(array, element, i, options);

                    EditorGUI.indentLevel += 1;
                    DrawElementBody(element);
                    EditorGUI.indentLevel -= 1;
                }
                EditorGUILayout.EndVertical();
            }

            DeleteElement(array);
        }

        private static void DrawElementHeader(SerializedProperty array, SerializedProperty property, int index, EditorListOption options)
        {
            bool showButtons = (options & EditorListOption.Buttons) != 0;
            if (showButtons)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button(_FoldoutButtonContent, EditorStyles.miniButton, _MiniButtonWidth))
                    {
                        property.isExpanded = !property.isExpanded;
                    }

                    EditorGUILayout.LabelField(property.displayName);

                    ShowButtons(array, index, options);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        private static void DrawElementBody(SerializedProperty property)
        {
            EditorGUILayout.BeginVertical();
            if (property.isExpanded)
            {
                var children = property.GetChildren();
                foreach (var child in children)
                {
                    EditorGUILayout.PropertyField(child, true);
                }
            }
            EditorGUILayout.EndVertical();
        }

        private static void ShowButtons(SerializedProperty list, int index, EditorListOption options)
        {
            bool showMove = (options & EditorListOption.ButtonMove) != 0;
            bool showPlus = (options & EditorListOption.ButtonPlus) != 0;
            bool showMinus = (options & EditorListOption.ButtonMinus) != 0;

            if (showMove)
            {
                if (GUILayout.Button(_MoveButtonContent, EditorStyles.miniButtonLeft, _MiniButtonWidth))
                {
                    list.MoveArrayElement(index, index + 1);
                }
            }

            if (showPlus)
            {
                if (GUILayout.Button(_DuplicateButtonContent, EditorStyles.miniButtonMid, _MiniButtonWidth))
                {
                    list.InsertArrayElementAtIndex(index);
                }
            }

            if (showMinus)
            {
                if (GUILayout.Button(_DeleteButtonContent, EditorStyles.miniButtonRight, _MiniButtonWidth))
                {
                    _DeleteIndex = index;
                }
            }
        }

        private static void DeleteElement(SerializedProperty array)
        {
            if (_DeleteIndex == -1)
            {
                return;
            }

            int oldSize = array.arraySize;
            array.DeleteArrayElementAtIndex(_DeleteIndex);
            if (array.arraySize == oldSize)
            {
                array.DeleteArrayElementAtIndex(_DeleteIndex);
            }

            _DeleteIndex = -1;
        }
        #endregion Private Methods
    }
}