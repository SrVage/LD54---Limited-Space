using Client.Code.ECS.Input;
using Code.ECS.CommonComponents;
using Code.ECS.Player.Components;
using Code.ECS.States.Components;
using Unity.Burst;
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
            var direction = SystemAPI.GetSingleton<InputDirectionComponent>().Value;
            var sprint = SystemAPI.GetSingleton<InputDirectionComponent>().Sprint;
            foreach (var entity in SystemAPI.Query<MovePlayerAspect>().WithAll<PlayerComponent>())
            {
                entity.Move(direction);
            }
        }
    }

    [BurstCompile]
    public readonly partial struct MovePlayerAspect : IAspect
    {
        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRO<SpeedComponent> _speed;
        private readonly RefRW<PhysicsVelocity> _velocity;

        public void Move(float2 direction)
        {
            var speed = _speed.ValueRO.Value;
            _velocity.ValueRW.Linear = new float3(direction.x, 0, direction.y) * speed;
            if (direction.x != 0 && direction.y != 0)
            {
                var atan = math.atan2(direction.x, direction.y);
                _transform.ValueRW.Rotation = quaternion.Euler(0, atan, 0);
            }
        }
    }
}