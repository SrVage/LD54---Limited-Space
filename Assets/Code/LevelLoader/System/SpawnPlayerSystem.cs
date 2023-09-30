using Code.ECS.Player.Components;
using Code.LevelLoader.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Code.LevelLoader.System
{
    public partial struct SpawnPlayerSystem : ISystem
    {
        private EntityQuery _playerSpawnedQuery;
        
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerSpawnComponent>();
            var spawnedPlayer = new EntityQueryDesc()
            {
                All = new ComponentType[]{typeof(PlayerComponent)}, 
                None = new ComponentType[]{typeof(Prefab)}
            };

           _playerSpawnedQuery = state.World.EntityManager.CreateEntityQuery(spawnedPlayer);
           
            state.RequireForUpdate<PlayerSpawnComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (_playerSpawnedQuery.CalculateEntityCount() > 0)
                return;
            
            var prefab = SystemAPI.GetSingleton<PlayerSpawnComponent>().PlayerPrefab;
            var position = SystemAPI.GetSingleton<SpawnPointComponent>().Position;
            var entity = state.EntityManager.Instantiate(prefab);
            var transform = SystemAPI.GetComponentRW<LocalTransform>(entity);
            transform.ValueRW.Position = position;
            
            state.EntityManager.DestroyEntity(SystemAPI.GetSingletonEntity<PlayerSpawnComponent>());
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}