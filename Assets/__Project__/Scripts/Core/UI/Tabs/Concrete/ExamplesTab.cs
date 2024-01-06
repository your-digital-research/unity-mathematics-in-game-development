using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Core.UI
{
    public class ExamplesTab : ScrollableTab
    {
        #region SERIALIZED_VARIABLES

        [Header("Settings")]
        [SerializeField] [Range(0, 5)] private float showHideDuration;

        [Header("References")]
        [SerializeField] private Image blocker;

        #endregion

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
            base.Init();

            IsVisible = false;
            blocker.raycastTarget = true;
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void Show(bool force = false, Action onComplete = null)
        {
            transform.localScale = Vector3.zero;

            if (force)
            {
                // TODO - Implement if case of need
            }
            else
            {
                transform
                    .DOScale(Vector3.one, showHideDuration)
                    .SetEase(Ease.OutBack)
                    .OnStart(() => gameObject.SetActive(true))
                    .OnComplete(() =>
                    {
                        IsVisible = true;
                        IsStable = true;

                        blocker.raycastTarget = false;

                        onComplete?.Invoke();
                    });
            }
        }

        private void Hide(bool force = false, Action onComplete = null)
        {
            blocker.raycastTarget = true;

            transform.localScale = Vector3.one;

            if (force)
            {
                // TODO - Implement if case of need
            }
            else
            {
                transform
                    .DOScale(Vector3.zero, showHideDuration)
                    .SetEase(Ease.InBack)
                    .OnComplete(() =>
                    {
                        IsVisible = false;
                        IsStable = true;

                        gameObject.SetActive(false);

                        onComplete?.Invoke();
                    });
            }
        }

        #endregion
    }
}