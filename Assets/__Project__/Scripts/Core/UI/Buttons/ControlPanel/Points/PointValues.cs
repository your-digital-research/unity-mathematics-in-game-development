using Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.UI
{
    public class PointValues : MonoBehaviour
    {
        #region SERIALIZED_VARIABLES

        [Header("Settings")]
        [SerializeField] private int pointIndex;

        #endregion

        #region PRIVATE_VARIABLES

        private List<PositionInputField> _positionInputFields;

        #endregion

        #region PROPERTIES

        public int Index => pointIndex;

        #endregion

        #region EVENTS

        public Action<int, Axis, float> PositionUpdated;

        #endregion

        #region MONO

        private void Start()
        {
            Init();
        }

        #endregion

        #region PUBLIC_FUNCTIONS

        public void UpdateFields(Vector3 position)
        {
            foreach (PositionInputField inputField in _positionInputFields)
            {
                switch (inputField.Axis)
                {
                    case Axis.X:
                        inputField.UpdateField(position.x);
                        break;
                    case Axis.Y:
                        inputField.UpdateField(position.y);
                        break;
                    case Axis.Z:
                        inputField.UpdateField(position.z);
                        break;
                }
            }
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void OnPositionChanged(Axis axis, float value)
        {
            PositionUpdated?.Invoke(pointIndex, axis, value);
        }

        private void Init()
        {
            InitInputFields();
        }

        private void InitInputFields()
        {
            _positionInputFields = GetComponentsInChildren<PositionInputField>(true).ToList();

            foreach (PositionInputField inputField in _positionInputFields)
            {
                inputField.Init();
                inputField.PositionChanged += OnPositionChanged;
            }
        }

        #endregion
    }
}