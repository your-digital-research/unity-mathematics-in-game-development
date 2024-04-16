using UnityEngine;

namespace Core.UI
{
    public class DotProductRange : SliderRange
    {
        #region SERIALIZED_VARIABLES

        [Header("Settings")]
        [SerializeField] [Range(-1, 1)] private float initialValue;

        #endregion

        #region OVERRIDDEN_FUNCTIONS

        public override void Init()
        {
            InitValue();

            base.Init();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void InitValue()
        {
            Slider.value = initialValue;

            UpdateText(initialValue);

            ValueChanged?.Invoke(initialValue);
        }

        #endregion
    }
}