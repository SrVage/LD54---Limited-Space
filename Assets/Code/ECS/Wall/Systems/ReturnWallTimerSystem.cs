using Code.ECS.Wall.Components;
using Unity.Entities;

namespace Code.ECS.Wall.Systems
{
    public partial struct ReturnWallTimerSystem:ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ReturnWallTimer>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var timer = SystemAPI.GetSingleton<ReturnWallTimer>().Value;
            timer -= SystemAPI.Time.DeltaTime;
            if (timer<=0) 
                state.EntityManager.DestroyEntity(SystemAPI.GetSingletonEntity<ReturnWallTimer>());
            else
                SystemAPI.SetSingleton(new ReturnWallTimer() { Value = timer });
        }
    }
}