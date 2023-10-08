using Core.UI;
using System;
using UnityEngine;
using Cinemachine;
using Zenject;
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

        private CameraMovementView _cameraMovementView;

        private IDisposable _movementDisposable;

        private float _currentXRotation;

        private float _horizontalInput;
        private float _verticalInput;
        private float _downInput;
        private float _upInput;

        private bool _isBoosted;

        private float _pitch;
        private float _yaw;

        #endregion

        #region ZENJECT

        [Inject]
        private void Constructor(CameraMovementView cameraMovementView)
        {
            _cameraMovementView = cameraMovementView;
        }

        #endregion

        #region MONO

        private void Start()
        {
            Init();
        }

        private void OnDisable()
        {
            StopMovement();
            RemoveListeners();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void OnMovementUpdated(MovementDirection direction, bool toggle)
        {
            switch (direction)
            {
                case MovementDirection.Unknown:
                    break;
                case MovementDirection.Forward:
                    _verticalInput = toggle ? 1 : 0;
                    break;
                case MovementDirection.Right:
                    _horizontalInput = toggle ? 1 : 0;
                    break;
                case MovementDirection.Back:
                    _verticalInput = toggle ? -1 : 0;
                    break;
                case MovementDirection.Left:
                    _horizontalInput = toggle ? -1 : 0;
                    break;
                case MovementDirection.Down:
                    _downInput = toggle ? 1 : 0;
                    break;
                case MovementDirection.Up:
                    _upInput = toggle ? 1 : 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        private void OnRotationUpdated(RotationDirection direction, bool toggle)
        {
            switch (direction)
            {
                case RotationDirection.Unknown:
                    break;
                case RotationDirection.PitchUp:
                    _pitch = toggle ? 1 : 0;
                    break;
                case RotationDirection.YawRight:
                    _yaw = toggle ? 1 : 0;
                    break;
                case RotationDirection.PitchDown:
                    _pitch = toggle ? -1 : 0;
                    break;
                case RotationDirection.YawLeft:
                    _yaw = toggle ? -1 : 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        private void Init()
        {
            InitFields();

            StartMovement();

            AddListeners();
        }

        private void InitFields()
        {
            _currentXRotation = 0;

            _horizontalInput = 0;
            _verticalInput = 0;
            _upInput = 0;
            _downInput = 0;

            _isBoosted = false;

            _pitch = 0;
            _yaw = 0;
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

            Move(horizontalInput, verticalInput, upInput, downInput, isBoosted);
            Move(_horizontalInput, _verticalInput, _upInput, _downInput, _isBoosted);
        }

        private void HandleRotation()
        {
            // Rotate Input
            float yaw = Input.GetAxis("Mouse X");
            float pitch = Input.GetAxis("Mouse Y");

            Rotate(yaw, pitch, false);
            Rotate(_yaw, _pitch, true);
        }

        private void Move(float horizontalInput, float verticalInput, float upInput = 0, float downInput = 0, bool isBoosted = false)
        {
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

        private void Rotate(float yaw, float pitch, bool includeTime)
        {
            // Apply Rotation
            transform.Rotate(Vector3.up * yaw * (includeTime ? rotationSpeed : 5) * (includeTime ? Time.deltaTime : 1));

            // Calculate new X-axis rotation
            _currentXRotation -= pitch * (includeTime ? rotationSpeed : 5) * (includeTime ? Time.deltaTime : 1);
            _currentXRotation = Mathf.Clamp(_currentXRotation, xRotationClamp.x, xRotationClamp.y);

            // Create Quaternions for X and Y-axis rotations
            Quaternion xRotation = Quaternion.Euler(_currentXRotation, 0, 0);
            Quaternion yRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

            // Apply the rotations to the camera, blocking Z-axis rotation
            transform.rotation = Quaternion.Euler(xRotation.eulerAngles.x, yRotation.eulerAngles.y, 0);
        }

        private void AddListeners()
        {
            if (_cameraMovementView == null) return;

            _cameraMovementView.MovementUpdated += OnMovementUpdated;
            _cameraMovementView.RotationUpdated += OnRotationUpdated;
        }

        private void RemoveListeners()
        {
            if (_cameraMovementView == null) return;

            _cameraMovementView.MovementUpdated -= OnMovementUpdated;
            _cameraMovementView.RotationUpdated -= OnRotationUpdated;
        }

        #endregion
    }
}