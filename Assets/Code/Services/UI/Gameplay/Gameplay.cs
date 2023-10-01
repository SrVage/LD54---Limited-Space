using Code.Abstract;
using Code.Abstract.Interfaces.UI.Common;
using Code.ECS.Common.References;
using Code.UI.View;
using Unity.Entities;
using Zenject;

namespace Code.Services.UI.Gameplay
{
    public class Gameplay : BaseUIManager<Gameplay>
    {
        
        [Inject]
        public Gameplay(
            IUIPanelsShower iuiPanelsShower
            ) : base(iuiPanelsShower)
        {
            var entity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity();
            World.DefaultGameObjectInjectionWorld.EntityManager.AddComponent<GameplayUIManager>(entity);
            World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(entity, new GameplayUIManager
            {
                Value = this
            });
        }

        protected override void AddBinding()
        {
            
        }

        protected override void ShowRequiredElements()
        {
            SetVisible<PlayerStatusView>(true);
        }
    }
}