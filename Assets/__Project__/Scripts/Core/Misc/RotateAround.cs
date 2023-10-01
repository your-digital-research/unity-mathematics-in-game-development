using System;
using UnityEngine;
using UniRx;

namespace Core.Misc
{
    public class RotateAround : MonoBehaviour
    {
        #region SERIALIZED_VARIABLES

        [Header("Settings")]
        [SerializeField] [Range(0, 360)] private float rotationSpeed;
        [SerializeField] private Vector3 rotationAxis;

        #endregion

        #region PRIVATE_VARIABLES

        private IDisposable _rotationDisposable;

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
            StartRotation();
        }

        private void StartRotation()
        {
            _rotationDisposable ??= Observable
                .EveryUpdate()
                .Repeat()
                .Subscribe(_ =>
                {
                    Rotate();
                });
        }

        private void StopRotation()
        {
            _rotationDisposable.Dispose();
            _rotationDisposable = null;
        }

        private void Rotate()
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;

            // Rotate the object in its local space around the specified axis
            transform.localRotation *= Quaternion.Euler(rotationAxis * rotationAmount);
        }

        #endregion
    }
}