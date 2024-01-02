using Core.Constants;
using System;
using UnityEngine;
using Zenject;
using UniRx;

namespace Core.UI
{
    public class DotProductUsageView : MonoBehaviour
    {
        #region SERIALIZED_VARIABLES

        [Header("References")]
        [SerializeField] private GameObject controlPanel;

        #endregion

        #region PRIVATE_VARIABLES

        private TransitionView _transitionView;

        #endregion

        #region ZENJECT

        [Inject]
        private void Constructor(TransitionView transitionView)
        {
            _transitionView = transitionView;
        }

        #endregion

        #region MONO

        private void Start()
        {
            Observable.Timer(TimeSpan.FromSeconds(Constant.InitDelay)).Subscribe(_ =>
            {
                _transitionView.Hide();

                Init();
            });
        }

        #endregion

        #region PUBLIC_FUNCTIONS

        public void OnControlPanelButtonClick()
        {
            ToggleControlPanel();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void Init()
        {
            //
        }

        private void ToggleControlPanel()
        {
            controlPanel.SetActive(!controlPanel.activeSelf);
        }

        #endregion
    }
}