#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using UnrealTeam.Common.Extensions;
using UnrealTeam.Common.Modules.Configs;
using EditorUtils = UnrealTeam.Common.Utils.EditorUtils;

namespace UnrealTeam.Common.Configs.Editor
{
    [CustomPropertyDrawer(typeof(UniqueId))]
    public class UniqueIdDrawer : PropertyDrawer
    {
        private const string _valuePropertyName = "<Value>k__BackingField";
        private const float _buttonWidth = 70;
        private const float _padding = 2;


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            GUI.enabled = false;
            Rect valueRect = position;
            valueRect.width -= _buttonWidth + _padding;
            SerializedProperty valueProperty = property.FindPropertyRelative(_valuePropertyName);
            EditorGUI.PropertyField(valueRect, valueProperty, GUIContent.none);

            GUI.enabled = true;
            Rect buttonRect = position;
            buttonRect.x += position.width - _buttonWidth;
            buttonRect.width = _buttonWidth;
            
            bool isRegenerate = GUI.Button(buttonRect, "Generate");
            GenerateId(valueProperty, isRegenerate);

            EditorGUI.EndProperty();
        }

        private static void GenerateId(SerializedProperty valueProperty, bool isRegenerate)
        {
            if (isRegenerate || string.IsNullOrEmpty(valueProperty.stringValue) || !IsIdUnique(valueProperty))
            {
                valueProperty.stringValue = Guid.NewGuid().ToString();
                EditorUtils.SaveSerialization(valueProperty);
            }
        }

        private static bool IsIdUnique(SerializedProperty idProperty)
        {
            Type objectType = idProperty.serializedObject.targetObject.GetType();
            string targetId = idProperty.stringValue;
            var idCount = 0;
            
            if (objectType.IsSubclassOf(typeof(ScriptableObject)) && 
                objectType.GetInterface(nameof(IMultipleConfig)) != null)
            {
                
                EditorUtils
                    .GetSoInstances(objectType)
                    .ForEach(o =>
                    {
                        if (((IMultipleConfig)o).Id == targetId)
                            idCount++;
                    });
            }

            return idCount <= 1;
        }
    }
}
#endif