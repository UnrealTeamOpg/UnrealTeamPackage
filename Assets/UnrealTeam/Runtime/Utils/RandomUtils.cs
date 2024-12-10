using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnrealTeam.Common.Additional;
using Random = UnityEngine.Random;

namespace UnrealTeam.Common.Utils
{
    public static class RandomUtils
    {
        /// <summary>
        /// </summary>
        /// <param name="min">Inclusive</param>
        /// <param name="max">Inclusive</param>
        public static int Generate(int min, int max)
            => Random.Range(min, max + 1);

        /// <summary>
        /// </summary>
        /// <param name="min">Inclusive</param>
        /// <param name="max">Inclusive</param>
        public static float Generate(float min, float max)
            => Random.Range(min, max);

        /// <summary>
        /// range.Min inclusive, range.Max inclusive
        /// </summary>
        public static int Generate(IntRange range)
            => Generate(range.Min, range.Max);

        /// <summary>
        /// range.Min inclusive, range.Max inclusive
        /// </summary>
        public static float Generate(FloatRange range)
            => Generate(range.Min, range.Max);

        /// <summary>
        /// </summary>
        /// <param name="chance">Chance coefficient from 0 to 1</param>
        /// <returns></returns>
        public static bool TryChance(float chance)
            => chance switch
            {
                <= 0 => false,
                >= 1.0f => true,
                _ => chance >= Generate(0.0f, 1.0f)
            };

        /// <summary>
        /// Picking item from collection one time (with equal chance)
        /// </summary>
        public static T PickOne<T>(IEnumerable<T> collection)
            => PickMany(collection, 1).Single();

        /// <summary>
        /// Picking unique items from collection multiple times (with equal chance)
        /// </summary>
        public static IEnumerable<T> PickMany<T>(IEnumerable<T> collection, int count)
            => collection.OrderBy(_ => Guid.NewGuid()).Take(count);

        /// <summary>
        /// Picking item from collection with weights
        /// </summary>
        /// <exception cref="InvalidOperationException">When all weights are zero</exception>
        public static T PickOneWithWeights<T>(IEnumerable<T> collection, Func<T, float> weightsGetter)
        {
            float totalSum = 0;
            foreach (T item in collection) 
                totalSum += weightsGetter.Invoke(item);

            if (totalSum == 0)
                throw new InvalidOperationException("Total weights must be more than zero");

            float randomSum = Random.Range(0, totalSum);
            float currentSum = 0;
            foreach (T item in collection)
            {
                float currentWeights = weightsGetter.Invoke(item);
                if (currentWeights == 0)
                    continue;

                currentSum += currentWeights;
                if (currentSum >= randomSum)
                    return item;
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Generates value from -range to range
        /// </summary>
        public static float GenerateFromRange(float range)
        {
            if (range == 0)
                return 0;

            float absValue = Mathf.Abs(range);
            return Generate(-absValue, absValue);
        }

        /// <summary>
        /// Generates value from -range to range for each axis
        /// </summary>
        public static Vector2 GenerateFromRange(Vector2 range)
            => new(GenerateFromRange(range.x), GenerateFromRange(range.y));

        /// <summary>
        /// Generates value from -range to range for each axis
        /// </summary>
        public static Vector3 GenerateFromRange(Vector3 range)
            => new(GenerateFromRange(range.x), GenerateFromRange(range.y), GenerateFromRange(range.z));

        /// <summary>
        /// Generates -1 or 1
        /// </summary>
        public static int GenerateSign()
            => Random.Range(0, 2) == 0 ? -1 : 1;
    }
}