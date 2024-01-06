using System;

namespace Core.UI
{
    public class ButtonsTab : Tab
    {
        #region OVERRIDDEN_FUNCTIONS

        public override void Toggle(bool value, bool force = false, Action onComplete = null)
        {
            IsStable = false;

            if (value)
            {
                Show(force, onComplete);
            }
            else
            {
                Hide(force, onComplete);
            }
        }

        protected override void Init()
        {
            IsVisible = true;
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void Show(bool force, Action onComplete)
        {
            if (force)
            {
                gameObject.SetActive(true);

                IsVisible = true;
                IsStable = true;

                onComplete?.Invoke();
            }
            else
            {
                // TODO - Implement if case of need
            }
        }

        private void Hide(bool force, Action onComplete)
        {
            if (force)
            {
                gameObject.SetActive(false);

                IsVisible = false;
                IsStable = true;

                onComplete?.Invoke();
            }
            else
            {
                // TODO - Implement if case of need
            }
        }

        #endregion
    }
}