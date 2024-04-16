using Core.Loaders;
using UnityEngine;
using Zenject;

namespace Core.UI
{
    public class GeneralView : MonoBehaviour
    {
        #region PRIVATE_VARIABLES

        private SceneLoader _sceneLoader;
        private TransitionView _transitionView;

        #endregion

        #region ZENJECT

        [Inject]
        private void Constructor(SceneLoader sceneLoader, TransitionView transitionView)
        {
            _sceneLoader = sceneLoader;
            _transitionView = transitionView;
        }

        #endregion

        #region PUBLIC_FUNCTIONS

        public void OnHomeButtonClick()
        {
            _transitionView.Show(() => _sceneLoader.LoadMainScene());
        }

        #endregion
    }
}