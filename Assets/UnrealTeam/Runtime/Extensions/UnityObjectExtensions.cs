using UnityEngine;
using UnityEngine.UI;

namespace UnrealTeam.Common.Extensions
{
    public static class UnityObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject)
            where T : Component
            => gameObject.TryGetComponent(out T modelComponent)
                ? gameObject.AddComponent<T>()
                : modelComponent;

        public static T GetOrAddComponent<T>(this Component component)
            where T : Component
            => component.gameObject.GetOrAddComponent<T>();

        public static void Dispose(this GameObject gameObject)
            => Object.Destroy(gameObject);       
        
        public static void Dispose(this Component component)
            => Object.Destroy(component.gameObject);
        
        public static void SetNormalizeValue(this Slider slider, float currentValue)
        {
            if (Mathf.Approximately(slider.minValue, slider.maxValue))
                return;
            
            var normalizeValue = (currentValue - slider.minValue) / (slider.maxValue - slider.minValue);

            slider.value = normalizeValue;
        }
        
        public static Transform FindNearestChild(this Transform target, Transform current)
        {
            Transform nearestChild = null;
            float minDistanceSquared = Mathf.Infinity;
            
            foreach (Transform child in target)
            {
                float distanceSquared = (current.position - child.position).sqrMagnitude;

                if (distanceSquared < minDistanceSquared)
                {
                    minDistanceSquared = distanceSquared;
                    nearestChild = child;
                }
            }

            return nearestChild;
        }
        
        public static void RemoveAllChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }
        
        public static void SetChildrenActive(this Transform transform, bool isActive)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(isActive);
            }
        }
    }
}