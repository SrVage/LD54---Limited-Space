using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Code.MapGenerator.Components
{
    public class MapPrefabsAuthoring : MonoBehaviour
    {
        public MapGeneratorConfig Config;
    }

    public struct TilesPrefabComponent:IComponentData
    {
        public Entity SimpleTilePrefab;       
        public Entity TrapTilePrefab;         
        public Entity SpeedDecreaseTilePrefab;
        public Entity UnWalkTilePrefab;
        public Entity StartTilePrefab;
        public int3 Percents;
    }

    public struct MapSizeComponent : IComponentData
    {
        public int2 Value;
    }

    public class MapPrefabsBaker:Baker<MapPrefabsAuthoring>
    {
        public override void Bake(MapPrefabsAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new TilesPrefabComponent()
            {
                SimpleTilePrefab = GetEntity(authoring.Config.SimpleTilePrefab, TransformUsageFlags.Dynamic),
                TrapTilePrefab = GetEntity(authoring.Config.TrapTilePrefab, TransformUsageFlags.Dynamic),
                SpeedDecreaseTilePrefab = GetEntity(authoring.Config.SpeedDecreaseTilePrefab, TransformUsageFlags.Dynamic),
                UnWalkTilePrefab = GetEntity(authoring.Config.UnWalkTilePrefab, TransformUsageFlags.Dynamic),
                StartTilePrefab = GetEntity(authoring.Config.StartTilePrefab, TransformUsageFlags.Dynamic),
                Percents = authoring.Config.TilesPercents
            });
            AddComponent(entity, new MapSizeComponent()
            {
                Value = authoring.Config.MapSize
            });
        }
    }
}