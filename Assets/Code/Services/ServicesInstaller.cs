using Code.Abstract.Interfaces;
using Code.Abstract.Interfaces.UI.Common;
using Code.Abstract.Interfaces.UI.MainMenu;
using Code.Audio.Service;
using Code.Services.UI.Gameplay;
using Code.Services.UI.MainMenu;
using Code.Services.UI.MainMenu.Panels;
using Code.UI;
using UnityEngine;
using Zenject;

namespace Code.Services
{
    public class ServicesInstaller : MonoInstaller
    {
        [SerializeField] private UITag _uiTag;
        
        public override void InstallBindings()
        {
            Container.Bind<UITag>().FromInstance(_uiTag).AsSingle();
            
            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle().NonLazy();
            Container.Bind<IUIPanelsShower>().To<UIPanelsShower>().AsSingle().NonLazy();
            
            Container.Bind<IStartScreenService>().To<StartScreenService>().AsSingle();
            
            Container.Bind<IUIManager>().WithId(nameof(MainMenu)).To<MainMenu>().AsSingle().NonLazy();
            Container.Bind<IUIManager>().WithId(nameof(Gameplay)).To<Gameplay>().AsSingle().NonLazy();
            
            Container.Bind<IAudioService>().To<AudioService>().AsSingle();
            Container.Bind<IConfigReferenceService>().To<ConfigReferenceService>().AsSingle().NonLazy();
        }
    }
}