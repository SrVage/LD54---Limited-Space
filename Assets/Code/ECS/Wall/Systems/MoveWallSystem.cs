using Code.ECS.States.Components;
using Unity.Entities;

namespace Code.ECS.Wall.Systems
{
    public partial struct MoveWallSystem:ISystem
    {
        private EntityQuery _playState;

        public void OnCreate(ref SystemState state)
        {
            _playState = SystemAPI.QueryBuilder().WithAll<PlayState>().Build();
        }

        public void OnUpdate(ref SystemState state)
        {
            foreach (var moveWall in SystemAPI.Query<MoveWallAspect>())
            {
                moveWall.Move(SystemAPI.Time.DeltaTime, _playState.IsEmpty);
            }
        }
    }
}