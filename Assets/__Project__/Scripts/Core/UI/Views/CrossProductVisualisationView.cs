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
    public class CrossProductVisualisationView : MonoBehaviour
    {
        #region SERIALIZED_VARIABLES

        [Header("References")]
        [SerializeField] private ControlPanelTab controlPanelTab;
        [SerializeField] private TextMeshProUGUI result;

        [Header("Settings")]
        [SerializeField] [NaughtyAttributes.MinMaxSlider(-50, 50)] private Vector2 minMaxValue;
        [SerializeField] private List<Vector3> initialPositions;

        [Header("Points")]
        [SerializeField] private List<Point> points;

        #endregion

        #region PRIVATE_VARIABLES

        private TransitionView _transitionView;

        private List<Vector3> _pointsPositions;
        private List<PointPositionValues> _pointsPositionValues;

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

        #endregion

        #region PUBLIC_FUNCTIONS

        public void OnControlPanelButtonClick()
        {
            ToggleControlPanel();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void OnPointPositionUpdated(int pointIndex, Axis axis, float value)
        {
            float clampedValue = Mathf.Clamp(value, minMaxValue.x, minMaxValue.y);

            UpdatePoint(pointIndex, axis, clampedValue);
            UpdateValues(pointIndex);
            UpdateResult();
        }

        private void Init()
        {
            InitPositions();
            InitPoints();
            InitInputFields();

            UpdateResult();
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
            _pointsPositionValues = GetComponentsInChildren<PointPositionValues>(true).ToList();

            foreach (PointPositionValues pointValue in _pointsPositionValues)
            {
                pointValue.PositionUpdated += OnPointPositionUpdated;
                pointValue.UpdateFields(initialPositions[pointValue.Index]);
            }
        }

        private void UpdatePoint(int pointIndex, Axis axis, float value)
        {
            Vector3 position = _pointsPositions[pointIndex];

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

            _pointsPositions[pointIndex] = position;

            points.Find(point => point.Index == pointIndex).LookAt(position);
        }

        private void UpdateValues(int pointIndex)
        {
            _pointsPositionValues[pointIndex].UpdateFields(_pointsPositions[pointIndex]);
        }

        private void UpdateResult()
        {
            Point lastPoint = points[^1];

            Vector3 firstPointPosition = _pointsPositions[0];
            Vector3 secondPointPosition = _pointsPositions[1];

            Vector3 dotProduct = Utils.CrossProductViaMatrix(firstPointPosition, secondPointPosition);

            _pointsPositions[^1] = dotProduct;

            lastPoint.LookAt(dotProduct);

            result.text = "Cross Product : (" + $"{dotProduct.x}, {dotProduct.y}, {dotProduct.z}" + ") (XYZ)";
        }

        private void ToggleControlPanel()
        {
            if (!controlPanelTab.IsStable) return;

            controlPanelTab.Toggle(!controlPanelTab.IsVisible);
        }

        #endregion
    }
}