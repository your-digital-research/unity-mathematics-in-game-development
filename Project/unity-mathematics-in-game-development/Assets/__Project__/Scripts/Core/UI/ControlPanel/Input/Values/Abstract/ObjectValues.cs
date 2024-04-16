using Core.Types;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.UI
{
    public abstract class ObjectValues : MonoBehaviour
    {
        #region PRIVATE_VARIABLES

        private List<InputField> _inputFields;

        #endregion

        #region MONO

        private void Start()
        {
            Init();
        }

        #endregion

        #region ABSTRACT_FUNCTIONS

        protected abstract void OnInputFieldValueChanged(Axis axis, float value);

        #endregion

        #region PUBLIC_FUNCTIONS

        public void UpdateFields(Vector3 position)
        {
            foreach (InputField inputField in _inputFields)
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

        private void Init()
        {
            InitInputFields();
        }

        private void InitInputFields()
        {
            _inputFields = GetComponentsInChildren<InputField>(true).ToList();

            foreach (InputField inputField in _inputFields)
            {
                inputField.Init();
                inputField.ValueChanged += OnInputFieldValueChanged;
            }
        }

        #endregion
    }
}