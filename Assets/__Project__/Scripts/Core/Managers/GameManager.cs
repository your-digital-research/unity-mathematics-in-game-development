using System;
using UnityEngine;

namespace Core.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region MONO

        private void Start()
        {
            Init();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void Init()
        {
            InitFramerate();
        }

        private void InitFramerate()
        {
            Application.targetFrameRate = 60;
        }

        #endregion
    }
}