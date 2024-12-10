using UnityEngine;

namespace UnrealTeam.Common.Utils
{
    public static class PhysicsUtils
    {
        public static void OrderRaycastHits(RaycastHit[] raycastHits, int hitsCount)
        {
            for (var i = 1; i < hitsCount; i++)
            {
                RaycastHit key = raycastHits[i];
                int j = i - 1;

                while (j >= 0 && raycastHits[j].distance > key.distance)
                {
                    raycastHits[j + 1] = raycastHits[j];
                    j -= 1;
                }
                
                raycastHits[j + 1] = key;
            }
        }
    }
}