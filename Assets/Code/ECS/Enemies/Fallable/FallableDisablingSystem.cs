using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Code.ECS.Enemies.Fallable
{
    public partial struct FallableDisablingSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FallableComponent>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (localTransform, fallable) in SystemAPI.Query<RefRW<LocalTransform>, EnabledRefRW<FallableComponent>>())
            {
                if (localTransform.ValueRO.Position.y <= 0)
                {
                    localTransform.ValueRW.Position.y = 0;

                    fallable.ValueRW = false;
                }
            }
        }
    }
}