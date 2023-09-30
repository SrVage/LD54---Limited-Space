using Unity.Entities;
using Unity.Mathematics;
using Random = UnityEngine.Random;

namespace Code.MapGenerator
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
                for (int i = 0; i < mapSize.ValueRO.Value.x; i++)
                {
                    for (int j = 0; j < mapSize.ValueRO.Value.y; j++)
                    {
                        var randomTile = Random.Range(0, 4);
                    }
                }
            }
        }
    }
}