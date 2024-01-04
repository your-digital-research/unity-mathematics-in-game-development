using UnityEngine;

namespace Core.UI
{
    public class AngleRange : SliderRange
    {
        #region SERIALIZED_VARIABLES

        [Header("Settings")]
        [SerializeField] [Range(0, 360)] private float initialAngle;

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
            Slider.value = initialAngle;

            UpdateText(initialAngle);

            ValueChanged?.Invoke(initialAngle);
        }

        #endregion
    }
}