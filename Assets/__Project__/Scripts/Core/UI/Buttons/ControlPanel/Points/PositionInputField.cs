using System;
using UnityEngine;
using TMPro;

namespace Core.UI
{
    public class PositionInputField : MonoBehaviour
    {
        #region SERIALIZED_VARIABLES

        [Header("Settings")]
        [SerializeField] private PointPositionAxis pointPositionAxis;

        #endregion

        #region PRIVATE_VARIABLES

        private TMP_InputField _inputField;

        #endregion

        #region PROPERTIES

        public PointPositionAxis PositionAxis => pointPositionAxis;

        #endregion

        #region EVENTS

        public Action<PointPositionAxis, float> PositionChanged;

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
            _inputField.text = $"{value}";
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void OnEndEdit(string value)
        {
            PositionChanged?.Invoke(pointPositionAxis, float.Parse(value));
        }

        #endregion
    }
}