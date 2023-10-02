using Code.ECS.States.Components;
using Code.ECS.Tiles.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Code.ECS.Tiles.Systems
{
    public partial struct ChangeTrapArrowsSystem:ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayState>();
        }

        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (cycle, damage, arrow, entity) in SystemAPI.Query<RefRW<ArrowsCycleComponent>, EnabledRefRW<DamageComponent>, RefRO<ArrowsEntityComponent>>()
                         .WithNone<ArrowsTimerComponent>().WithEntityAccess()
                         .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState))
            {
                cycle.ValueRW.Reverse = !cycle.ValueRO.Reverse;
                damage.ValueRW = !cycle.ValueRO.Reverse;
                var transform = SystemAPI.GetComponentRW<LocalTransform>(arrow.ValueRO.Value);
                transform.ValueRW.Position = new float3(transform.ValueRO.Position.x, cycle.ValueRO.Reverse ? -1 : 0,
                    transform.ValueRO.Position.z);
                
                ecb.AddComponent(entity, new ArrowsTimerComponent()
                {
                    Value = cycle.ValueRO.Reverse?cycle.ValueRO.SafeTime:cycle.ValueRO.AttackTime
                });

                if (!cycle.ValueRO.Reverse && state.EntityManager.HasBuffer<DamagedEntityBuffer>(entity))
                {
                    state.EntityManager.GetBuffer<DamagedEntityBuffer>(entity).Clear();
                }
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}