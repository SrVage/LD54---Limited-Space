using Code.ECS.States.Components;
using Code.ECS.Wall.Components;
using Unity.Collections;
using Unity.Entities;

namespace Code.ECS.Wall.Systems
{
    public partial struct MoveWallSystem:ISystem
    {
        private EntityQuery _returnTimer;

        public void OnCreate(ref SystemState state)
        {
            _returnTimer = SystemAPI.QueryBuilder().WithAll<ReturnWallTimer>().Build();
        }

        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var moveWall in SystemAPI.Query<MoveWallAspect>())
            {
                moveWall.Move(SystemAPI.Time.DeltaTime, ecb, !_returnTimer.IsEmpty);
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}