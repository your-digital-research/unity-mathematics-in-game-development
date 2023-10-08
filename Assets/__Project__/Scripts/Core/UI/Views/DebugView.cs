using Core.Managers;
using UnityEngine;
using Zenject;
using TMPro;

namespace Core.UI
{
    public class DebugView : MonoBehaviour
    {
        #region SERIALIZED_VARIABLES

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

        private void UpdateFPSText(int fps)
        {
            fpsCounter.text = $"FPS : {fps}";
        }

        #endregion
    }
}