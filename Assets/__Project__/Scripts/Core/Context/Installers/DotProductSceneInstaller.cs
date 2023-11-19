using Core.Cameras;
using Core.Managers;
using Core.Loaders;
using UnityEngine;
using Zenject;

namespace Core.Context
{
    public class DotProductSceneInstaller : MonoInstaller
    {
        #region SERIALIZED_VARIABLES

        [Header("Managers")]
        [SerializeField] private GameManager gameManager;
        [SerializeField] private DebugManager debugManager;

        [Header("Loaders")]
        [SerializeField] private SceneLoader sceneLoader;

        [Header("Camera")]
        [SerializeField] private CameraController cameraController;

        #endregion

        #region OVERRIDDEN_FUNCTIONS

        public override void InstallBindings()
        {
            BindManagers();
            BindLoaders();
            BindCamera();
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

        #endregion
    }
}