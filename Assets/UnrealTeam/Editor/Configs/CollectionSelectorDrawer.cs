using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnrealTeam.Runtime.Modules.Configs.Classes;

namespace UnrealTeam.Common.Configs.Editor
{

    [CustomPropertyDrawer(typeof(CollectionSelector<>), true)]
    public class CollectionSelectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Type itemType = GetItemType();
            SerializedProperty collectionProperty = property.FindPropertyRelative("Collection");

            if (collectionProperty == null || !collectionProperty.isArray)
            {
                EditorGUI.LabelField(position, label.text, "Collection field not found or not an array.");
                return;
            }

            var labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth,
                EditorGUIUtility.singleLineHeight);
            Rect dropdownRect = GetDropdownRect(position);
            
            
            SerializedProperty idProperty = property.FindPropertyRelative("Id");
            SerializedProperty nameProperty = property.FindPropertyRelative("Name");

            // Создание списка элементов
            List<object> items = GetCollectionItems(collectionProperty);
            if (items == null || items.Count == 0)
            {
                DrawErrorLabel(labelRect, dropdownRect, label, "No items found in collection");
                return;
            }
            
            DrawField(position, labelRect, dropdownRect, label, idProperty, nameProperty, items);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        private void DrawField(
            Rect position,
            Rect labelRect,
            Rect dropdownRect,
            GUIContent label,
            SerializedProperty idProperty,
            SerializedProperty nameProperty,
            List<object> items)
        {
            EditorGUI.PrefixLabel(labelRect, label);

            string selectedItemName = nameProperty.stringValue;
            bool dropdownClicked = EditorGUI.DropdownButton(
                dropdownRect,
                new GUIContent(string.IsNullOrEmpty(selectedItemName) ? "(None)" : selectedItemName),
                FocusType.Keyboard);

            if (dropdownClicked)
            {
                ShowDropdownMenu(idProperty, nameProperty, items);
            }
        }

        private void ShowDropdownMenu(SerializedProperty idProperty, SerializedProperty nameProperty,
            List<object> items)
        {
            var menu = new GenericMenu();

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                string itemName = nameProperty.stringValue;
                string itemId = i.ToString();
                bool isSelected = idProperty.stringValue == itemId;

                menu.AddItem(new GUIContent(itemName), isSelected, () =>
                {
                    idProperty.stringValue = itemId;
                    nameProperty.stringValue = itemName;
                    idProperty.serializedObject.ApplyModifiedProperties();
                });
            }

            menu.ShowAsContext();
        }

        private void DrawErrorLabel(Rect labelRect, Rect dropdownRect, GUIContent label, string errorMessage)
        {
            EditorGUI.PrefixLabel(labelRect, label);
            EditorGUI.LabelField(dropdownRect, new GUIContent(errorMessage), new GUIStyle
            {
                normal = new GUIStyleState { textColor = Color.red }
            });
        }

        private List<object> GetCollectionItems(SerializedProperty collectionProperty)
        {
            List<object> items = new();
            for (int i = 0; i < collectionProperty.arraySize; i++)
            {
                var element = collectionProperty.GetArrayElementAtIndex(i);
                if (element.propertyType == SerializedPropertyType.String)
                    items.Add(element.stringValue);
                else if (element.propertyType == SerializedPropertyType.ObjectReference)
                    items.Add(element.objectReferenceValue);
                else
                    items.Add(element);
            }

            return items;
        }

        private Type GetItemType()
        {
            return fieldInfo.FieldType.GetGenericArguments()[0];
        }

        private static Rect GetDropdownRect(Rect position)
        {
            return new Rect(
                position.x + EditorGUIUtility.labelWidth,
                position.y,
                position.width - EditorGUIUtility.labelWidth,
                EditorGUIUtility.singleLineHeight);
        }
    }
}
