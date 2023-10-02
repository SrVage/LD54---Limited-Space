using Client.Code.ECS.Input;
using Code.ECS.States.Components;
using Unity.Entities;
using Unity.Mathematics;

namespace Code.ECS.Input.Systems
{
    [UpdateAfter(typeof(BindInputSystem))]
    public partial struct CheckInputSystem:ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<InputDirectionComponent>();
            state.RequireForUpdate<PlayState>();
        }

        public void OnUpdate(ref SystemState state)
        {
            if (SystemAPI.HasSingleton<MoveSignal>())
                state.EntityManager.DestroyEntity(SystemAPI.GetSingletonEntity<MoveSignal>());
            var input = SystemAPI.GetSingleton<InputDirectionComponent>();
            if (math.pow(input.Value.x,2) < 0.01 && math.pow(input.Value.y,2) < 0.01)
                return;
            state.EntityManager.AddComponent<MoveSignal>(state.EntityManager.CreateEntity());
        }
    }
}