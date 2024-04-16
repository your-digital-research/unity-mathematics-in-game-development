using Core.Types;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
            float x = (firstVector.y * secondVector.z) - (firstVector.z * secondVector.y);
            float y = (firstVector.z * secondVector.x) - (firstVector.x * secondVector.z);
            float z = (firstVector.x * secondVector.y) - (firstVector.y * secondVector.x);

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
            float firstScalar = firstQuaternion.W;
            float secondScalar = secondQuaternion.W;

            Vector3 firstVector = new Vector3(firstQuaternion.X, firstQuaternion.Y, firstQuaternion.Z);
            Vector3 secondVector = new Vector3(secondQuaternion.X, secondQuaternion.Y, secondQuaternion.Z);

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
            return new CustomQuaternion(-quaternion.X, -quaternion.Y, -quaternion.Z, quaternion.W);
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

            return new Vector3(rotatedPoint.X, rotatedPoint.Y, rotatedPoint.Z);
        }

        /// <summary>
        /// Rotates the given point by a given angle.
        /// </summary>
        /// <param name="point">Point to rotate</param>
        /// <param name="angleInDegrees">Rotation angle</param>
        /// <returns>Returns the rotated angle</returns>
        public static Vector2 RotationMatrix2D(Vector2 point, float angleInDegrees)
        {
            float angleInRadians = angleInDegrees * (Mathf.PI / 180);

            Matrix4x4 matrix = new Matrix4x4
            {
                [0, 0] = Mathf.Cos(angleInRadians),
                [0, 1] = -Mathf.Sin(angleInRadians),
                [1, 1] = Mathf.Sin(angleInRadians),
                [1, 1] = Mathf.Cos(angleInRadians)
            };

            return matrix * point;
        }

        /// <summary>
        /// Return Yaw by given angle (Z axis)
        /// </summary>
        /// <param name="angle">Angle of rotation</param>
        /// <returns>Return matrix of rotation</returns>
        public static Matrix4x4 GetYaw(float angle)
        {
            float cosTheta = Mathf.Cos(angle);
            float sinTheta = Mathf.Sin(angle);

            Matrix4x4 matrix = new Matrix4x4
            {
                [0, 0] = cosTheta,
                [0, 1] = -sinTheta,
                [0, 2] = 0,
                [1, 0] = sinTheta,
                [1, 1] = cosTheta,
                [1, 2] = 0,
                [2, 0] = 0,
                [2, 1] = 0,
                [2, 2] = 1
            };

            return matrix;
        }

        /// <summary>
        /// Return Pitch by given angle (X axis)
        /// </summary>
        /// <param name="angle">Angle of rotation</param>
        /// <returns>Return matrix of rotation</returns>
        public static Matrix4x4 GetPitch(float angle)
        {
            float cosTheta = Mathf.Cos(angle);
            float sinTheta = Mathf.Sin(angle);

            Matrix4x4 matrix = new Matrix4x4
            {
                [0, 0] = cosTheta,
                [0, 1] = 0,
                [0, 2] = -sinTheta,
                [1, 0] = 0,
                [1, 1] = 1,
                [1, 2] = 0,
                [2, 0] = sinTheta,
                [2, 1] = 0,
                [2, 2] = cosTheta
            };

            return matrix;
        }

        /// <summary>
        /// Return Roll by given angle (Y axis)
        /// </summary>
        /// <param name="angle">Angle of rotation</param>
        /// <returns>Return matrix of rotation</returns>
        public static Matrix4x4 GetRoll(float angle)
        {
            float cosTheta = Mathf.Cos(angle);
            float sinTheta = Mathf.Sin(angle);

            Matrix4x4 matrix = new Matrix4x4
            {
                [0, 0] = 1,
                [0, 1] = 0,
                [0, 2] = 0,
                [1, 0] = 0,
                [1, 1] = cosTheta,
                [1, 2] = -sinTheta,
                [2, 0] = 0,
                [2, 1] = sinTheta,
                [2, 2] = cosTheta
            };

            return matrix;
        }

        public static float Map(float value, float originalMin, float originalMax, float newMin, float newMax)
        {
            float normalizedValue = Mathf.InverseLerp(originalMin, originalMax, value);
            float remappedValue = Mathf.Lerp(newMin, newMax, normalizedValue);

            return remappedValue;
        }

        public static bool IsInFrontOfCamera(Vector3 worldPosition, Camera outputCamera)
        {
            Transform cameraTransform = outputCamera.transform;

            return Vector3.Dot(worldPosition - cameraTransform.position, cameraTransform.forward) > 0f;
        }

        public static bool IsPointerOverUI()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var results = new List<RaycastResult>();

            EventSystem.current.RaycastAll(eventData, results);

            return results.Count > 0;
        }

        // Function to convert dot product value to angle in degrees [0, 360]
        public static float DotProductToDegreesFullCircle(float dotProductValue)
        {
            // Ensure dotProductValue is within the valid range [-1, 1]
            dotProductValue = Mathf.Clamp(dotProductValue, -1f, 1f);

            // Get the angle based on Dot Product value by mapping
            float angleDegrees = dotProductValue switch
            {
                <= 0 and >= -1 => Map(dotProductValue, -1, 0, 360, 90),
                >= 0 and <= 1 => Map(dotProductValue, 0, 1, 90, 0),
                _ => 0
            };

            return angleDegrees;
        }

        // Function to convert dot product value to angle in degrees [0, 180]
        public static float DotProductToDegreesHalfCircle(float dotProductValue)
        {
            // Ensure dotProductValue is within the valid range [-1, 1]
            dotProductValue = Mathf.Clamp(dotProductValue, -1f, 1f);

            // Calculate the angle in radians using the inverse cosine function
            float angleRadians = Mathf.Acos(dotProductValue);

            // Convert radians to degrees
            float angleDegrees = angleRadians * Mathf.Rad2Deg;

            return angleDegrees;
        }

        // Function to convert angle in degrees to dot product value
        public static float DegreesToDotProduct(float angleDegrees)
        {
            // Convert degrees to radians
            float angleRadians = angleDegrees * Mathf.Deg2Rad;

            // Calculate dot product using the cosine function
            float dotProductValue = Mathf.Cos(angleRadians);

            // Handle precision issues manually
            const float epsilon = 1e-6f; // Adjust the epsilon based on your needs

            if (Mathf.Abs(dotProductValue) < epsilon)
            {
                dotProductValue = 0f;
            }

            return dotProductValue;
        }
    }
}