using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Core.UI
{
    public class TransitionView : MonoBehaviour
    {
        #region SERIALIZED_VARIABLES

        [Header("References")]
        [SerializeField] private Image background;
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Settings")]
        [SerializeField] [Range(0, 5)] private float transitionSpeed;

        #endregion

        #region MONO

        private void Start()
        {
            Init();
        }

        #endregion

        #region PUBLIC_FUNCTIONS

        public void Hide(Action onComplete = null)
        {
            canvasGroup
                .DOFade(0, transitionSpeed)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    background.raycastTarget = false;
                    onComplete?.Invoke();
                });
        }

        public void Show(Action onComplete = null)
        {
            canvasGroup
                .DOFade(1, transitionSpeed)
                .SetEase(Ease.Linear)
                .OnStart(() => background.raycastTarget = true)
                .OnComplete(() => onComplete?.Invoke());
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void Init()
        {
            InitImage();
            InitCanvas();
        }

        private void InitImage()
        {
            background.raycastTarget = true;
        }

        private void InitCanvas()
        {
            canvasGroup.alpha = 1;
        }

        #endregion
    }
}