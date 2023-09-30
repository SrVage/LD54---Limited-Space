using Code.Abstract.Interfaces;
using Sirenix.OdinInspector.Editor.GettingStarted;
using UnityEditor;
using Zenject;

namespace Code.Services
{
    public class ServicesInstaller : MonoInstaller
    {
        
        public override void InstallBindings()
        {
            Container.Bind<IConfigReferenceService>().To<ConfigReferenceService>().AsSingle().NonLazy();
        }
    }
}