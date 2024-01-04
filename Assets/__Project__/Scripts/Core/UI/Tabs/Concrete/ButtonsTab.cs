using System;

namespace Core.UI
{
    public class ButtonsTab : Tab
    {
        #region OVERRIDDEN_FUNCTIONS

        public override void Toggle(bool value, bool force = false, Action onComplete = null)
        {
            gameObject.SetActive(value);
        }

        protected override void Init()
        {
            IsVisible = true;
        }

        #endregion
    }
}