using Code.ECS.Player.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Code.ECS.Battle.Systems
{
    public partial struct CheckEnemiesForAttackSystem:ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (strength, transform) in SystemAPI.Query<RefRO<StrengthAttackComponent>, RefRO<LocalTransform>>()
                         .WithAll<PlayerComponent>().WithAll<AttackEvent>())
            {
                foreach (var (hp, transformEnemy, entity) in SystemAPI.Query<RefRW<HealthComponent>, RefRO<LocalTransform>>().WithNone<PlayerComponent>().WithEntityAccess())
                {
                    var distance = math.distance(transform.ValueRO.Position, transformEnemy.ValueRO.Position);
                    if (distance>strength.ValueRO.Distance)
                        continue;
                    var vector = math.normalize(transformEnemy.ValueRO.Position - transform.ValueRO.Position);
                    var dot = math.dot(math.normalize(transform.ValueRO.Forward()), vector);
                    if (dot < strength.ValueRO.Range)
                        continue;
                    hp.ValueRW.Value -= strength.ValueRO.Value;
                    Debug.Log(hp.ValueRO.Value);
                    Debug.Log(entity);
                    if (hp.ValueRO.Value <= 0)
                    {
                        ecb.DestroyEntity(entity);
                    }
                }
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}