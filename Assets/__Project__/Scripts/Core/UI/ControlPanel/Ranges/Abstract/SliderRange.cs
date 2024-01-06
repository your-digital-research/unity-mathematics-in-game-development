using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Core.UI
{
    public abstract class SliderRange : MonoBehaviour
    {
        #region SERIALIZED_VARIABLES

        [Header("References")]
        [SerializeField] protected Slider Slider;
        [SerializeField] protected TextMeshProUGUI CurrentValue;

        #endregion

        #region EVENTS

        public Action<float> ValueChanged;

        #endregion

        #region MONO

        private void OnDestroy()
        {
            RemoveListeners();
        }

        #endregion

        #region VIRTUAL_FUNCTIONS

        public virtual void Init()
        {
            AddListeners();

            ValueChanged?.Invoke(GetCurrentValue());
        }

        #endregion

        #region PUBLIC_FUNCTIONS

        public float GetCurrentValue()
        {
            return Slider.value;
        }

        #endregion

        #region PROTECTED_FUNCTIONS

        protected void UpdateText(float angle)
        {
            CurrentValue.text = $"{angle:F}";
        }

        #endregion

        #region PUBLIC_FUNCTIONS

        public void UpdateValue(float value)
        {
            RemoveListeners();

            Slider.value = value;

            UpdateText(value);

            AddListeners();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void OnValueChanged(float angle)
        {
            UpdateText(angle);

            ValueChanged?.Invoke(angle);
        }

        private void AddListeners()
        {
            Slider.onValueChanged.AddListener(OnValueChanged);
        }

        private void RemoveListeners()
        {
            Slider.onValueChanged.RemoveListener(OnValueChanged);
        }

        #endregion
    }
}