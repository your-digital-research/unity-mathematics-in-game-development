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
        [SerializeField] private GameObject sphere;
        [SerializeField] private GameObject cylinder;
        [SerializeField] private GameObject pointPosition;

        #endregion

        #region PROPERTIES

        public int Index => index;

        #endregion

        #region PUBLIC_FUNCTIONS

        public void LookAt(Vector3 position)
        {
            transform.LookAt(position);
            gameObject.SetActive(position.magnitude > 0);

            float distance = (position - transform.position).magnitude;
            float shiftValue = distance - 1;

            Vector3 conePosition = cone.transform.localPosition;
            conePosition.z = shiftValue;
            cone.transform.localPosition = conePosition;
            cone.gameObject.SetActive(position.magnitude > 0.75f);

            Vector3 cylinderScale = cylinder.transform.localScale;
            cylinderScale.z = shiftValue;
            cylinder.transform.localScale = cylinderScale;
            cylinder.gameObject.SetActive(position.magnitude > 0.75f);

            sphere.transform.position = pointPosition.transform.position;
        }

        #endregion
    }
}