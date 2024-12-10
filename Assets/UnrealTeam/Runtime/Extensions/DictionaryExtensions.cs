using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace UnrealTeam.Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFactory)
        {
            if (dictionary.TryGetValue(key, out TValue value))
                return value;

            value = valueFactory.Invoke();
            dictionary[key] = value;
            return value;
        }       
        
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue newValue)
        {
            if (dictionary.TryGetValue(key, out TValue value))
                return value;

            dictionary[key] = newValue;
            return newValue;
        }

        public static void AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
            => dictionary[key] = value;
        
        public static async UniTask<TValue> GetOrAddAsync<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<UniTask<TValue>> factory)
        {
            if (dictionary.TryGetValue(key, out TValue value))
                return value;

            value = await factory.Invoke();
            dictionary[key] = value;
            return value;
        }
    }
}