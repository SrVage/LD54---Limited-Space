using Code.ECS.CommonComponents;
using Code.ECS.Enemies.Falling;
using Code.ECS.Player.Components;
using Code.ECS.Player.Systems;
using Code.ECS.States.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

namespace Code.ECS.Moving
{
    [UpdateBefore(typeof(MovingSystem))]
    [UpdateBefore(typeof(PlayerMoveSystem))]
    public partial struct SpeedMultiplySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SpeedComponent>();
            state.RequireForUpdate<PlayState>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            foreach (var (speed, entity) in SystemAPI.Query<RefRW<SpeedComponent>>().WithEntityAccess())
            {
                speed.ValueRW.Multiply = 1;
                
                if (state.EntityManager.HasComponent<MultiplyComponent>(entity))
                {
                    speed.ValueRW.Multiply = state.EntityManager.GetComponentData<MultiplyComponent>(entity).Value;
                    ecb.RemoveComponent<MultiplyComponent>(entity);
                }
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}