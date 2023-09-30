using Code.Abstract.Interfaces;
using Code.ECS.Common.References;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;
using EnemiesCount = Code.Configs.LevelConfig.EnemiesCount;

namespace Code.ECS.Enemies.Spawn
{
    [BurstCompile]
    [UpdateInGroup(typeof(EnemiesSpawningSystemGroup))]
    [UpdateAfter(typeof(SpawnerTimerSystem))]
    public partial class EnemySpawnSystem : SystemBase
    {
        private int _targetEnemiesCount;
        
        private Entity _cachedEnemyEntity;
        private float3 _cachedSpawnPosition;
        
        private EntityQuery _spawnedEnemiesQuery;
        private EntityQuery _spawnPointsQuery;
        //private EntityQuery _reusableEnemiesQuery;
        

        private EnemiesCount _enemies;
        private IEntityTag[] _enemiesTags;
        private NativeArray<EntityQuery> _loadedEnemiesPrefabsQueries;
        private int _currentEnemyTag;
        private bool _isEnemiesCountWritten;
        
        protected override void OnCreate()
        {
            RequireForUpdate<EnemySpawnerComponent>();
            RequireForUpdate<EnemySpawnPointComponent>();
            RequireForUpdate<ReferenceConfigReferenceService>();

            var spawnedEnemiesQuery = new EntityQueryDesc
            {
                All = new ComponentType[] { ComponentType.ReadOnly<EnemyComponent>() },
                //None = new ComponentType[] { typeof(Dead) }
            };
            
            _spawnedEnemiesQuery = World.EntityManager.CreateEntityQuery(typeof(EnemyComponent));
            _spawnPointsQuery = World.EntityManager.CreateEntityQuery(typeof(EnemySpawnPointComponent), typeof(LocalTransform));
            //_reusableEnemiesQuery = World.EntityManager.CreateEntityQuery(typeof(EnemyComponent), typeof(ReadyForReuseComponent));

            _cachedSpawnPosition = default;
            
            _isEnemiesCountWritten = false;
        }

        protected override void OnUpdate()
        {
            if (!_isEnemiesCountWritten)
            {
                WriteEnemiesCount();
            }
            
            foreach (RefRW<EnemySpawnerComponent> spawner in SystemAPI.Query<RefRW<EnemySpawnerComponent>>())
            {
                if (spawner.ValueRO.CurrentSpawnDelay > 0)
                {
                    return;
                }

                if (_enemies.Cap > _targetEnemiesCount)
                {
                    _targetEnemiesCount = _enemies.Start + (Mathf.FloorToInt(spawner.ValueRO.LevelTimer / 30) * _enemies.PerHalfMinuteMultiply);
                }
                
                if (_spawnedEnemiesQuery.CalculateEntityCount() >= _targetEnemiesCount)
                {
                    return;
                }
            
                spawner.ValueRW.CurrentSpawnDelay = spawner.ValueRO.MaxSpawnDelay;
                
                StartSpawn();
            }
        }

        private void WriteEnemiesCount()
        {
            foreach (var configReferenceServiceReferenceComponent in SystemAPI.Query<ReferenceConfigReferenceService>())
            {
                _enemies = configReferenceServiceReferenceComponent.Value.LevelConfig.Enemies;
                
                _loadedEnemiesPrefabsQueries = new NativeArray<EntityQuery>(_enemies.Tags.Length, Allocator.Persistent);

                for (int i = 0; i < _enemies.Tags.Length; i++)
                {
                    var spawnedEnemiesDesc = new EntityQueryDesc
                    {
                        All = new ComponentType[] { typeof(Prefab), _enemies.Tags[i].GetType() }
                    };
                    
                    _loadedEnemiesPrefabsQueries[i] = World.EntityManager.CreateEntityQuery(spawnedEnemiesDesc);
                }
                
                _isEnemiesCountWritten = true;
            }
        }

        [BurstCompile]
        private void StartSpawn()
        {
            var availablePoints = _spawnPointsQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);
            
            foreach (var (enemySpawnerComponent, spawnerEntity) in SystemAPI.Query<RefRO<EnemySpawnerComponent>>().WithEntityAccess())
            {
                int spawnCount;

                spawnCount = _enemies.SpawnPerTime > availablePoints.Length ? availablePoints.Length : _enemies.SpawnPerTime;
                
                for (int i = 0; i < spawnCount; i++)
                {
                    _cachedSpawnPosition = availablePoints[i].Position;
                    
                    // Spawn new enemy

                    var enemiesPrefabsQuery = _loadedEnemiesPrefabsQueries[_currentEnemyTag % _loadedEnemiesPrefabsQueries.Length];
                    
                    if (enemiesPrefabsQuery.IsEmpty)
                    {
                        Debug.Log($"{enemiesPrefabsQuery} wasn't found!");
                        continue;
                    }
                    
                    var enemiesPrefabsEntities = enemiesPrefabsQuery.ToEntityArray(Allocator.Temp);
                    _cachedEnemyEntity = World.EntityManager.Instantiate(enemiesPrefabsEntities[Random.Range(0, enemiesPrefabsEntities.Length)]);
                    enemiesPrefabsEntities.Dispose();
                    _currentEnemyTag++;

                    SystemAPI.GetComponentRW<LocalTransform>(_cachedEnemyEntity).ValueRW.Position = _cachedSpawnPosition;

                    var mass = EntityManager.GetComponentData<PhysicsMass>(_cachedEnemyEntity);
                    mass.InverseInertia.x = 0;
                    mass.InverseInertia.z = 0;
                    EntityManager.SetComponentData(_cachedEnemyEntity, mass);
                }
            }
        }
    }
}