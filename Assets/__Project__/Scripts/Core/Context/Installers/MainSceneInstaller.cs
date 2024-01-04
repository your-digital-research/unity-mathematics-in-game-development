using Core.UI;
using Core.Loaders;
using Core.Managers;
using UnityEngine;
using Zenject;

namespace Core.Context
{
    public class MainSceneInstaller : MonoInstaller
    {
        #region SERIALIZED_VARIABLES

        [Header("Managers")]
        [SerializeField] private GameManager gameManager;
        [SerializeField] private DebugManager debugManager;

        [Header("Loaders")]
        [SerializeField] private SceneLoader sceneLoader;

        [Header("Views")]
        [SerializeField] private DebugView debugView;
        [SerializeField] private MainMenuView mainMenuView;
        [SerializeField] private TransitionView transitionView;

        #endregion

        #region OVERRIDDEN_FUNCTIONS

        public override void InstallBindings()
        {
            BindManagers();
            BindLoaders();
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

        private void BindViews()
        {
            Container.Bind<DebugView>().FromInstance(debugView).AsSingle();
            Container.Bind<MainMenuView>().FromInstance(mainMenuView).AsSingle();
            Container.Bind<TransitionView>().FromInstance(transitionView).AsSingle();
        }

        #endregion
    }
}