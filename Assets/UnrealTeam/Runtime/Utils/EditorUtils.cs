#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using UnityEditor;
using Object = UnityEngine.Object;

namespace UnrealTeam.Common.Utils
{
    public static class EditorUtils
    {
        public static void SetDirtyIfNot(Object obj)
        {
            if (!EditorUtility.IsDirty(obj))
                EditorUtility.SetDirty(obj);
        }

        public static void SaveSerialization(Object obj)
        {
            EditorUtility.SetDirty(obj);
            SaveAssets();
        }

        public static void SaveSerialization(SerializedObject serializedObject)
        {
            serializedObject.ApplyModifiedProperties();
            SaveAssets();
        }

        public static void SaveSerialization(SerializedProperty serializedProperty) 
            => SaveSerialization(serializedProperty.serializedObject);

        public static void SaveAssets() 
            => SaveAssetsAsync().Forget();

        public static T[] GetSoInstances<T>() where T : Object
            => AssetDatabase
                .FindAssets($"t:{typeof(T).Name}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>)
                .ToArray();

        public static T GetSoInstance<T>() where T : Object
            => AssetDatabase
                .FindAssets($"t:{typeof(T).Name}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>)
                .FirstOrDefault();

        public static Object[] GetSoInstances(Type type)
            => AssetDatabase
                .FindAssets($"t:{type.Name}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(path => AssetDatabase.LoadAssetAtPath(path, type))
                .ToArray();

        public static TCastTo[] GetSoInstances<TCastTo>(Type type)
            => AssetDatabase
                .FindAssets($"t:{type.Name}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(path => AssetDatabase.LoadAssetAtPath(path, type))
                .Cast<TCastTo>()
                .ToArray();

        public static void OpenFolder(string folderPath)
        {
            Object obj = AssetDatabase.LoadAssetAtPath(folderPath, typeof(Object));
            EditorUtility.FocusProjectWindow();
            var projectBrowser = Type.GetType("UnityEditor.ProjectBrowser,UnityEditor");
            
            object lastInteractedBrowser = projectBrowser
                !.GetField("s_LastInteractedProjectBrowser", BindingFlags.Static | BindingFlags.Public)
                ?.GetValue(null);
            
            MethodInfo showDirectoryMethod = projectBrowser
                .GetMethod("ShowFolderContents", BindingFlags.NonPublic | BindingFlags.Instance);
            
            showDirectoryMethod!.Invoke(lastInteractedBrowser, new object[ ] { obj.GetInstanceID(), true });
        }

        public static HashSet<Object> GetActiveSelections(Type assetType)
            => Selection.assetGUIDs
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(path => AssetDatabase.LoadAssetAtPath(path, assetType))
                .ToHashSet();

        private static async UniTask SaveAssetsAsync()
        {
            // Avoiding unity draw editor exceptions
            await UniTask.WaitForSeconds(0.05f);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
    }
}
#endif
