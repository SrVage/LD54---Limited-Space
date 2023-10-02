using System.Security.Cryptography.X509Certificates;
using Code.Abstract.Interfaces.UI.Gameplay;
using Code.ECS.Common.References;
using Code.ECS.Player.Components;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Code.ECS.Player.Systems
{
    public partial struct PlayerDamageSystem:ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var playerStatusReference in SystemAPI.Query<PlayerStatusServiceReference>())
            {
                foreach (var audioServiceReference in SystemAPI.Query<AudioServiceReference>())
                {
                    foreach (var damageAspect in SystemAPI.Query<DamageAspect>().WithNone<PlayerComponent>())
                    {
                        damageAspect.Hit(state.EntityManager, ecb);
                        audioServiceReference.Value.PlayHit();
                    }
                    foreach (var damageAspect in SystemAPI.Query<DamageAspect>().WithAll<PlayerComponent>())
                    {
                        damageAspect.Hit(state.EntityManager, ecb, playerStatusReference.Value);
                        audioServiceReference.Value.PlayHit();
                    }
                }
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
    
    public readonly partial struct DamageAspect:IAspect
    {
        private readonly RefRW<HealthComponent> _health;
        private readonly RefRO<HitComponent> _hit;
        private readonly Entity _entity;

        public void Hit(EntityManager entityManager, EntityCommandBuffer ecb, IPlayerStatusService playerStatusService)
        {
            Hit(entityManager, ecb);
            playerStatusService.PlayerCurrentHealth.Value = _health.ValueRO.Value;
        }

        public void Hit(EntityManager entityManager, EntityCommandBuffer ecb)
        {
            if (entityManager.HasComponent<UndamageTimer>(_entity))
            {
                ecb.RemoveComponent<HitComponent>(_entity);
                return;
            }
            _health.ValueRW.Value -= _hit.ValueRO.Value;
            ecb.RemoveComponent<HitComponent>(_entity);
            ecb.AddComponent(_entity, new UndamageTimer()
            {
                Value = 1
            });
        }
    }
}