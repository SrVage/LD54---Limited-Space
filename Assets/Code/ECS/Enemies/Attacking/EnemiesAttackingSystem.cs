using Code.ECS.Enemies.Targeting;
using Code.ECS.Moving;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

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
                
                Debug.Log("Damage");
                attackable.ValueRW.CurrentCooldown = attackable.ValueRO.MaxCooldown;
            }
        }
    }
}