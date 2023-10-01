using Client.Code.ECS.Input;
using Code.ECS.CommonComponents;
using Code.ECS.Player.Components;
using Code.ECS.States.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Code.ECS.Player.Systems
{
    [BurstCompile]
    public partial struct PlayerMoveSystem:ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<InputDirectionComponent>();
            state.RequireForUpdate<PlayerComponent>();
            state.RequireForUpdate<PlayState>();
        }

        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            var direction = SystemAPI.GetSingleton<InputDirectionComponent>().Value;
            foreach (var entity in SystemAPI.Query<MovePlayerAspect>().WithAll<PlayerComponent>())
            {
                entity.Move(direction, state.EntityManager, ecb);
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }

    [BurstCompile]
    public readonly partial struct MovePlayerAspect : IAspect
    {
        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRO<SpeedComponent> _speed;
        private readonly RefRW<PhysicsVelocity> _velocity;
        private readonly Entity _entity;

        public void Move(float2 direction, EntityManager entityManager, EntityCommandBuffer ecb)
        {
            float multiply = 1;
            if (entityManager.HasComponent<MultiplyComponent>(_entity))
            {
                multiply = entityManager.GetComponentData<MultiplyComponent>(_entity).Value;
                ecb.RemoveComponent<MultiplyComponent>(_entity);
            }
            var speed = _speed.ValueRO.Value*multiply;
            _velocity.ValueRW.Linear = new float3(direction.x, 0, direction.y) * speed;
            if (direction.x != 0 && direction.y != 0)
            {
                var atan = math.atan2(direction.x, direction.y);
                _transform.ValueRW.Rotation = quaternion.Euler(0, atan, 0);
            }
        }
    }
}