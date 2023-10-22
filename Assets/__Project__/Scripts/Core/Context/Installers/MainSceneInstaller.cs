using Core.Managers;
using Core.UI;
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

        [Header("Views")]
        [SerializeField] private DebugView debugView;
        [SerializeField] private MainMenuView mainMenuView;

        #endregion

        #region OVERRIDDEN_FUNCTIONS

        public override void InstallBindings()
        {
            BindManagers();
            BindViews();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void BindManagers()
        {
            Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
            Container.Bind<DebugManager>().FromInstance(debugManager).AsSingle();
        }

        private void BindViews()
        {
            Container.Bind<DebugView>().FromInstance(debugView).AsSingle();
            Container.Bind<MainMenuView>().FromInstance(mainMenuView).AsSingle();
        }

        #endregion
    }
}