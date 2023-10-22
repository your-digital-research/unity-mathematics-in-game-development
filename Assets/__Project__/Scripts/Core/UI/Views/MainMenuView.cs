using Core.Managers;
using Core.Loaders;
using System.Collections.Generic;
using System.Linq;
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

        [Header("Example Buttons")]
        [SerializeField] [NaughtyAttributes.ReadOnly] private List<ExampleButton> exampleButtons;

        #endregion

        #region PRIVATE_VARIABLES

        private GameManager _gameManager;
        private SceneLoader _sceneLoader;

        #endregion

        #region ZENJECT

        [Inject]
        private void Constructor(GameManager gameManager, SceneLoader sceneLoader)
        {
            _gameManager = gameManager;
            _sceneLoader = sceneLoader;
        }

        #endregion

        #region MONO

        private void Start()
        {
            Init();
        }

        private void OnDisable()
        {
            RemoveListeners();
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

        private void OnExampleButtonClicked(ExampleScene exampleScene)
        {
            _sceneLoader.LoadExampleScene(exampleScene);
        }

        private void Init()
        {
            InitButtons();

            AddListeners();

            ToggleButtonsTab(true);
            ToggleExamplesTab(false);
            ToggleSettingsTab(false);
        }

        private void InitButtons()
        {
            exampleButtons = GetComponentsInChildren<ExampleButton>(true).ToList();
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

        private void AddListeners()
        {
            exampleButtons.ForEach(button => button.ExampleButtonClicked += OnExampleButtonClicked);
        }

        private void RemoveListeners()
        {
            exampleButtons.ForEach(button => button.ExampleButtonClicked -= OnExampleButtonClicked);
        }

        #endregion
    }
}