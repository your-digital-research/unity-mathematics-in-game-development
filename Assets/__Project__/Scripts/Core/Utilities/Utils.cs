using Core.Types;
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

        /// <summary>
        /// Dot Product returns a three-dimensional vector which is perpendicular to the original vectors.
        /// Uses only Vectors for calculation.
        /// </summary>
        /// <param name="firstVector">First Vector of Product</param>
        /// <param name="secondVector">Second Vector of Product</param>
        /// <returns>
        /// Returns three-dimensional vector which is perpendicular to the original vectors
        /// </returns>
        public static Vector3 CrossProduct(Vector3 firstVector, Vector3 secondVector)
        {
            float x = firstVector.y * secondVector.z - firstVector.z * secondVector.y;
            float y = firstVector.z * secondVector.x - firstVector.x * secondVector.z;
            float z = firstVector.x * secondVector.y - firstVector.y * secondVector.x;

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Dot Product returns a three-dimensional vector which is perpendicular to the original vectors.
        /// Uses the Matrix and Vector multiplication for calculation.
        /// </summary>
        /// <param name="firstVector">First Vector of Product</param>
        /// <param name="secondVector">Second Vector of Product</param>
        /// <returns>
        /// Returns three-dimensional vector which is perpendicular to the original vectors
        /// </returns>
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

        /// <summary>
        /// The Quaternion product.
        /// Represented in scalar-vector form.
        /// </summary>
        /// <param name="firstQuaternion">First Quaternion of Product</param>
        /// <param name="secondQuaternion">Second Quaternion of Product</param>
        /// <returns>
        /// Returns Quaternion product
        /// </returns>
        public static CustomQuaternion MultiplyQuaternion(CustomQuaternion firstQuaternion, CustomQuaternion secondQuaternion)
        {
            float firstScalar = firstQuaternion.w;
            float secondScalar = secondQuaternion.w;

            Vector3 firstVector = new Vector3(firstQuaternion.x, firstQuaternion.y, firstQuaternion.z);
            Vector3 secondVector = new Vector3(secondQuaternion.x, secondQuaternion.y, secondQuaternion.z);

            float finalScalar = firstScalar * secondScalar - Vector3.Dot(firstVector, secondVector);
            Vector3 finalVector = firstScalar * secondVector + secondScalar * firstVector + Vector3.Cross(firstVector, secondVector);

            return new CustomQuaternion(finalVector.x, finalVector.y, finalVector.z, finalScalar);
        }

        /// <summary>
        /// Quaternion conjugation.
        /// Conjugate the definition, which implies changing the sign of all imaginary terms.
        /// </summary>
        /// <param name="quaternion">Quaternion to conjugate</param>
        /// <returns>Returns conjugate Quaternion</returns>
        public static CustomQuaternion ConjugateQuaternion(CustomQuaternion quaternion)
        {
            return new CustomQuaternion(-quaternion.x, -quaternion.y, -quaternion.z, quaternion.w);
        }

        /// <summary>
        /// Create Quaternion using angle and axis.
        /// </summary>
        /// <param name="angle">Angle of Quaternion</param>
        /// <param name="axis">Axis of Quaternion</param>
        /// <returns>Returns a Quaternion</returns>
        public static CustomQuaternion CreateQuaternion(float angle, Vector3 axis)
        {
            float realPart = Mathf.Cos(angle / 2f);
            float imaginaryPartMagnitude = Mathf.Sin(angle / 2f);
            Vector3 rotationAxisComponent = Vector3.Normalize(axis) * imaginaryPartMagnitude;

            return new CustomQuaternion(rotationAxisComponent.x, rotationAxisComponent.y, rotationAxisComponent.z, realPart);
        }

        /// <summary>
        /// Rotates point in Quaternion by given axis an angle
        /// </summary>
        /// <param name="point">Point to rotate</param>
        /// <param name="axis">Axis of rotation</param>
        /// <param name="angle">Angle of rotation</param>
        /// <returns></returns>
        public static Vector3 RotateQuaternion(Vector3 point, Vector3 axis, float angle)
        {
            CustomQuaternion quaternion = CreateQuaternion(angle, axis);
            CustomQuaternion conjugateQuaternion = ConjugateQuaternion(quaternion);
            CustomQuaternion quaternionOfPoint = new CustomQuaternion(point.x, point.y, point.z, 0f);
            CustomQuaternion rotatedPoint = MultiplyQuaternion(quaternion, quaternionOfPoint);

            rotatedPoint = MultiplyQuaternion(rotatedPoint, conjugateQuaternion);

            return new Vector3(rotatedPoint.x, rotatedPoint.y, rotatedPoint.z);
        }
    }
}