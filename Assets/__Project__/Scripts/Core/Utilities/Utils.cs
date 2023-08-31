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
    }
}