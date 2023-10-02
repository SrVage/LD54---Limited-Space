using Code.ECS.Common.References;
using Code.ECS.Player.Components;
using Unity.Collections;
using Unity.Entities;

namespace Code.ECS.Player.Systems
{
    public partial struct PlayerDamageSystem:ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var audioServiceReference in SystemAPI.Query<AudioServiceReference>())
            {
                foreach (var damageAspect in SystemAPI.Query<DamageAspect>())
                {
                    damageAspect.Hit(state.EntityManager, ecb);
                    audioServiceReference.Value.PlayHit();
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