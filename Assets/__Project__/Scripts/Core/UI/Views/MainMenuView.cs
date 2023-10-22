using Core.Managers;
using UnityEngine;
using Zenject;

namespace Core.UI
{
    public class MainMenuView : MonoBehaviour
    {
        #region SERIALIZED_VARIABLES

        [Header("References")]
        [SerializeField] private GameObject buttons;
        [SerializeField] private GameObject examples;
        [SerializeField] private GameObject settings;

        #endregion

        #region PRIVATE_VARIABLES

        private GameManager _gameManager;

        #endregion

        #region ZENJECT

        [Inject]
        private void Constructor(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        #endregion

        #region MONO

        private void Start()
        {
            Init();
        }

        #endregion

        #region PUBLIC_FUNCTIONS

        public void OnExamplesButtonClick()
        {
            ToggleButtonsTab(false);
            ToggleExamplesTab(true);
        }

        public void OnExamplesCloseButtonClick()
        {
            ToggleButtonsTab(true);
            ToggleExamplesTab(false);
        }

        public void OnSettingsButtonClick()
        {
            ToggleButtonsTab(false);
            ToggleSettingsTab(true);
        }

        public void OnSettingsCloseButtonClick()
        {
            ToggleButtonsTab(true);
            ToggleSettingsTab(false);
        }

        public void OnQuitButtonClick()
        {
            _gameManager.QuitApp();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void Init()
        {
            ToggleButtonsTab(true);
            ToggleExamplesTab(false);
            ToggleSettingsTab(false);
        }

        private void ToggleButtonsTab(bool value)
        {
            buttons.SetActive(value);
        }

        private void ToggleExamplesTab(bool value)
        {
            examples.SetActive(value);
        }

        private void ToggleSettingsTab(bool value)
        {
            settings.SetActive(value);
        }

        #endregion
    }
}