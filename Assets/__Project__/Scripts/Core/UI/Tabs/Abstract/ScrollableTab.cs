using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public abstract class ScrollableTab : Tab
    {
        #region SERIALIZED_VARIABLES

        [Header("Settings")]
        [SerializeField] private ScrollRect scrollRect;

        #endregion

        #region OVERRIDDEN_FUNCTIONS

        protected override void Init()
        {
            InitScroll();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void InitScroll()
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }

        #endregion
    }
}