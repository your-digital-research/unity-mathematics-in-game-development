using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.UI
{
    public class CameraMovementView : MonoBehaviour
    {
        #region SERIALIZED_VARIABLES

        [Header("Movement Buttons")]
        [SerializeField] [NaughtyAttributes.ReadOnly] List<MovementButton> movementButtons;

        [Header("Rotation Buttons")]
        [SerializeField] [NaughtyAttributes.ReadOnly] private List<RotationButton> rotationButtons;

        #endregion

        #region EVENTS

        public Action<MovementDirection, bool> MovementUpdated;
        public Action<RotationDirection, bool> RotationUpdated;

        #endregion

        #region MONO

        private void Start()
        {
            Init();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void OnMovementUpdate(MovementDirection direction, bool toggle)
        {
            MovementUpdated?.Invoke(direction, toggle);
        }

        private void OnRotationUpdate(RotationDirection direction, bool toggle)
        {
            RotationUpdated?.Invoke(direction, toggle);
        }

        private void Init()
        {
            InitButtons();

            AddListeners();
        }

        private void InitButtons()
        {
            movementButtons = GetComponentsInChildren<MovementButton>(true).ToList();
            rotationButtons = GetComponentsInChildren<RotationButton>(true).ToList();
        }

        private void AddListeners()
        {
            movementButtons.ForEach(button =>
            {
                button.PointerDown += OnMovementUpdate;
                button.PointerUp += OnMovementUpdate;
            });

            rotationButtons.ForEach(button =>
            {
                button.PointerDown += OnRotationUpdate;
                button.PointerUp += OnRotationUpdate;
            });
        }

        private void RemoveListeners()
        {
            movementButtons.ForEach(button =>
            {
                button.PointerDown -= OnMovementUpdate;
                button.PointerUp -= OnMovementUpdate;
            });

            rotationButtons.ForEach(button =>
            {
                button.PointerDown -= OnRotationUpdate;
                button.PointerUp -= OnRotationUpdate;
            });
        }

        #endregion
    }
}