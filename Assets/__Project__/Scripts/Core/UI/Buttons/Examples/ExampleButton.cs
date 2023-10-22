using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class ExampleButton : MonoBehaviour
    {
        #region SERIALIZED_VARIABLES

        [Header("Settings")]
        [SerializeField] private ExampleScene exampleScene;

        #endregion

        #region PRIVATE_VARIABLES

        private Button _button;

        #endregion

        #region EVENTS

        public Action<ExampleScene> ExampleButtonClicked;

        #endregion

        #region MONO

        private void Awake()
        {
            GetComponents();
        }

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

        private void OnButtonClick()
        {
            ExampleButtonClicked?.Invoke(exampleScene);
        }

        private void GetComponents()
        {
            _button = GetComponent<Button>();
        }

        private void Init()
        {
            AddListeners();
        }

        private void AddListeners()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void RemoveListeners()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        #endregion
    }
}