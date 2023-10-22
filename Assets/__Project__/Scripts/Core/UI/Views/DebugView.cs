using Core.Managers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

namespace Core.UI
{
    public class DebugView : MonoBehaviour
    {
        #region SERIALIZED_VARIABLES

        [Header("References")]
        [SerializeField] private GameObject settings;
        [SerializeField] private GameObject background;

        [Header("Button")]
        [SerializeField] private Button closeButton;
        [SerializeField] private Button settingsButton;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI fpsCounter;

        #endregion

        #region PRIVATE_VARIABLES

        private DebugManager _debugManager;

        #endregion

        #region ZENJECT

        [Inject]
        private void Constructor(DebugManager debugManager)
        {
            _debugManager = debugManager;
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

        public void OnDebugButtonClick()
        {
            ToggleView(true);
        }

        public void OnCloseButtonClick()
        {
            ToggleView(false);
        }

        public void OnReloadButtonClick()
        {
            _debugManager.Reload();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void OnFPSUpdated(int fps)
        {
            UpdateFPSText(fps);
        }

        private void Init()
        {
            AddListeners();
        }

        private void AddListeners()
        {
            _debugManager.FPSUpdated += OnFPSUpdated;
        }

        private void RemoveListeners()
        {
            _debugManager.FPSUpdated -= OnFPSUpdated;
        }

        private void ToggleView(bool value)
        {
            background.gameObject.SetActive(value);
            settingsButton.gameObject.SetActive(!value);
            closeButton.gameObject.SetActive(value);
            settings.gameObject.SetActive(value);
        }

        private void UpdateFPSText(int fps)
        {
            fpsCounter.text = $"FPS : {fps}";
        }

        #endregion
    }
}