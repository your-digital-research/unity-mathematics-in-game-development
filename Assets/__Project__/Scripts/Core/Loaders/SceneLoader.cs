using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Loaders
{
    public class SceneLoader : MonoBehaviour
    {
        #region PUBLIC_FUNCTIONS

        public void LoadSceneByIndex(int index)
        {
            SceneManager.LoadScene(index, LoadSceneMode.Single);
        }

        #endregion
    }
}