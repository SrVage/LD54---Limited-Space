using Code.ECS.Common.References;
using Code.ECS.Player.Components;
using Code.LevelLoader.Components;
using Code.Services.UI.Gameplay.Panels;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

namespace Code.LevelLoader.System
{
    public partial struct SpawnPlayerSystem : ISystem
    {
        private EntityQuery _playerSpawnedQuery;
        
        public void OnCreate(ref SystemState state)
        {
            var spawnedPlayer = new EntityQueryDesc()
            {
                All = new ComponentType[]{typeof(PlayerComponent)}, 
                None = new ComponentType[]{typeof(Prefab)}
            };

           _playerSpawnedQuery = state.World.EntityManager.CreateEntityQuery(spawnedPlayer);
           
            state.RequireForUpdate<PlayerSpawnComponent>();
            state.RequireForUpdate<SpawnPointComponent>();
        }

        public void OnUpdate(ref SystemState state)
        {
            if (_playerSpawnedQuery.CalculateEntityCount() > 0)
                return;
            
            var prefab = SystemAPI.GetSingleton<PlayerSpawnComponent>().PlayerPrefab;
            var spawnPointEntity = SystemAPI.GetSingletonEntity<SpawnPointComponent>();
            var position = SystemAPI.GetComponentRO<LocalToWorld>(spawnPointEntity).ValueRO.Position;
            var entity = state.EntityManager.Instantiate(prefab);
            var transform = SystemAPI.GetComponentRW<LocalTransform>(entity);
            transform.ValueRW.Position = position;
            state.EntityManager.DestroyEntity(SystemAPI.GetSingletonEntity<PlayerSpawnComponent>());
            var mass = state.EntityManager.GetComponentData<PhysicsMass>(entity);
            mass.InverseInertia.x = 0;
            mass.InverseInertia.z = 0;
            state.EntityManager.SetComponentData(entity, mass);

            foreach (var playerStatusService in SystemAPI.Query<PlayerStatusServiceReference>())
            {
                playerStatusService.Value.PlayerMaxHealth.Value = 100;
            }
        }
    }
}