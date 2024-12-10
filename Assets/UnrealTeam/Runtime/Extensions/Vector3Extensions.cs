using UnityEngine;

namespace UnrealTeam.Common.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 AddX(this Vector3 vector, float offset)
        {
            vector.x += offset;
            return vector;
        }
        
        public static Vector3 AddY(this Vector3 vector, float offset)
        {
            vector.y += offset;
            return vector;
        }
        
        public static Vector3 AddZ(this Vector3 vector, float offset)
        {
            vector.z += offset;
            return vector;
        }

        public static Vector3 ConvertVector2ToVector3(this Vector2 vector2)
            => new Vector3(vector2.x, 0, vector2.y);
        
        public static Vector3 Divide(this Vector3 first, Vector3 second)
            => new(first.x / second.x, first.y / second.y, first.z / second.z);
    }
}