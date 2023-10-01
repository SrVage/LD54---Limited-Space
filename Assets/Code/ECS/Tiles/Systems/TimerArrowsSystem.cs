using Code.ECS.States.Components;
using Code.ECS.Tiles.Components;
using Unity.Collections;
using Unity.Entities;

namespace Code.ECS.Tiles.Systems
{
    public partial struct TimerArrowsSystem:ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayState>();
            state.RequireForUpdate<ArrowsTimerComponent>();
        }

        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (timer, entity) in SystemAPI.Query<RefRW<ArrowsTimerComponent>>().WithEntityAccess())
            {
                timer.ValueRW.Value -= SystemAPI.Time.DeltaTime;
                if (timer.ValueRO.Value <= 0) 
                    ecb.RemoveComponent<ArrowsTimerComponent>(entity);
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}