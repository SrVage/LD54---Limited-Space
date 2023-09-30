using Code.ECS.Wall.Components;
using Code.MapGenerator.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.MapGenerator.Systems
{
    [UpdateBefore(typeof(GenerateLevelSystem))]
    public partial struct GenerateWallSystem:ISystem
    {
        private EntityQuery _signalQuery;

        public void OnCreate(ref SystemState state)
        {
            _signalQuery = SystemAPI.QueryBuilder().WithAll<HasLevelComponent>().Build();
            state.RequireForUpdate<MapSizeComponent>();
        }

        public void OnUpdate(ref SystemState state)
        {
            if (!_signalQuery.IsEmpty)
                return;
            var mapSize = SystemAPI.GetSingleton<MapSizeComponent>().Value;
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var tiles in SystemAPI.Query<RefRO<WallPrefabComponent>>())
            {
                var percent = tiles.ValueRO.Percents;
                for (int i = 0; i < mapSize.x; i++)
                {
                    Entity tile;
                    var randomTile = Random.Range(0, 100);
                    if (randomTile<=tiles.ValueRO.Percents)
                        tile = state.EntityManager.Instantiate(tiles.ValueRO.LightWallPrefab);
                    else
                        tile = state.EntityManager.Instantiate(tiles.ValueRO.SimpleWallPrefab);
                    var transform = SystemAPI.GetComponentRW<LocalTransform>(tile);
                    transform.ValueRW.Position = new float3(2*i, 0, mapSize.y*2-1);
                    ecb.AddComponent<MovableTag>(tile);
                    var border = SystemAPI.GetComponentRW<BordersPositionComponent>(tile);
                    border.ValueRW.StartPosition = transform.ValueRO.Position;
                    border.ValueRW.EndPosition = new float3(transform.ValueRO.Position.x, transform.ValueRO.Position.y, -1);
                }
                for (int i = 0; i < mapSize.y; i++)
                {
                    Entity tile;
                    var randomTile = Random.Range(0, 100);
                    if (randomTile<=tiles.ValueRO.Percents)
                        tile = state.EntityManager.Instantiate(tiles.ValueRO.LightWallPrefab);
                    else
                        tile = state.EntityManager.Instantiate(tiles.ValueRO.SimpleWallPrefab);
                    var transform = SystemAPI.GetComponentRW<LocalTransform>(tile);
                    transform.ValueRW.Position = new float3(mapSize.x*2-1, 0, 2*i);
                    transform.ValueRW.Rotation = Quaternion.Euler(0, 90, 0);
                }
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}