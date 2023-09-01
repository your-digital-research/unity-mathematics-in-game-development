using UnityEngine;

namespace Core.Utilities
{
    public static class Utils
    {
        /// <summary>
        /// The Dot Product (Scalar Product) between vector A and vector B
        /// is equal to the sum of the products of each component.
        /// </summary>
        /// <param name="firstPoint">First Point of Product</param>
        /// <param name="secondPoint">Second Point of Product</param>
        /// <param name="centerPoint">Reference (Center) Point of Product</param>
        /// <returns>
        /// Returns the value of the cosine of the angle between two-unit vectors.
        /// So its result lies within a range from -1f to 1f
        /// </returns>
        public static float DotProduct(Vector3 firstPoint, Vector3 secondPoint, Vector3 centerPoint)
        {
            Vector3 firstVector = (firstPoint - centerPoint).normalized;
            Vector3 secondVector = (secondPoint - centerPoint).normalized;

            return (firstVector.x * secondVector.x) + (firstVector.y * secondVector.y) + (firstVector.z * secondVector.z);
        }

        public static Vector3 CrossProduct(Vector3 firstVector, Vector3 secondVector)
        {
            float x = firstVector.y * secondVector.z - firstVector.z * secondVector.y;
            float y = firstVector.z * secondVector.x - firstVector.x * secondVector.z;
            float z = firstVector.x * secondVector.y - firstVector.y * secondVector.x;

            return new Vector3(x, y, z);
        }

        public static Vector3 CrossProductViaMatrix(Vector3 firstVector, Vector3 secondVector)
        {
            Matrix4x4 matrix = new Matrix4x4
            {
                [0, 0] = 0,
                [0, 1] = secondVector.z,
                [0, 2] = -secondVector.y,

                [1, 0] = -secondVector.z,
                [1, 1] = 0,
                [1, 2] = secondVector.x,

                [2, 0] = secondVector.y,
                [2, 1] = -secondVector.x,
                [2, 2] = 0
            };

            return matrix * firstVector;
        }
    }
}