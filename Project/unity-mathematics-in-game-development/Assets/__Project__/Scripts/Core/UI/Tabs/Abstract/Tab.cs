using System;
using UnityEngine;

namespace Core.UI
{
    public abstract class Tab : MonoBehaviour
    {
        #region PROPERTIES

        public bool IsVisible { get; protected set; } = false;
        public bool IsStable { get; protected set; } = true;

        #endregion

        #region MONO

        private void Start()
        {
            Init();
        }

        #endregion

        #region ABSTRACT_FUNCTIONS

        public abstract void Toggle(bool value, bool force = false, Action onComplete = null);

        protected abstract void Init();

        #endregion
    }
}