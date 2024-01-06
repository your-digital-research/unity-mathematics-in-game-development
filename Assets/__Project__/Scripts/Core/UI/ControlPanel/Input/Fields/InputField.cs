using Core.Types;
using System;
using UnityEngine;
using TMPro;

namespace Core.UI
{
    public class InputField : MonoBehaviour
    {
        #region SERIALIZED_VARIABLES

        [Header("Settings")]
        [SerializeField] private Axis axis;

        #endregion

        #region PRIVATE_VARIABLES

        private TMP_InputField _inputField;

        #endregion

        #region PROPERTIES

        public Axis Axis => axis;

        #endregion

        #region EVENTS

        public Action<Axis, float> ValueChanged;

        #endregion

        #region MONO

        private void Awake()
        {
            GetInputField();
        }

        #endregion

        #region PUBLIC_FUNCTIONS

        private void GetInputField()
        {
            _inputField = GetComponent<TMP_InputField>();
        }

        public void Init()
        {
            _inputField.onEndEdit.AddListener(OnEndEdit);
        }

        public void UpdateField(float value)
        {
            _inputField.text = $"{value:F}";
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void OnEndEdit(string value)
        {
            ValueChanged?.Invoke(axis, float.Parse(value));
        }

        #endregion
    }
}