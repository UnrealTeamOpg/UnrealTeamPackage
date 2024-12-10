using System;
using System.Reflection;
using SolidUtilities.UnityEditorInternals;
using UnityEditor;

namespace UnrealTeam.Common.Utils
{
    public static class ReflectionUtils
    {
        public static void SetInPrivateSetter(object instance, string propertyName, object value)
        {
            instance.GetType()
                !.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)
                !.GetSetMethod(nonPublic: true)
                .Invoke(instance, new[] { value });
        }        
        
        public static T GetFromProperty<T>(object instance, string propertyName)
        {
            return (T) instance.GetType()
                !.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)
                !.GetGetMethod(nonPublic: true)
                .Invoke(instance, Array.Empty<object>());
        }

        public static bool HasAttribute<TAttribute>(SerializedProperty serializedProperty) where TAttribute : Attribute
            => HasAttribute<TAttribute>(GetSerializedField(serializedProperty));
        
        public static bool HasAttribute<TAttribute>(FieldInfo fieldInfo) where TAttribute : Attribute 
            => fieldInfo.GetCustomAttributes(typeof(TAttribute), true).Length > 0;

        public static FieldInfo GetSerializedField(SerializedProperty serializedProperty) 
            => serializedProperty.GetFieldInfoAndType().FieldInfo;
    }
}