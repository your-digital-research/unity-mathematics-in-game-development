using Core.Types;
using System;
using UnityEngine;

namespace Core.UI
{
    public class PointPositionValues : ObjectValues
    {
        #region SERIALIZED_VARIABLES

        [Header("Settings")]
        [SerializeField] private int pointIndex;

        #endregion

        #region PROPERTIES

        public int Index => pointIndex;

        #endregion

        #region EVENTS

        public Action<int, Axis, float> PositionUpdated;

        #endregion

        #region OVERRIDDEN_FUNCTIONS

        protected override void OnInputFieldValueChanged(Axis axis, float value)
        {
            PositionUpdated?.Invoke(pointIndex, axis, value);
        }

        #endregion
    }
}