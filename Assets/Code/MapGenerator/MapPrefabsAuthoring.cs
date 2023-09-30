using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Code.MapGenerator
{
    public class MapPrefabsAuthoring : MonoBehaviour
    {
        public GameObject SimpleTilePrefab;
        public GameObject TrapTilePrefab;
        public GameObject SpeedDecreaseTilePrefab;
        public GameObject UnWalkTilePrefab;
        public float2 MapSize;
    }

    public struct TilesPrefabComponent:IComponentData
    {
        public Entity SimpleTilePrefab;       
        public Entity TrapTilePrefab;         
        public Entity SpeedDecreaseTilePrefab;
        public Entity UnWalkTilePrefab;       
    }

    public struct MapSizeComponent : IComponentData
    {
        public float2 Value;
    }

    public class MapPrefabsBaker:Baker<MapPrefabsAuthoring>
    {
        public override void Bake(MapPrefabsAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new TilesPrefabComponent()
            {
                SimpleTilePrefab = GetEntity(authoring.SimpleTilePrefab, TransformUsageFlags.Dynamic),
                TrapTilePrefab = GetEntity(authoring.TrapTilePrefab, TransformUsageFlags.Dynamic),
                SpeedDecreaseTilePrefab = GetEntity(authoring.SpeedDecreaseTilePrefab, TransformUsageFlags.Dynamic),
                UnWalkTilePrefab = GetEntity(authoring.UnWalkTilePrefab, TransformUsageFlags.Dynamic),
                
            });
            AddComponent(entity, new MapSizeComponent()
            {
                Value = authoring.MapSize
            });
        }
    }
}