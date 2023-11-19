using Core.Cameras;
using Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using UniRx;

namespace Core.UI
{
    public class CoordinatesView : MonoBehaviour
    {
        #region SERIALIZED_VARIABLES

        [Header("Prefabs")]
        [SerializeField] private Coordinate coordinatePrefab;

        [Header("References")]
        [SerializeField] private Transform coordinatesContainer;

        [Header("Settings")]
        [SerializeField] [NaughtyAttributes.MinMaxSlider(1, 100)] private Vector2 coordinateRange;

        #endregion

        #region PRIVATE_VARIABLES

        private CameraController _cameraController;

        private IDisposable _coordinateUpdateDisposable;
        private Dictionary<Coordinate, Vector3> _coordinatePositionPairsDictionary;

        #endregion

        #region ZENJECT

        [Inject]
        private void Constructor(CameraController cameraController)
        {
            _cameraController = cameraController;
        }

        #endregion

        #region MONO

        private void Start()
        {
            Init();
        }

        private void OnDisable()
        {
            StopCoordinateUpdate();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void Init()
        {
            InitCoordinates();
            StartCoordinateUpdate();
        }

        private void InitCoordinates()
        {
            _coordinatePositionPairsDictionary = new Dictionary<Coordinate, Vector3>();

            int start = -(int)coordinateRange.y;
            int end = (int)coordinateRange.y;

            AddCoordinate(Vector3.zero, PointPositionAxis.Unknown);

            for (int i = start; i <= end; i++)
            {
                if (i == 0) continue;

                AddCoordinate(new Vector3(i, 0, 0), PointPositionAxis.X);
                AddCoordinate(new Vector3(0, i, 0), PointPositionAxis.Y);
                AddCoordinate(new Vector3(0, 0, i), PointPositionAxis.Z);
            }
        }

        private void AddCoordinate(Vector3 position, PointPositionAxis axis)
        {
            Coordinate coordinate = Instantiate(coordinatePrefab, coordinatesContainer);

            switch (axis)
            {
                case PointPositionAxis.Unknown:
                    coordinate.UpdateCoordinate(0);
                    break;
                case PointPositionAxis.X:
                    coordinate.UpdateCoordinate((int)position.x);
                    break;
                case PointPositionAxis.Y:
                    coordinate.UpdateCoordinate((int)position.y);
                    break;
                case PointPositionAxis.Z:
                    coordinate.UpdateCoordinate((int)position.z);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
            }

            _coordinatePositionPairsDictionary.Add(coordinate, position);
        }

        private void StartCoordinateUpdate()
        {
            _coordinateUpdateDisposable = Observable
                .EveryUpdate()
                .Repeat()
                .Subscribe(_ =>
                {
                    UpdateCoordinates();
                    UpdateUIOrder();
                });
        }

        private void StopCoordinateUpdate()
        {
            _coordinateUpdateDisposable?.Dispose();
            _coordinateUpdateDisposable = null;
        }

        private void UpdateCoordinates()
        {
            foreach (var (coordinate, position) in _coordinatePositionPairsDictionary)
            {
                UpdateCoordinatePosition(coordinate, position);
                UpdateCoordinateScale(coordinate);
            }
        }

        private void UpdateCoordinatePosition(Coordinate coordinate, Vector3 worldPosition)
        {
            Camera outputCamera = _cameraController.Brain.OutputCamera;

            if (Utils.IsInFrontOfCamera(worldPosition, outputCamera))
            {
                Vector2 screenPosition = outputCamera.WorldToScreenPoint(worldPosition);

                coordinate.transform.position = screenPosition;
                coordinate.gameObject.SetActive(true);
            }
            else
            {
                coordinate.gameObject.SetActive(false);
            }
        }

        private void UpdateCoordinateScale(Coordinate coordinate)
        {
            Vector3 cameraPosition = _cameraController.transform.position;
            float distanceFromCenter = Vector3.Distance(cameraPosition, Vector3.zero);
            float scale = 1 - Utils.Map(distanceFromCenter, 0, 100, 0, 1);

            coordinate.transform.localScale = Vector3.one * scale;
        }

        private void UpdateUIOrder()
        {
            Vector3 cameraPosition = _cameraController.transform.position;

            List<Coordinate> coordinates = _coordinatePositionPairsDictionary.Keys.ToList();

            // Sort the UI elements based on distance in ascending order (closest first)
            coordinates
                .Sort((a, b) =>
                    Vector3.Distance(b.transform.position, cameraPosition)
                        .CompareTo(Vector3.Distance(a.transform.position, cameraPosition)));

            for (int i = 0; i < coordinates.Count; i++)
            {
                // Update the order or position of the UI elements based on the sorted order
                // For example, you might set the RectTransform's siblingIndex
                coordinates[i].transform.SetSiblingIndex(i);
            }
        }

        #endregion
    }
}