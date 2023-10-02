using Code.ECS.Enemies.Falling;
using Code.ECS.Enemies.Targeting;
using Code.ECS.Moving;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Code.ECS.Enemies.Attacking
{
    public partial struct StoppingBeforeAttack : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<AttackableComponent>();
            state.RequireForUpdate<MovableComponent>();
            state.RequireForUpdate<TargetableComponent>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (attackable, targetable, movable) in SystemAPI.Query<RefRO<AttackableComponent>, RefRO<TargetableComponent>, EnabledRefRW<MovableComponent>>().WithOptions(EntityQueryOptions.IgnoreComponentEnabledState))
            {
                if (targetable.ValueRO.Target == Entity.Null)
                {
                    continue;
                }

                if (movable.ValueRO)
                {
                    if (attackable.ValueRO.StoppingDistance >= targetable.ValueRO.Distance)
                    {
                        movable.ValueRW = false;
                    }
                }
                else
                {
                    if (targetable.ValueRO.Distance > attackable.ValueRO.Distance &&
                        targetable.ValueRO.Distance > attackable.ValueRO.StoppingDistance)
                    {
                        movable.ValueRW = true;
                    }
                }
            }
        }
    }
}