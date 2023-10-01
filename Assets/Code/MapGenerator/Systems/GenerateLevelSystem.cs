using Code.MapGenerator.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = UnityEngine.Random;

namespace Code.MapGenerator.Systems
{
    public partial struct GenerateLevelSystem:ISystem
    {
        private EntityQuery _signalQuery;

        public void OnCreate(ref SystemState state)
        {
            _signalQuery = SystemAPI.QueryBuilder().WithAll<HasLevelComponent>().Build();
        }

        public void OnUpdate(ref SystemState state)
        {
            if (!_signalQuery.IsEmpty)
                return;
            foreach (var (mapSize, tiles) in SystemAPI.Query<RefRO<MapSizeComponent>, RefRO<TilesPrefabComponent>>())
            {
                var percents = tiles.ValueRO.Percents;
                var calculateCounts = CalculateCounts(tiles.ValueRO.Percents, mapSize.ValueRO.Value.x*mapSize.ValueRO.Value.y);
                int3 counts = new int3(0, 0, 0);
                int2 startTile = new int2(mapSize.ValueRO.Value.x / 2, mapSize.ValueRO.Value.y / 2);
                for (int i = 0; i < mapSize.ValueRO.Value.x; i++)
                {
                    for (int j = 0; j < mapSize.ValueRO.Value.y; j++)
                    {
                        Entity tile;
                        if (startTile.x == i && startTile.y == j)
                        {
                            tile = state.EntityManager.Instantiate(tiles.ValueRO.StartTilePrefab);
                        }
                        else if (startTile.x == i+1 && startTile.y == j-1)
                        {
                            tile = state.EntityManager.Instantiate(tiles.ValueRO.SpawnTilePrefab);
                        }
                        else
                        {
                            var randomTile = Random.Range(0, 100);
                            if (randomTile < percents.x)
                            {
                                if (counts.x >= calculateCounts.x)
                                    tile = state.EntityManager.Instantiate(tiles.ValueRO.SimpleTilePrefab);
                                else
                                {
                                    counts.x++;
                                    tile = state.EntityManager.Instantiate(tiles.ValueRO.TrapTilePrefab);
                                }
                            }
                            else if (randomTile >= percents.x && randomTile<percents.y+percents.x)
                            {
                                if (counts.y >= calculateCounts.y)
                                    tile = state.EntityManager.Instantiate(tiles.ValueRO.SimpleTilePrefab);
                                else
                                {
                                    counts.y++;
                                    tile = state.EntityManager.Instantiate(tiles.ValueRO.SpeedDecreaseTilePrefab);
                                }
                            }
                            else if (randomTile >= percents.x+percents.y && randomTile<percents.x+percents.y+percents.z)
                            {
                                if (counts.z >= calculateCounts.z)
                                    tile = state.EntityManager.Instantiate(tiles.ValueRO.SimpleTilePrefab);
                                else
                                {
                                    counts.z++;
                                    tile = state.EntityManager.Instantiate(tiles.ValueRO.UnWalkTilePrefab);
                                }
                            }
                            else
                            {
                                tile = state.EntityManager.Instantiate(tiles.ValueRO.SimpleTilePrefab);
                            }
                        }
                        var transform = SystemAPI.GetComponentRW<LocalTransform>(tile);
                        transform.ValueRW.Position = new float3(2*i, 0, 2*j);
                    }
                }
            }
            state.EntityManager.AddComponent<HasLevelComponent>(state.EntityManager.CreateEntity());
        }

        private int3 CalculateCounts(int3 percents, int generalCount)
        {
            return new int3((int)(generalCount*((float)percents.x/100)), (int)(generalCount*((float)percents.y/100)), (int)(generalCount*((float)percents.z/100)));
        }
    }
}