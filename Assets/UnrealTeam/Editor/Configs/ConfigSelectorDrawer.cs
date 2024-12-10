#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityDropdown.Editor;
using UnityEditor;
using UnityEngine;
using UnrealTeam.Common.Modules.Configs;
using UnrealTeam.Common.Utils;

namespace UnrealTeam.Common.Configs.Editor
{
    [CustomPropertyDrawer(typeof(ConfigSelector<>), true)]
    public class ConfigSelectorDrawer : PropertyDrawer
    {
        private readonly Color _errorButtonColor = new(1f, 0, 0, 0.7f);
        private readonly Color _errorTextColor = new(1f, 0, 0, 1f);
        private const float _verticalPadding = 5f;


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Type configType = GetConfigType();
            var labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
            Rect dropdownRect = GetDropdownRect(position, property);

            if (!typeof(IMultipleConfig).IsAssignableFrom(configType) || !typeof(ScriptableObject).IsAssignableFrom(configType))
            {
                DrawErrorLabel(labelRect, dropdownRect, label, $"Config must be {nameof(ScriptableObject)}");
                return;
            }

            if (configType == typeof(IMultipleConfig) || configType == typeof(SoMultipleConfig))
            {
                DrawErrorLabel(labelRect, dropdownRect, label, "Select concrete config type");
                return;
            }

            IMultipleConfig[] configs = EditorUtils.GetSoInstances<IMultipleConfig>(configType);
            if (configs == null || configs.Length == 0)
            {
                DrawErrorLabel(labelRect, dropdownRect, label, "No configs found");
                return;
            }

            SerializedProperty idProperty = property.FindPropertyRelative($"<{nameof(ConfigSelector<IMultipleConfig>.Id)}>k__BackingField");
            SerializedProperty nameProperty = property.FindPropertyRelative($"<{nameof(ConfigSelector<IMultipleConfig>.Name)}>k__BackingField");
            DrawField(position, labelRect, dropdownRect, label, property, idProperty, nameProperty, configs);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float lineHeight = EditorGUIUtility.singleLineHeight;
            if (!ReflectionUtils.HasAttribute<DrawOpenConfigAttribute>(property))
                return lineHeight;
            
            IMultipleConfig[] configs = EditorUtils.GetSoInstances<IMultipleConfig>(GetConfigType());
            if (configs == null || configs.Length == 0)
                return lineHeight;
            
            SerializedProperty idProperty = property.FindPropertyRelative($"<{nameof(ConfigSelector<IMultipleConfig>.Id)}>k__BackingField");
            if (FindConfig(configs, idProperty.stringValue) != null)
                return (lineHeight + _verticalPadding) * 2;

            return lineHeight;
        }

        private void DrawField(
            Rect position,
            Rect labelRect,
            Rect dropdownRect,
            GUIContent label, 
            SerializedProperty property,
            SerializedProperty idProperty,
            SerializedProperty nameProperty,
            IMultipleConfig[] configs)
        {
            EditorGUI.PrefixLabel(labelRect, label);
            ScriptableObject config = FindConfig(configs, idProperty.stringValue);
            bool dropdownButton;

            if (config != null)
            {
                dropdownButton = EditorGUI.DropdownButton(dropdownRect, new GUIContent(config.name), FocusType.Keyboard);
            }
            else if (!string.IsNullOrEmpty(nameProperty.stringValue))
            {
                Color previousColor = GUI.color;
                GUI.color = _errorButtonColor;
                dropdownButton = EditorGUI.DropdownButton(dropdownRect, new GUIContent(nameProperty.stringValue),
                    FocusType.Keyboard);
                GUI.color = previousColor;
            }
            else
            {
                dropdownButton = EditorGUI.DropdownButton(dropdownRect, new GUIContent("(None)"), FocusType.Keyboard);
            }

            if (dropdownButton)
                ShowConfigDropdown(idProperty, nameProperty, configs);

            if (config != null && ReflectionUtils.HasAttribute<DrawOpenConfigAttribute>(property))
            {
                var openConfigRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + _verticalPadding, position.width, EditorGUIUtility.singleLineHeight);
                if (GUI.Button(openConfigRect, new GUIContent("Open Config")))
                    Selection.activeObject = config;
            }
        }


        private void DrawErrorLabel(Rect labelRect, Rect dropdownRect, GUIContent label, string dropdownText)
        {
            EditorGUI.PrefixLabel(labelRect, label);
            Color previousColor = GUI.color;
            GUI.color = _errorTextColor;
            EditorGUI.LabelField(dropdownRect, dropdownText);
            GUI.color = previousColor;
        }

        private void ShowConfigDropdown(SerializedProperty idProperty, SerializedProperty nameProperty, IMultipleConfig[] configs)
        {
            List<DropdownItem<string>> dropdownItems = configs
                .Select(config => new DropdownItem<string>(
                    value: config.Id,
                    path: (config as ScriptableObject)!.name,
                    isSelected: config.Id == idProperty.stringValue)
                ).ToList();

            var dropdownMenu = new DropdownMenu<string>(
                dropdownItems,
                onValueSelected: selectedId => OnDropdownItemSelected(selectedId, idProperty, nameProperty, configs),
                searchbarMinItemsCount: 10,
                sortItems: false,
                showNoneElement: true
            );

            dropdownMenu.ShowAsContext();
        }

        private void OnDropdownItemSelected(string selectedId, SerializedProperty idProperty, SerializedProperty nameProperty, IMultipleConfig[] configs)
        {
            idProperty.stringValue = selectedId;
            nameProperty.stringValue = string.IsNullOrEmpty(selectedId) ? null : FindConfig(configs, selectedId).name;
            EditorUtils.SaveSerialization(idProperty);
        }

        private Type GetConfigType()
        {
            Type enumerableField = fieldInfo
                .FieldType
                .GetInterfaces()
                .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IList<>));
            
            Type selectorType = enumerableField != null
                ? enumerableField.GetGenericArguments().FirstOrDefault()
                : fieldInfo.FieldType;

            Type configType = selectorType!.GetGenericArguments().SingleOrDefault();
            return configType;
        }

        private ScriptableObject FindConfig(IMultipleConfig[] configs, string id)
            => configs.FirstOrDefault(c => c.Id == id) as ScriptableObject;

        private static Rect GetDropdownRect(Rect position, SerializedProperty property) 
            => ReflectionUtils.HasAttribute<HideLabelAttribute>(property)
                ? new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight)
                : new Rect(position.x + EditorGUIUtility.labelWidth, position.y, position.width - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
    }
}
#endif