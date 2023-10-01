using Code.Abstract.Enums;
using Code.Abstract.Interfaces.UI.MainMenu;
using Code.ECS.ServiceReferences;
using Code.ECS.States.Components;
using UniRx;
using Unity.Entities;
using UnityEngine;

namespace Code.Services.UI.MainMenu.Panels
{
    public class StartScreenService : IStartScreenService
    {
        public ReactiveProperty<bool> MainUIEnable { get; set; } = new ReactiveProperty<bool>();

        public StartScreenService()
        {
            var entity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity();
            World.DefaultGameObjectInjectionWorld.EntityManager.AddComponent<StartScreenServiceReference>(entity);
            World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(entity, new StartScreenServiceReference()
            {
                Value = this
            });
        }
        
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