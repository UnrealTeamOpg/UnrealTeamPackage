using UnityEngine;

namespace UnrealTeam.Common.Utils
{
    public static class DebugUtils
    {
        private const int _segments = 20;
        private const float _deltaTheta = (float)(2.0 * Mathf.PI) / _segments;
        private const float _deltaPhi = Mathf.PI / _segments;
        
        
        public static void DrawSphere(Vector3 center, float radius, Color color, float duration = 0, bool depthTest = true)
        {
            Vector3 previousPoint = Vector3.zero;
            Vector3 nextPoint = Vector3.zero;

            for (var i = 0; i <= _segments; i++)
            {
                float theta = i * _deltaTheta;
                for (var j = 0; j <= _segments; j++)
                {
                    float phi = j * _deltaPhi;

                    nextPoint.x = center.x + radius * Mathf.Sin(phi) * Mathf.Cos(theta);
                    nextPoint.y = center.y + radius * Mathf.Sin(phi) * Mathf.Sin(theta);
                    nextPoint.z = center.z + radius * Mathf.Cos(phi);

                    if (i > 0 && j > 0) 
                        Debug.DrawLine(previousPoint, nextPoint, color, duration, depthTest);
                    
                    previousPoint = nextPoint;
                }
            }
        }
    }
}