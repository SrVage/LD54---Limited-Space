using Code.Abstract;
using Code.Abstract.Interfaces.UI;
using Code.Abstract.Interfaces.UI.Common;
using Code.Abstract.Interfaces.UI.MainMenu;
using Code.ECS.Common.References;
using Code.UI.View;
using UniRx;
using Unity.Entities;
using Zenject;

namespace Code.Services.UI.MainMenu
{
    public class MainMenu : BaseUIManager<MainMenu>
    {
        private IStartScreenService _startScreenService;
        
        [Inject]
        public MainMenu(
            IUIPanelsShower iuiPanelsShower,
            IStartScreenService startScreenService
            ) : base(iuiPanelsShower)
        {
            _startScreenService = startScreenService;
            
            var entity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity();
            World.DefaultGameObjectInjectionWorld.EntityManager.AddComponent<MainMenuUIManager>(entity);
            World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(entity, new MainMenuUIManager
            {
                Value = this
            });

            ShowRequiredElements();
            AddBinding();
        }

        protected override void AddBinding()
        {
            _startScreenService.MainUIEnable.Subscribe(SetVisible<StartScreenView>).AddTo(_disposables);
        }

        protected override void ShowRequiredElements()
        {
            SetVisible<StartScreenView>(true);
            _startScreenService.MainUIEnable.Value = true;
        }
    }
}