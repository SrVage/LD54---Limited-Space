using Code.Abstract.Enums;
using Code.Abstract.Interfaces.UI.MainMenu;
using Code.ECS.States.Components;
using UniRx;
using Unity.Entities;

namespace Code.Services.UI.MainMenu.Panels
{
    public class StartScreenService : IStartScreenService
    {
        public ReactiveProperty<bool> MainUIEnable { get; set; } = new ReactiveProperty<bool>();
        
        public void StartGamePressed()
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            var entity = entityManager.CreateEntity();
            entityManager.AddComponent<ChangeState>(entity);
            entityManager.SetComponentData(entity, new ChangeState()
            {
                Value = State.PlayState
            });
        }
    }
}