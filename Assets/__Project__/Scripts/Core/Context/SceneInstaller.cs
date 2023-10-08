using Core.UI;
using UnityEngine;
using Zenject;

namespace Core.Context
{
    public class SceneInstaller : MonoInstaller
    {
        #region SERIALIZED_VARIABLES

        [Header("References")]
        [SerializeField] private CameraMovementView cameraMovementView;

        #endregion

        #region OVERRIDDEN_FUNCTIONS

        public override void InstallBindings()
        {
            Bind();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void Bind()
        {
            Container.Bind<CameraMovementView>().FromInstance(cameraMovementView).AsSingle();
        }

        #endregion
    }
}