using Code.ECS.Enemies.Attacking;
using Code.ECS.Moving;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Code.ECS.Enemies.Targeting
{
    public partial struct CalculateDistanceToTargetSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<TargetableComponent>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (targetable, localTransform) in SystemAPI.Query<RefRW<TargetableComponent>, RefRO<LocalTransform>>())
            {
                var targetTransform = state.EntityManager.GetComponentData<LocalTransform>(targetable.ValueRO.Target);

                targetable.ValueRW.Distance = math.distance(targetTransform.Position, localTransform.ValueRO.Position);
            }
        }
    }
}