using Core.Misc;
using Core.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace Core.UI
{
    public class CrossProductView : MonoBehaviour
    {
        #region SERIALIZED_VARIABLES

        [Header("References")]
        [SerializeField] private GameObject controlPanel;
        [SerializeField] private TextMeshProUGUI result;

        [Header("Settings")]
        [SerializeField] [NaughtyAttributes.MinMaxSlider(-50, 50)] private Vector2 minMaxValue;
        [SerializeField] private List<Vector3> initialPositions;

        [Header("Points")]
        [SerializeField] [NaughtyAttributes.ReadOnly] private List<Point> points;

        #endregion

        #region PRIVATE_VARIABLES

        private List<Vector3> _pointsPositions;
        private List<PointValues> _pointValues;

        #endregion

        #region MONO

        private void Awake()
        {
            FindPoints();
        }

        private void Start()
        {
            Init();
        }

        #endregion

        #region PUBLIC_FUNCTIONS

        public void OnControlPanelButtonClick()
        {
            ToggleControlPanel();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void OnPointPositionUpdated(int pointIndex, PointPositionAxis axis, float value)
        {
            float clampedValue = Mathf.Clamp(value, minMaxValue.x, minMaxValue.y);

            UpdatePoint(pointIndex, axis, clampedValue);
            UpdateValues(pointIndex);
            UpdateResult();
        }

        private void FindPoints()
        {
            points = FindObjectsOfType<Point>(true).ToList();
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
            _pointValues = GetComponentsInChildren<PointValues>(true).ToList();

            foreach (PointValues pointValue in _pointValues)
            {
                pointValue.PositionUpdated += OnPointPositionUpdated;
                pointValue.UpdateFields(initialPositions[pointValue.Index]);
            }
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
            Point lastPoint = points[^1];

            Vector3 firstPointPosition = _pointsPositions[0];
            Vector3 secondPointPosition = _pointsPositions[1];

            Vector3 dotProduct = Utils.CrossProductViaMatrix(firstPointPosition, secondPointPosition);

            _pointsPositions[^1] = dotProduct;

            lastPoint.LookAt(dotProduct);

            result.text = "Result Cross Product : {" + $"{dotProduct.x}, {dotProduct.y}, {dotProduct.z}" + "} (XYZ)";
        }

        private void ToggleControlPanel()
        {
            controlPanel.SetActive(!controlPanel.activeSelf);
        }

        #endregion
    }
}