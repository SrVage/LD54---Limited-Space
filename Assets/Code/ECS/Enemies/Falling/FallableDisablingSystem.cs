using Code.ECS.Moving;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Code.ECS.Enemies.Falling
{
    public partial struct FallableDisablingSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FallableComponent>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (localTransform, fallable, entity) in SystemAPI.Query<RefRW<LocalTransform>, EnabledRefRW<FallableComponent>>().WithEntityAccess())
            {
                if (localTransform.ValueRO.Position.y > 0)
                {
                    continue;
                }
                
                localTransform.ValueRW.Position.y = 0;

                fallable.ValueRW = false;

                if (state.EntityManager.HasComponent<RotatableComponent>(entity))
                {
                    state.EntityManager.SetComponentEnabled<RotatableComponent>(entity, true);
                }

                if (state.EntityManager.HasComponent<MovableComponent>(entity))
                {
                    state.EntityManager.SetComponentEnabled<MovableComponent>(entity, true);
                }
            }
        }
    }
}