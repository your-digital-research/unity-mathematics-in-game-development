using Core.UI;
using UnityEngine;
using Zenject;

namespace Core.Context
{
    public class ViewInstaller : MonoInstaller
    {
        #region SERIALIZED_VARIABLES

        [Header("Views")]
        [SerializeField] private DebugView debugView;
        [SerializeField] private MainMenuView mainMenuView;
        [SerializeField] private CameraMovementView cameraMovementView;

        #endregion

        #region OVERRIDDEN_FUNCTIONS

        public override void InstallBindings()
        {
            BindViews();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void BindViews()
        {
            Container.Bind<DebugView>().FromInstance(debugView);
            Container.Bind<MainMenuView>().FromInstance(mainMenuView).AsSingle();
            Container.Bind<CameraMovementView>().FromInstance(cameraMovementView).AsSingle();
        }

        #endregion
    }
}