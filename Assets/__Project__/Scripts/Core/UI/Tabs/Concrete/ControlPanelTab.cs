using System;
using DG.Tweening;
using UnityEngine;

namespace Core.UI
{
    public class ControlPanelTab : ScrollableTab
    {
        #region SERIALIZED_VARIABLES

        [Header("Settings")]
        [SerializeField] [Range(0, 25)] private float offsetForBorders;

        #endregion

        #region OVERRIDDEN_FUNCTIONS

        public override void Toggle(bool value, Action onComplete = null)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            Transform selfTransform = rectTransform.transform;
            Vector3 selfPosition = selfTransform.position;

            float height = rectTransform.rect.height;

            if (value)
            {
                Show(rectTransform, selfTransform, selfPosition, height);
            }
            else
            {
                Hide(rectTransform, selfTransform, selfPosition, height);
            }
        }

        protected override void Init()
        {
            base.Init();

            IsVisible = true;
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void Show(RectTransform rectTransform, Transform selfTransform, Vector3 selfPosition, float height, Action onComplete = null)
        {
            IsStable = false;

            Vector3 newPosition = new Vector3(selfPosition.x, -height - offsetForBorders, selfPosition.z);

            selfTransform.position = newPosition;

            rectTransform
                .DOMoveY(0, 0.5f)
                .SetEase(Ease.OutBack)
                .OnStart(() => gameObject.SetActive(true))
                .OnComplete(() =>
                {
                    IsVisible = true;
                    IsStable = true;

                    onComplete?.Invoke();
                });
        }

        private void Hide(RectTransform rectTransform, Transform selfTransform, Vector3 selfPosition, float height, Action onComplete = null)
        {
            IsStable = false;

            Vector3 newPosition = new Vector3(selfPosition.x, 0, selfPosition.z);

            selfTransform.position = newPosition;

            rectTransform
                .DOAnchorPosY(-height - offsetForBorders, 0.25f)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    IsVisible = false;
                    IsStable = true;

                    gameObject.SetActive(false);

                    onComplete?.Invoke();
                });
        }

        #endregion
    }
}