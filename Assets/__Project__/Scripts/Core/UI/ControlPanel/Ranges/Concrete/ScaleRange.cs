using UnityEngine;

namespace Core.UI
{
    public class ScaleRange : SliderRange
    {
        #region SERIALIZED_VARIABLES

        [Header("Settings")]
        [SerializeField] [Range(1, 5)] private float initialScale;

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
            Slider.value = initialScale;

            UpdateText(initialScale);

            ValueChanged?.Invoke(initialScale);
        }

        #endregion
    }
}