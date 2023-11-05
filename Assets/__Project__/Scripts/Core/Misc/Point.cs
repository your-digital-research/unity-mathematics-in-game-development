using UnityEngine;

namespace Core.Misc
{
    public class Point : MonoBehaviour
    {
        #region SERIALIZED_VARIABLES

        [Header("Settings")]
        [SerializeField] private int index;

        [Header("References")]
        [SerializeField] private GameObject cone;
        [SerializeField] private GameObject cylinder;

        #endregion

        #region PROPERTIES

        public int Index => index;

        #endregion

        #region PUBLIC_FUNCTIONS

        public void LookAt(Vector3 position)
        {
            transform.LookAt(position);

            float distance = (position - transform.position).magnitude;
            float shiftValue = distance - 1;

            if (shiftValue < 1) shiftValue = 1;

            Vector3 conePosition = cone.transform.localPosition;
            conePosition.z = shiftValue;
            cone.transform.localPosition = conePosition;

            Vector3 cylinderScale = cylinder.transform.localScale;
            cylinderScale.z = shiftValue;
            cylinder.transform.localScale = cylinderScale;
        }

        #endregion
    }
}