using Code.ECS.Enemies;
using Code.ECS.Enemies.Targeting;
using Code.ECS.Player.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace Code.ECS.Moving
{
    public partial struct EnemyRotatingSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate(state.EntityManager.CreateEntityQuery(
                typeof(PhysicsVelocity),
                typeof(EnemyComponent),
                typeof(TargetableComponent),
                typeof(RotatableComponent)));
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (localTransform, rotatable, targetable) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotatableComponent>, RefRO<TargetableComponent>>().WithAll<EnemyComponent>())
            {
                var targetLocalTransform = SystemAPI.GetComponentRO<LocalTransform>(targetable.ValueRO.Target);

                float3 vector = targetLocalTransform.ValueRO.Position - localTransform.ValueRO.Position;
                float3 direction = math.normalize(vector);
                
                var atan = math.atan2(direction.x, direction.z);
                localTransform.ValueRW.Rotation = quaternion.Euler(0, atan, 0);

                //physicsVelocity.ValueRW.Linear.y = rotatable.ValueRO.ValueInDegrees;
                //Debug.Log(targetLocalTransform.ValueRO.Position);
            }
        }
    }
}