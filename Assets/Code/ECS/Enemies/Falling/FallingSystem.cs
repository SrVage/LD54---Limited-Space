using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Code.ECS.Enemies.Fallable
{
    public partial struct FallingSystem : ISystem
    {
        private const float _gravityAcceleration = 9.78f;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FallableComponent>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (localTransform, fallable) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<FallableComponent>>())
            {
                fallable.ValueRW.TimeInAir += SystemAPI.Time.DeltaTime;
                
                localTransform.ValueRW.Position.y -= _gravityAcceleration * fallable.ValueRO.TimeInAir;
            }
        }
    }
}