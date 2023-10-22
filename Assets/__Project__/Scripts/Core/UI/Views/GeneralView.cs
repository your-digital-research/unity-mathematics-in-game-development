using Core.Loaders;
using UnityEngine;
using Zenject;

namespace Core.UI
{
    public class GeneralView : MonoBehaviour
    {
        #region PRIVATE_VARIABLES

        private SceneLoader _sceneLoader;

        #endregion

        #region ZENJECT

        [Inject]
        private void Constructor(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        #endregion

        #region PUBLIC_FUNCTIONS

        public void OnHomeButtonClick()
        {
            _sceneLoader.LoadMainScene();
        }

        #endregion
    }
}