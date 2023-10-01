using Code.ECS.Common.References;
using Code.ECS.States.Components;
using Unity.Entities;

namespace Code.ECS.States.Systems.StatesGroups
{
    public partial class PlayStateSystem : ComponentSystemGroup
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireForUpdate<PlayState>();
        }

        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            
            foreach (var gameplayManagerReference in SystemAPI.Query<GameplayUIManager>())
            {
                gameplayManagerReference.Value.IsCurrentState.Value = true;
            }
            
            foreach (var gameplayManagerReference in SystemAPI.Query<AudioServiceReference>())
            {
                gameplayManagerReference.Value.SetGameplayMusic();
            }
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            
            // ???
        }

        protected override void OnStopRunning()
        {
            base.OnStopRunning();
            
            foreach (var gameplayManagerReference in SystemAPI.Query<GameplayUIManager>())
            {
                gameplayManagerReference.Value.IsCurrentState.Value = false;
            }
        }
    }
}