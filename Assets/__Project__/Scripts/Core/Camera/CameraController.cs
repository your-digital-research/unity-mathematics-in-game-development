using System;
using UnityEngine;
using Cinemachine;
using UniRx;

namespace Core.Camera
{
    public class CameraController : MonoBehaviour
    {
        #region SERIALIZED_VARIABLES

        [Header("Cameras")]
        [SerializeField] private CinemachineBrain brain;
        [SerializeField] private CinemachineVirtualCamera mainCamera;

        [Header("Settings")]
        [SerializeField] [Range(5, 25)] private float movementSpeed;
        [SerializeField] [Range(0, 360)] private float rotationSpeed;
        [SerializeField] [NaughtyAttributes.MinMaxSlider(-180, 180)] private Vector2 xRotationClamp;
        [SerializeField] [NaughtyAttributes.MinMaxSlider(-250, 250)] private Vector2 xPositionClamp;
        [SerializeField] [NaughtyAttributes.MinMaxSlider(-250, 250)] private Vector2 yPositionClamp;
        [SerializeField] [NaughtyAttributes.MinMaxSlider(-250, 250)] private Vector2 zPositionClamp;

        #endregion

        #region PRIVATE_VARIABLES

        private float _currentXRotation;
        private IDisposable _movementDisposable;

        #endregion

        #region MONO

        private void Start()
        {
            Init();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void Init()
        {
            InitRotations();
            StartMovement();
        }

        private void InitRotations()
        {
            _currentXRotation = 0;
        }

        private void StartMovement()
        {
            _movementDisposable ??= Observable
                .EveryUpdate()
                .Repeat()
                .Subscribe(_ =>
                {
                    HandleMovement();
                    HandleRotation();
                });
        }

        private void StopMovement()
        {
            _movementDisposable.Dispose();
            _movementDisposable = null;
        }

        private void HandleMovement()
        {
            // Movement Input
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            float upInput = Input.GetKey(KeyCode.E) ? 1.0f : 0.0f;
            float downInput = Input.GetKey(KeyCode.Q) ? 1.0f : 0.0f;
            bool isBoosted = Input.GetKey(KeyCode.LeftShift);

            Vector3 moveDirection = new Vector3(horizontalInput, upInput - downInput, verticalInput);
            moveDirection.Normalize(); // Ensure diagonal movement isn't faster

            Transform selfTransform = transform;
            float speed = isBoosted ? movementSpeed * 2 : movementSpeed;

            // Apply Movement
            selfTransform.Translate(moveDirection * (speed * Time.deltaTime));

            // Clamp Position
            Vector3 position = selfTransform.position;

            float clampedX = Mathf.Clamp(position.x, xPositionClamp.x, xPositionClamp.y);
            float clampedY = Mathf.Clamp(position.y, yPositionClamp.x, yPositionClamp.y);
            float clampedZ = Mathf.Clamp(position.z, zPositionClamp.x, zPositionClamp.y);

            Vector3 clampedPosition = new Vector3(clampedX, clampedY, clampedZ);

            selfTransform.position = clampedPosition;
        }

        private void HandleRotation()
        {
            // Rotate Input
            float rotateX = Input.GetAxis("Mouse X") * rotationSpeed;
            float rotateY = Input.GetAxis("Mouse Y") * rotationSpeed;

            // Apply Rotation
            transform.Rotate(Vector3.up * rotateX);

            // Calculate new X-axis rotation
            _currentXRotation -= rotateY;
            _currentXRotation = Mathf.Clamp(_currentXRotation, xRotationClamp.x, xRotationClamp.y);

            // Create Quaternions for X and Y-axis rotations
            Quaternion xRotation = Quaternion.Euler(_currentXRotation, 0, 0);
            Quaternion yRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

            // Apply the rotations to the camera, blocking Z-axis rotation
            transform.rotation = Quaternion.Euler(xRotation.eulerAngles.x, yRotation.eulerAngles.y, 0);
        }

        #endregion
    }
}