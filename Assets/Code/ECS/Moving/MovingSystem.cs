using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Code.ECS.Moving
{
    public partial struct MovingSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<MovableComponent>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (movable, localTransform) in SystemAPI.Query<RefRO<MovableComponent>, RefRW<LocalTransform>>())
            {
                localTransform.ValueRW.Position += localTransform.ValueRO.Forward() * movable.ValueRO.Speed * SystemAPI.Time.DeltaTime;
            }
        }
    }
}