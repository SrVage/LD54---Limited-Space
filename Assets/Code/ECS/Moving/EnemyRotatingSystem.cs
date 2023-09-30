using Code.ECS.Enemies;
using Code.ECS.Enemies.Targeting;
using Code.ECS.Player.Components;
using Unity.Burst;
using Unity.Entities;
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
                typeof(Rotatable)));
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (physicsVelocity, rotatable, targetable) in SystemAPI.Query<RefRW<PhysicsVelocity>, RefRO<Rotatable>, RefRO<TargetableComponent>>().WithAll<EnemyComponent>())
            {
                var targetLocalTransform = SystemAPI.GetComponentRO<LocalTransform>(targetable.ValueRO.Target);

                physicsVelocity.ValueRW.Linear.y = rotatable.ValueRO.ValueInDegrees;
                //Debug.Log(targetLocalTransform.ValueRO.Position);
            }
        }
    }
}