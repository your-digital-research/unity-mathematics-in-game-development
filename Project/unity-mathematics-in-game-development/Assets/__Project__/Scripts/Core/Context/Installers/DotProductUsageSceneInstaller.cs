using Core.UI;
using Core.Cameras;
using Core.Loaders;
using Core.Managers;
using UnityEngine;
using Zenject;

namespace Core.Context
{
    public class DotProductUsageSceneInstaller : MonoInstaller
    {
        #region SERIALIZED_VARIABLES

        [Header("Managers")]
        [SerializeField] private GameManager gameManager;
        [SerializeField] private DebugManager debugManager;

        [Header("Loaders")]
        [SerializeField] private SceneLoader sceneLoader;

        [Header("Camera")]
        [SerializeField] private CameraController cameraController;

        [Header("Views")]
        [SerializeField] private TransitionView transitionView;

        #endregion

        #region OVERRIDDEN_FUNCTIONS

        public override void InstallBindings()
        {
            BindManagers();
            BindLoaders();
            BindCamera();
            BindViews();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void BindManagers()
        {
            Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
            Container.Bind<DebugManager>().FromInstance(debugManager).AsSingle();
        }

        private void BindLoaders()
        {
            Container.Bind<SceneLoader>().FromInstance(sceneLoader).AsSingle();
        }

        private void BindCamera()
        {
            Container.Bind<CameraController>().FromInstance(cameraController).AsSingle();
        }

        private void BindViews()
        {
            Container.Bind<TransitionView>().FromInstance(transitionView).AsSingle();
        }

        #endregion
    }
}