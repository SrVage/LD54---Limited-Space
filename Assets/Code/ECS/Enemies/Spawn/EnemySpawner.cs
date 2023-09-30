using Code.Abstract.Interfaces;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

namespace Code.ECS.Enemies.Spawn
{
    public class EnemySpawnerAuthoring : MonoBehaviour
    {
        public EnemyAuthoring[] EnemiesPrefabs;
        public float MaxSpawnDelay;
    }
    
    public struct SpawnedEntitiesBuffer : IBufferElementData
    {
        public Entity EnemyPrefab;
    }
    
    public struct EnemySpawnerComponent : IComponentData
    {
        public float MaxSpawnDelay;
        public float CurrentSpawnDelay;
        public float LevelTimer;
    }
    
    public class EnemySpawnerBaker : Baker<EnemySpawnerAuthoring>
    {
        public override void Bake(EnemySpawnerAuthoring authoring)
        {
            var spawnerEntity = GetEntity(TransformUsageFlags.None);

            AddBuffer<SpawnedEntitiesBuffer>(spawnerEntity);
            
            NativeArray<Entity> entityPrefabs = new NativeArray<Entity>(authoring.EnemiesPrefabs.Length, Allocator.Temp);

            for (int i = 0; i < authoring.EnemiesPrefabs.Length; i++)
            {
                IEntityTag entityTag = default;
                
                foreach (var feature in authoring.EnemiesPrefabs[i].EntityTemplate)
                {
                    if (feature is IEntityTag)
                    {
                        entityTag = (IEntityTag)feature;
                        
                        break;
                    }
                }

                if (entityTag == default)
                {
                    Debug.LogError($"{authoring.EnemiesPrefabs[i]} havent BaseEnemyTagComponent!");
                }
                
                entityPrefabs[i] = GetEntity(authoring.EnemiesPrefabs[i], TransformUsageFlags.Dynamic);
                AppendToBuffer(spawnerEntity, new SpawnedEntitiesBuffer
                {
                    EnemyPrefab = entityPrefabs[i],
                });
            }

            var enemySpawnerComponent = new EnemySpawnerComponent
            {
                MaxSpawnDelay = authoring.MaxSpawnDelay,
                CurrentSpawnDelay = authoring.MaxSpawnDelay
            };
            
            AddComponent(spawnerEntity, enemySpawnerComponent);
        }
    }
}