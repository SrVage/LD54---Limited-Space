using Unity.Entities;
using UnityEngine;

namespace Code.MapGenerator.Components
{
    public class WallPrefabsAuthoring : MonoBehaviour
    {
        public MapGeneratorConfig Config;
    }

    public struct WallPrefabComponent:IComponentData
    {
        public Entity SimpleWallPrefab;       
        public Entity LightWallPrefab;         
        public int Percents;
    }

    public class WallPrefabsBaker:Baker<WallPrefabsAuthoring>
    {
        public override void Bake(WallPrefabsAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new WallPrefabComponent()
            {
                SimpleWallPrefab = GetEntity(authoring.Config.SimpleWallPrefab, TransformUsageFlags.Dynamic),
                LightWallPrefab = GetEntity(authoring.Config.LightWallPrefab, TransformUsageFlags.Dynamic),
                Percents = authoring.Config.WallPercents
            });
        }
    }
}