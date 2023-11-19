using TMPro;
using UnityEngine;

namespace Core.UI
{
    public class Coordinate : MonoBehaviour
    {
        #region SERIALIZED_VARIABLES

        [Header("References")]
        [SerializeField] private TextMeshProUGUI coordinate;

        #endregion

        #region PUBLIC_FUNCTIONS

        public void UpdateCoordinate(int newCoordinate)
        {
            coordinate.text = $"{newCoordinate}";
        }

        #endregion
    }
}