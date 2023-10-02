using Code.Abstract.Enums;
using Code.ECS.States.Components;
using Unity.Entities;
using UnityEngine;

namespace Code.Services
{
    public class InitScene:MonoBehaviour
    {
        private void Awake()
        {
            World.DefaultGameObjectInjectionWorld.EntityManager.AddComponent<MainState>(World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity());
            var stateEntity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity();
            World.DefaultGameObjectInjectionWorld.EntityManager.AddComponent<ChangeState>(stateEntity);
            World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(stateEntity, new ChangeState()
            {
                Value = State.MenuState
            });
        }
    }
}