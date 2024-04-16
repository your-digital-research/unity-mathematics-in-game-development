using Core.Types;
using System;

namespace Core.UI
{
    public class SpherePositionValues : ObjectValues
    {
        #region EVENTS

        public Action<Axis, float> PositionUpdated;

        #endregion

        #region OVERRIDDEN_FUNCTIONS

        protected override void OnInputFieldValueChanged(Axis axis, float value)
        {
            PositionUpdated?.Invoke(axis, value);
        }

        #endregion
    }
}