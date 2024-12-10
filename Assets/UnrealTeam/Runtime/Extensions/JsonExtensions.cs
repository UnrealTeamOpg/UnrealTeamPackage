using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace UnrealTeam.Common.Extensions
{
    public static class JsonExtensions
    {
        public static T FromJson<T>(this string jsonString)
            => JsonConvert.DeserializeObject<T>(jsonString);

        public static async UniTask<string> SaveJsonAsync(this string json, string fileName, string errorClass = null)
        {
            var path = Path.Combine(Application.dataPath, $"{fileName}.json");
            try
            {
                await using var writer = new StreamWriter(path);

                await writer.WriteAsync(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error saving JSON to file: {e.Message}-- error point:{errorClass}");
            }

            return json;
        }
        
        
        public static async UniTask<T> LoadJsonAsync<T>(string fileName)
        {
            var path = Path.Combine(Application.dataPath, $"{fileName}.json");

            try
            {
                if (!File.Exists(path))
                {
                    Debug.LogError($"Файл не найден: {path}");
                    return default;
                }

                using var reader = new StreamReader(path);
                
                var json = await reader.ReadToEndAsync();

                var result = FromJson<T>(json);
                if (result == null)
                {
                    Debug.LogError($"Ошибка десериализации JSON из файла: {path}");
                }
                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"Ошибка при загрузке JSON из файла: {e.Message}");
                return default;
            }
        }

        
        public static string ToJson<T>(this T obj)
            => JsonConvert.SerializeObject(obj, Formatting.Indented,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                });

        public static T GetValueOrDefault<T>(this JObject jObject, string key, T defaultValue = default)
            => jObject.TryGetValue(key, out JToken jToken)
                ? jToken.ToObject<T>()
                : defaultValue;

        public static T GetValueOrDefault<T>(this JObject jObject, string key, Func<T> defaultValueGetter)
            => jObject.TryGetValue(key, out JToken jToken)
                ? jToken.ToObject<T>()
                : defaultValueGetter.Invoke();

        public static T GetNestedValueOrDefault<T>(this JObject jObject, string key, T defaultValue = default)
        {
            JToken jToken = jObject.SelectToken(key);
            return jToken != null
                ? jToken.ToObject<T>()
                : defaultValue;
        }

        public static bool HasValue(this JObject jObject, string key)
            => jObject.TryGetValue(key, out _);
    }
}