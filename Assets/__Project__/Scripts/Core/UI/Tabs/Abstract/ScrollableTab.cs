using UnityEngine;
using UnityEngine.UI;

public class ScrollableTab : MonoBehaviour
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
        InitScroll();
    }

    private void InitScroll()
    {
        scrollRect.verticalNormalizedPosition = 1f;
    }

    #endregion
}