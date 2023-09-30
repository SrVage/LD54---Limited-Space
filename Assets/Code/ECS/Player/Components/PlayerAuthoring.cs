using Code.ECS.Common;
using Unity.Entities;
using UnityEngine;

namespace Code.ECS.Player.Components
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public CommonComponents[] Components;
    }
    
    public struct PlayerComponent:IComponentData
    {
        public Entity Value;
    }

    public class PlayerBaker:Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var entityPrefab = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerComponent() {Value = entityPrefab});
            foreach (var component in authoring.Components) 
                component.Bake(this, entity);
        }
    }
}