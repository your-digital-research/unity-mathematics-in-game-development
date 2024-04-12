using Core.Misc;
using Core.Types;
using Core.Utilities;
using Core.Constants;
using System;
using UnityEngine;
using Zenject;
using TMPro;
using UniRx;

namespace Core.UI
{
    public class CrossProductUsageView : MonoBehaviour
    {
        #region SERIALIZED_VARIABLES

        [Header("References")]
        [SerializeField] private ControlPanelTab controlPanelTab;
        [SerializeField] private TextMeshProUGUI rightVectorCrossProductResult;
        [SerializeField] private TextMeshProUGUI forwardVectorCrossProductResult;
        [SerializeField] private Transform raycastSphere;
        [SerializeField] private Transform raycastCube;

        [Header("Settings")]
        [SerializeField] [NaughtyAttributes.MinMaxSlider(-50, 50)] private Vector2 minMaxValue;
        [SerializeField] private Vector3 pointInitialPosition;
        [SerializeField] private Vector3 sphereInitialPosition;

        [Header("Ranges")]
        [SerializeField] private ScaleRange scaleRange;

        [Header("Points")]
        [SerializeField] private Point point;

        #endregion

        #region PRIVATE_VARIABLES

        private TransitionView _transitionView;

        private Vector3 _pointPosition;
        private Vector3 _spherePosition;

        private PointPositionValues _pointPositionValues;
        private SpherePositionValues _spherePositionValues;

        #endregion

        #region ZENJECT

        [Inject]
        private void Constructor(TransitionView transitionView)
        {
            _transitionView = transitionView;
        }

        #endregion

        #region MONO

        private void Start()
        {
            Observable
                .Timer(TimeSpan.FromSeconds(Constant.InitDelay))
                .Subscribe(_ =>
                {
                    _transitionView.Hide();

                    Init();
                });
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        #endregion

        #region PUBLIC_FUNCTIONS

        public void OnControlPanelButtonClick()
        {
            ToggleControlPanel();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void OnScaleRangeUpdated(float scale)
        {
            UpdateSphereScale(scale);
            PerformRaycast();
        }

        private void OnPointPositionUpdated(int pointIndex, Axis axis, float value)
        {
            float clampedValue = Mathf.Clamp(value, minMaxValue.x, minMaxValue.y);

            UpdatePointPosition(axis, clampedValue);
            PerformRaycast();
        }

        private void OnSpherePositionUpdated(Axis axis, float value)
        {
            float clampedValue = Mathf.Clamp(value, minMaxValue.x, minMaxValue.y);

            UpdateSpherePosition(axis, clampedValue);
            PerformRaycast();
        }

        private void Init()
        {
            AddListeners();

            InitPositions();
            InitPoint();
            InitSphere();
            InitInputFields();
            InitRanges();

            PerformRaycast(0);
        }

        private void InitPositions()
        {
            _pointPosition = pointInitialPosition;
            _spherePosition = sphereInitialPosition;
        }

        private void InitPoint()
        {
            point.LookAt(_pointPosition);
        }

        private void InitSphere()
        {
            raycastSphere.transform.position = _spherePosition;
        }

        private void InitInputFields()
        {
            _pointPositionValues = GetComponentInChildren<PointPositionValues>(true);
            _pointPositionValues.PositionUpdated += OnPointPositionUpdated;
            _pointPositionValues.UpdateFields(pointInitialPosition);

            _spherePositionValues = GetComponentInChildren<SpherePositionValues>(true);
            _spherePositionValues.PositionUpdated += OnSpherePositionUpdated;
            _spherePositionValues.UpdateFields(sphereInitialPosition);
        }

        private void InitRanges()
        {
            scaleRange.Init();
        }

        private void UpdatePointPosition(Axis axis, float value)
        {
            Vector3 position = _pointPosition;

            switch (axis)
            {
                case Axis.X:
                    position.x = value;
                    break;
                case Axis.Y:
                    position.y = value;
                    break;
                case Axis.Z:
                    position.z = value;
                    break;
            }

            SetPointPosition(position);
        }

        private void UpdateSpherePosition(Axis axis, float value)
        {
            Vector3 position = _spherePosition;

            switch (axis)
            {
                case Axis.X:
                    position.x = value;
                    break;
                case Axis.Y:
                    position.y = value;
                    break;
                case Axis.Z:
                    position.z = value;
                    break;
            }

            SetSpherePosition(position);
        }

        private void UpdateSphereScale(float scale)
        {
            raycastSphere.localScale = Vector3.one * scale;
        }

        private void PerformRaycast(int waitFramesCount = 5)
        {
            Vector3 origin = Vector3.zero;
            Vector3 direction = _pointPosition.normalized;

            Observable
                .TimerFrame(waitFramesCount)
                .Subscribe(_ =>
                {
                    if (Physics.Raycast(origin, direction, out RaycastHit hit))
                    {
                        Vector3 hitPosition = hit.point;
                        Vector3 normal = hit.normal;
                        Vector3 right = Utils.CrossProduct(normal, direction).normalized;
                        Vector3 forward = Utils.CrossProduct(right, normal);

                        bool reversed = Utils.DotProduct(normal, Vector3.up, Vector3.zero) < 0;

                        SetPointPosition(hitPosition);
                        ToggleHit(true, hitPosition, normal, reversed);
                        UpdateResults(right, forward);
                    }
                    else
                    {
                        ToggleHit(false, Vector3.zero, Vector3.zero, false);
                        UpdateResults(Vector3.zero, Vector3.zero);
                    }
                });
        }

        private void UpdateResults(Vector3 right, Vector3 forward)
        {
            rightVectorCrossProductResult.text = "<color=#FF0000>Cross Product<color=#FFF>\n(" + $"{right.x:F}, {right.y:F}, {right.z:F}" + ") (XYZ)";
            forwardVectorCrossProductResult.text = "<color=#0000FF>Cross Product<color=#FFF>\n(" + $"{forward.x:F}, {forward.y:F}, {forward.z:F}" + ") (XYZ)";
        }

        private void SetPointPosition(Vector3 position)
        {
            _pointPosition = position;

            point.LookAt(position);

            _pointPositionValues.UpdateFields(position);
        }

        private void SetSpherePosition(Vector3 position)
        {
            _spherePosition = position;

            raycastSphere.transform.position = position;

            _spherePositionValues.UpdateFields(position);
        }

        private void ToggleHit(bool value, Vector3 hitPosition, Vector3 normal, bool reversed)
        {
            point.TogglePointer(value);

            Transform raycastCubeTransform = raycastCube.transform;

            if (value)
            {
                raycastCubeTransform.position = hitPosition;

                Vector3 spherePosition = _spherePosition;

                spherePosition.y = 0;

                float angleBetween = Vector3.SignedAngle(Vector3.forward, spherePosition, Vector3.up);

                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);

                rotation *= Quaternion.Euler(0f, angleBetween, 0f);

                if (reversed) rotation *= Quaternion.Euler(0f, 180f, 0f);

                raycastCubeTransform.rotation = rotation;
            }
            else
            {
                raycastCubeTransform.position = Vector3.zero;
                raycastCubeTransform.forward = Vector3.forward;
            }

            raycastCube.gameObject.SetActive(value);
        }

        private void ToggleControlPanel()
        {
            if (!controlPanelTab.IsStable) return;

            controlPanelTab.Toggle(!controlPanelTab.IsVisible);
        }

        private void AddListeners()
        {
            scaleRange.ValueChanged += OnScaleRangeUpdated;
        }

        private void RemoveListeners()
        {
            scaleRange.ValueChanged -= OnScaleRangeUpdated;
        }

        #endregion
    }
}