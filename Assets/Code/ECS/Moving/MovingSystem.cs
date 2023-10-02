using Code.ECS.CommonComponents;
using Code.ECS.Enemies.Falling;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

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
            foreach (var (physicsVelocity, speed, localTransform) in SystemAPI.Query<RefRW<PhysicsVelocity>, RefRO<SpeedComponent>, RefRO<LocalTransform>>().WithAll<MovableComponent>().WithNone<FallableComponent>())
            {
                physicsVelocity.ValueRW.Linear = localTransform.ValueRO.Forward() * speed.ValueRO.Value * speed.ValueRO.Multiply;
            }
        }
    }
}