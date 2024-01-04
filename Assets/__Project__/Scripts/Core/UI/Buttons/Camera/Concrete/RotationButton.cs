using Core.Types;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.UI
{
    public class RotationButton : CameraButton
    {
        #region SERIALIZED_VARIABLES

        [Header("Settings")]
        [SerializeField] private RotationDirection rotationDirection;

        #endregion

        #region EVENTS

        public Action<RotationDirection, bool> PointerDown;
        public Action<RotationDirection, bool> PointerUp;

        #endregion

        #region PRIVATE_FUNCTIONS

        protected override void OnPointerDown(PointerEventData data)
        {
            PointerDown?.Invoke(rotationDirection, true);
        }

        protected override void OnPointerUp(PointerEventData data)
        {
            PointerUp?.Invoke(rotationDirection, false);
        }

        #endregion
    }
}