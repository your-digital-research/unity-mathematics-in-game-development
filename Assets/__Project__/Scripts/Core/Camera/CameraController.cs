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
        [SerializeField] [Range(5, 25)] private float moveSpeed;
        [SerializeField] [Range(0, 360)] private float rotateSpeed;
        [SerializeField] [NaughtyAttributes.MinMaxSlider(-180, 180)] private Vector2 xRotationClamp;

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

            Vector3 moveDirection = new Vector3(horizontalInput, upInput - downInput, verticalInput);
            moveDirection.Normalize(); // Ensure diagonal movement isn't faster

            // Apply Movement
            transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
        }

        private void HandleRotation()
        {
            // Rotate Input
            float rotateX = Input.GetAxis("Mouse X") * rotateSpeed;
            float rotateY = Input.GetAxis("Mouse Y") * rotateSpeed;

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