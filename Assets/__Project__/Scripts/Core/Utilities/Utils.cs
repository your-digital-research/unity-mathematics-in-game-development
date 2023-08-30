using UnityEngine;

namespace Core.Utilities
{
    public static class Utils
    {
        public static float DotProduct(Vector3 p0, Vector3 p1, Vector3 c)
        {
            Vector3 a = (p0 - c).normalized;
            Vector3 b = (p1 - c).normalized;

            return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
        }
    }
}