using UnityEngine;

namespace Core.Types
{
    public struct ViewCastInfo
    {
        public readonly bool Hit;

        public readonly float Angle;
        public readonly float Distance;

        public Vector3 Point;

        public ViewCastInfo(bool hit, float angle, float distance, Vector3 point)
        {
            Hit = hit;
            Angle = angle;
            Distance = distance;
            Point = point;
        }
    }
}