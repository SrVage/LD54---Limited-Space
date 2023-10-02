using Code.ECS.Player.Components;
using Unity.Burst;
using Unity.Entities;

namespace Code.ECS.Enemies.Targeting
{
    [BurstCompile]
    public partial struct EnemiesTargetingSystem : ISystem
    {
        private EntityQuery _spawnedEnemiesQuery;
        
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerComponent>();
            state.RequireForUpdate(state.EntityManager.CreateEntityQuery(typeof(EnemyComponent), typeof(TargetableComponent)));
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
            
            foreach (var (targetableComponent, entity) in SystemAPI.Query<RefRW<TargetableComponent>>().WithAll<EnemyComponent>().WithEntityAccess().WithOptions(EntityQueryOptions.IgnoreComponentEnabledState))
            {
                if (targetableComponent.ValueRO.Target != playerEntity)
                {
                    state.EntityManager.SetComponentEnabled<TargetableComponent>(entity, true);
                    targetableComponent.ValueRW.Target = SystemAPI.GetSingletonEntity<PlayerComponent>();
                }
            }
        }
    }
}