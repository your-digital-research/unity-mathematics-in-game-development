using Core.Managers;
using UnityEngine;
using Zenject;

namespace Core.Context
{
    public class ManagerInstaller : MonoInstaller
    {
        #region SERIALIZED_VARIABLES

        [Header("Managers")]
        [SerializeField] private GameManager gameManager;
        [SerializeField] private DebugManager debugManager;

        #endregion

        #region OVERRIDDEN_FUNCTIONS

        public override void InstallBindings()
        {
            BindManagers();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void BindManagers()
        {
            Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
            Container.Bind<DebugManager>().FromInstance(debugManager).AsSingle();
        }

        #endregion
    }
}