using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class ControlPanel : MonoBehaviour
    {
        #region SERIALIZED_VARIABLES

        [Header("Settings")]
        [SerializeField] private ScrollRect scrollRect;

        #endregion

        #region MONO

        private void Start()
        {
            Init();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void Init()
        {
            InitScrollRect();
        }

        private void InitScrollRect()
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }

        #endregion
    }
}