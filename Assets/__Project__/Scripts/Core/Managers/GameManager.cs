using Core.Constants;
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

        #region PUBLIC_FUNCTIONS

        public void QuitApp()
        {
            Application.Quit();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void Init()
        {
            InitFramerate();
        }

        private void InitFramerate()
        {
            Application.targetFrameRate = Constant.TargetFramerate;
        }

        #endregion
    }
}