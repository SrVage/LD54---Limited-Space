using Code.ECS.Player.Components;
using Unity.Collections;
using Unity.Entities;

namespace Code.ECS.Player.Systems
{
    public partial struct UndamageTimerSystem:ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (timer, entity) in SystemAPI.Query<RefRW<UndamageTimer>>().WithEntityAccess())
            {
                timer.ValueRW.Value -= SystemAPI.Time.DeltaTime;
                if (timer.ValueRO.Value <= 0)
                {
                    ecb.RemoveComponent<UndamageTimer>(entity);
                }
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}