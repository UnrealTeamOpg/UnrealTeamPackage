#if UNITY_EDITOR
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnrealTeam.Common.Attributes.Editor
{
    [CustomPropertyDrawer(typeof(CollectionElementLabel))]
    public class CollectionElementLabelDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var arrayLabel = (CollectionElementLabel)attribute;
            Object contextObject = property.serializedObject.targetObject;
            string methodName = arrayLabel.LabelGetterMethod;
        
            MethodInfo method = contextObject.GetType().GetMethod(methodName,
                BindingFlags.NonPublic |
                BindingFlags.Public |
                BindingFlags.Instance);

            if (method != null && method.ReturnType == typeof(string))
            {
                string indexString = property.propertyPath.Split('[').Last().TrimEnd(']');
                if (int.TryParse(indexString, out int index))
                {
                    var customLabel = method.Invoke(contextObject, new object[] { index }) as string;
                    EditorGUI.PropertyField(position, property, new GUIContent(customLabel), true);
                }
            }
        }
    
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
            => EditorGUI.GetPropertyHeight(property, label, true);
    }
}
#endif