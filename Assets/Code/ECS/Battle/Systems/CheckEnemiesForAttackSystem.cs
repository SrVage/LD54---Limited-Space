using Code.ECS.Player.Components;
using Code.ECS.States.Components;
using Code.ECS.Wall.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Code.ECS.Battle.Systems
{
    public partial struct CheckEnemiesForAttackSystem:ISystem
    {
        private const float ReturnTimeForKill = 0.5f;
        private EntityQuery _returnTimer;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayState>();
            _returnTimer = SystemAPI.QueryBuilder().WithAll<ReturnWallTimer>().Build();
        }

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
                        if (!_returnTimer.IsEmpty)
                        {
                            var timer = SystemAPI.GetSingleton<ReturnWallTimer>().Value;
                            SystemAPI.SetSingleton(new ReturnWallTimer()
                            {
                                Value = timer+ReturnTimeForKill
                            });
                        }
                        else
                        {
                            var entityTimer = ecb.CreateEntity();
                            ecb.AddComponent<ReturnWallTimer>(entityTimer);
                            ecb.SetComponent(entityTimer, new ReturnWallTimer(){Value = ReturnTimeForKill});
                        }
                        break;
                    }
                }
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}