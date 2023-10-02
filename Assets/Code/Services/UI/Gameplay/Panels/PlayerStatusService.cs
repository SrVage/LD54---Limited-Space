using Code.Abstract.Interfaces.UI.Gameplay;
using Code.ECS.Common.References;
using UniRx;
using Unity.Entities;
using Zenject;

namespace Code.Services.UI.Gameplay.Panels
{
    public class PlayerStatusService : IPlayerStatusService
    {
        public ReactiveProperty<bool> MainUIEnable { get; set; } = new ReactiveProperty<bool>();
        public ReactiveProperty<int> PlayerMaxHealth { get; set; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> PlayerCurrentHealth { get; set; } = new ReactiveProperty<int>();

        [Inject]
        public PlayerStatusService()
        {
            var entity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity();
            World.DefaultGameObjectInjectionWorld.EntityManager.AddComponent<PlayerStatusServiceReference>(entity);
            World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(entity, new PlayerStatusServiceReference()
            {
                Value = this
            });
        }
    }
}