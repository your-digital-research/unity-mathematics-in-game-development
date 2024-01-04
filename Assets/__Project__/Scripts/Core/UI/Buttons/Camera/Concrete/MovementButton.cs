using Core.Types;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.UI
{
    public class MovementButton : CameraButton
    {
        #region SERIALIZED_VARIABLES

        [Header("Settings")]
        [SerializeField] private MovementDirection movementDirection;

        #endregion

        #region EVENTS

        public Action<MovementDirection, bool> PointerDown;
        public Action<MovementDirection, bool> PointerUp;

        #endregion

        #region OVERRIDDEN_FUNCTIONS

        protected override void OnPointerDown(PointerEventData data)
        {
            PointerDown?.Invoke(movementDirection, true);
        }

        protected override void OnPointerUp(PointerEventData data)
        {
            PointerUp?.Invoke(movementDirection, false);
        }

        #endregion
    }
}