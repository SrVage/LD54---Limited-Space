using Code.ECS.CommonComponents;
using Code.ECS.Wall.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Code.ECS.Wall.Systems
{
    public readonly partial struct MoveWallAspect:IAspect
    {
        private const float Delta = 0.5f;
        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRO<SpeedComponent> _speed;
        private readonly RefRO<MoveDirectionComponent> _direction;
        private readonly RefRO<BordersPositionComponent> _border;
        private readonly RefRO<MovableTag> _tag;
        
        public void Move(float deltaTime, bool reverse = false)
        {
            if (!reverse)
            {
                if (math.distance(_transform.ValueRO.Position, _border.ValueRO.EndPosition)>Delta)
                    _transform.ValueRW.Position += _direction.ValueRO.Value * _speed.ValueRO.Value * deltaTime;
            }
            else
            {
                if (math.distance(_transform.ValueRO.Position, _border.ValueRO.StartPosition)>Delta)
                    _transform.ValueRW.Position += (-10)*_direction.ValueRO.Value * _speed.ValueRO.Value * deltaTime;
            }
        }
    }
}