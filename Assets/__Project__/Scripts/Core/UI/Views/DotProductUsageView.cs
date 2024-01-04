using Core.Misc;
using Core.Types;
using Core.Utilities;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using TMPro;
using UniRx;

namespace Core.UI
{
    public class DotProductUsageView : MonoBehaviour
    {
        #region SERIALIZED_VARIABLES

        [Header("References")]
        [SerializeField] private ControlPanelTab controlPanelTab;
        [SerializeField] private TextMeshProUGUI result;

        [Header("Settings")]
        [SerializeField] [NaughtyAttributes.MinMaxSlider(-50, 50)] private Vector2 minMaxValue;
        [SerializeField] private List<Vector3> initialPositions;

        [Header("Field Of View")]
        [SerializeField] private FieldOfView fieldOfView;

        [Header("Ranges")]
        [SerializeField] private DotProductRange dotProductRange;
        [SerializeField] private AngleRange angleRange;

        [Header("Points")]
        [SerializeField] private List<Point> points;

        #endregion

        #region PRIVATE_VARIABLES

        private TransitionView _transitionView;

        private List<Vector3> _pointsPositions;
        private List<PointValues> _pointValues;

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
            Observable.Timer(TimeSpan.FromSeconds(Constant.InitDelay)).Subscribe(_ =>
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

        private void OnAngleRangeChanged(float angle)
        {
            UpdateRangesAfterAngleRangeChanged(angle);
            CheckForTrigger();
        }

        private void OnDotProductRangeChanged(float dotProduct)
        {
            UpdateRangesAfterDotProductRangeChanged(dotProduct);
            CheckForTrigger();
        }

        private void OnPointPositionUpdated(int pointIndex, PointPositionAxis axis, float value)
        {
            float clampedValue = Mathf.Clamp(value, minMaxValue.x, minMaxValue.y);

            UpdatePoint(pointIndex, axis, clampedValue);
            UpdateValues(pointIndex);
            UpdateResult();
            CheckForTrigger();

            if (pointIndex == 0) UpdateFieldOfView();
        }

        private void Init()
        {
            AddListeners();

            InitPositions();
            InitPoints();
            InitInputFields();
            InitRanges();

            UpdateResult();
            UpdateFieldOfView();
        }

        private void InitPositions()
        {
            _pointsPositions = new List<Vector3>();

            initialPositions.ForEach(position => _pointsPositions.Add(position));
        }

        private void InitPoints()
        {
            points.ForEach(point => point.LookAt(_pointsPositions[point.Index]));
        }

        private void InitInputFields()
        {
            _pointValues = GetComponentsInChildren<PointValues>(true).ToList();

            foreach (PointValues pointValue in _pointValues)
            {
                pointValue.PositionUpdated += OnPointPositionUpdated;
                pointValue.UpdateFields(initialPositions[pointValue.Index]);
            }
        }

        private void InitRanges()
        {
            dotProductRange.Init();
            angleRange.Init();
        }

        private void UpdatePoint(int pointIndex, PointPositionAxis axis, float value)
        {
            Vector3 position = _pointsPositions[pointIndex];

            switch (axis)
            {
                case PointPositionAxis.X:
                    position.x = value;
                    break;
                case PointPositionAxis.Y:
                    position.y = value;
                    break;
                case PointPositionAxis.Z:
                    position.z = value;
                    break;
            }

            _pointsPositions[pointIndex] = position;

            points.Find(point => point.Index == pointIndex).LookAt(position);
        }

        private void UpdateValues(int pointIndex)
        {
            _pointValues[pointIndex].UpdateFields(_pointsPositions[pointIndex]);
        }

        private void UpdateResult()
        {
            Vector3 firstPointPosition = _pointsPositions[0];
            Vector3 secondPointPosition = _pointsPositions[1];

            float dotProduct = Utils.DotProduct(firstPointPosition, secondPointPosition, Vector3.zero);

            result.text = $"Dot Product : {dotProduct:F}";
        }

        private void UpdateFieldOfView()
        {
            Vector3 firstPointPosition = _pointsPositions[0];

            fieldOfView.ViewRadius = firstPointPosition.magnitude;
        }

        private void UpdateRangesAfterAngleRangeChanged(float angle)
        {
            fieldOfView.ViewAngle = angle;

            float dotProduct = Utils.DegreesToDotProduct(angle / 2);

            dotProductRange.UpdateValue(dotProduct);
        }

        private void UpdateRangesAfterDotProductRangeChanged(float dotProduct)
        {
            float angle = Utils.DotProductToDegreesHalfCircle(dotProduct) * 2;

            fieldOfView.ViewAngle = angle;

            angleRange.UpdateValue(angle);
        }

        private void CheckForTrigger()
        {
            Vector3 firstPointPosition = _pointsPositions[0];
            Vector3 secondPointPosition = _pointsPositions[1];

            float dotProduct = Utils.DotProduct(firstPointPosition, secondPointPosition, Vector3.zero);

            fieldOfView.ToggleTrigger(dotProduct >= dotProductRange.GetCurrentValue());
        }

        private void ToggleControlPanel()
        {
            controlPanelTab.gameObject.SetActive(!controlPanelTab.gameObject.activeSelf);
        }

        private void AddListeners()
        {
            angleRange.ValueChanged += OnAngleRangeChanged;
            dotProductRange.ValueChanged += OnDotProductRangeChanged;
        }

        private void RemoveListeners()
        {
            angleRange.ValueChanged -= OnAngleRangeChanged;
            dotProductRange.ValueChanged -= OnDotProductRangeChanged;
        }

        #endregion
    }
}