using Code.ECS.Enemies.Falling;
using Code.ECS.States.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace Code.ECS.Moving
{
    public partial struct StoppingSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<MovableComponent>();
            state.RequireForUpdate<PlayState>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (physicsVelocity, enabledMovable, fallableComponent) in SystemAPI.Query<RefRW<PhysicsVelocity>, EnabledRefRO<MovableComponent>, EnabledRefRO<FallableComponent>>().WithOptions(EntityQueryOptions.IgnoreComponentEnabledState))
            {
                if (fallableComponent.ValueRO)
                {
                    continue;
                }
                
                if (!enabledMovable.ValueRO)
                {
                    physicsVelocity.ValueRW.Linear = float3.zero;
                }
            }
        }
    }
}