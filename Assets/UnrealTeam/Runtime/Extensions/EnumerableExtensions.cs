using System;
using System.Collections.Generic;

namespace UnrealTeam.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach(T item in enumeration) 
                action(item);
        }
        public static T GetElementAt<T>(this IEnumerable<T> enumeration,int index)
        {
            var currentIndex = 0;
            foreach (var item in enumeration)
            {
                if (currentIndex == index)
                    return item;
                currentIndex++;
            }

            throw new InvalidOperationException("Элемент не найден");
        }
    }
}