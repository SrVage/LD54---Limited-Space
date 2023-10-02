using Code.ECS.Input.Components;
using Unity.Collections;
using Unity.Entities;

namespace Code.ECS.Input.Systems
{
    public partial struct AttackCooldownTimerSystem:ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (timer, entity) in SystemAPI.Query<RefRW<AttackCooldownTImer>>().WithEntityAccess())
            {
                timer.ValueRW.Value -= SystemAPI.Time.DeltaTime;
                if (timer.ValueRO.Value <= 0)
                {
                    ecb.RemoveComponent<AttackCooldownTImer>(entity);
                }
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}