using Core.Types;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Loaders
{
    public class SceneLoader : MonoBehaviour
    {
        #region PUBLIC_FUNCTIONS

        public void LoadMainScene()
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        public void LoadExampleScene(ExampleScene exampleScene)
        {
            SceneManager.LoadScene((int)exampleScene, LoadSceneMode.Single);
        }

        #endregion
    }
}