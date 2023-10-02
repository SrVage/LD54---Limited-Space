using Code.ECS.Enemies.Targeting;
using Code.ECS.Moving;
using Code.ECS.Player.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Code.ECS.Enemies.Attacking
{
    public partial struct EnemiesAttackingSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<AttackableComponent>();
            state.RequireForUpdate<TargetableComponent>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (attackable, targetable, entity) in SystemAPI.Query<RefRW<AttackableComponent>, RefRO<TargetableComponent>>().WithEntityAccess())
            {
                if (state.EntityManager.HasComponent<MovableComponent>(entity) && state.EntityManager.IsComponentEnabled<MovableComponent>(entity))
                {
                    continue;
                }

                if (attackable.ValueRO.Distance < targetable.ValueRO.Distance)
                {
                    continue;
                }

                if (attackable.ValueRO.CurrentCooldown > 0)
                {
                    continue;
                }

                ecb.AddComponent<HitComponent>(targetable.ValueRO.Target);
                ecb.SetComponent(targetable.ValueRO.Target, new HitComponent()
                {
                    Value = attackable.ValueRO.Damage
                });
                attackable.ValueRW.CurrentCooldown = attackable.ValueRO.MaxCooldown;
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}