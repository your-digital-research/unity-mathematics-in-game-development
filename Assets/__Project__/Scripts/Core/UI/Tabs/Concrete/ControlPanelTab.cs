using System;
using UnityEngine;
using DG.Tweening;

namespace Core.UI
{
    public class ControlPanelTab : ScrollableTab
    {
        #region SERIALIZED_VARIABLES

        [Header("References")]
        [SerializeField] private RectTransform selfRectTransform;

        [Header("Settings")]
        [SerializeField] [Range(0, 5)] private float showHideDuration;
        [SerializeField] [Range(0, 25)] private float offsetForBorders;

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

            IsVisible = true;
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void Show(bool force, Action onComplete = null)
        {
            float height = selfRectTransform.rect.height;
            float endY = 0;

            Vector2 startPosition = new Vector3(0, -height - offsetForBorders);
            Vector2 endPosition = new Vector2(0, endY);

            selfRectTransform.anchoredPosition = startPosition;

            if (force)
            {
                // TODO - Implement if case of need
            }
            else
            {
                selfRectTransform
                    .DOMoveY(0, showHideDuration)
                    .SetEase(Ease.OutBack)
                    .OnStart(() => gameObject.SetActive(true))
                    .OnComplete(() =>
                    {
                        IsVisible = true;
                        IsStable = true;

                        onComplete?.Invoke();
                    });
            }
        }

        private void Hide(bool force, Action onComplete = null)
        {
            float height = selfRectTransform.rect.height;
            float endY = -height - offsetForBorders;

            Vector2 startPosition = new Vector2(0, 0);
            Vector2 endPosition = new Vector2(0, endY);

            selfRectTransform.anchoredPosition = startPosition;

            if (force)
            {
                // TODO - Implement if case of need
            }
            else
            {
                selfRectTransform
                    .DOAnchorPosY(endY, showHideDuration)
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