using Unity.Entities;
using UnityEngine;

namespace Code.LevelLoader.Components
{
    public class PlayerSpawnAuthoring : MonoBehaviour
    {
        public GameObject Prefab;
    }
    
    public struct PlayerSpawnComponent:IComponentData
    {
        public Entity PlayerPrefab;
    }
    
    public class PlayerSpawnBaker:Baker<PlayerSpawnAuthoring>
    {
        public override void Bake(PlayerSpawnAuthoring authoring)
        {
            var entityPrefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic);
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerSpawnComponent(){PlayerPrefab = entityPrefab });
        }
    }
}