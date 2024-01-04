using System;

namespace Core.UI
{
    public class SettingsTab : ScrollableTab
    {
        #region OVERRIDDEN_FUNCTIONS

        public override void Toggle(bool value, Action onComplete = null)
        {
            //
        }

        protected override void Init()
        {
            base.Init();

            IsVisible = false;
        }

        #endregion
    }
}