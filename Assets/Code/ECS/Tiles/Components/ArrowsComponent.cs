using Code.Abstract.Interfaces;
using Unity.Entities;
using UnityEngine;

namespace Code.ECS.Tiles.Components
{
    public struct ArrowsComponent:IEntityFeature
    {
        public GameObject Prefab;
        public void Compose(IBaker baker, Entity entity)
        {
            baker.AddComponent(entity, new ArrowsEntityComponent()
            {
                Value = baker.GetEntity(Prefab, TransformUsageFlags.Dynamic)
            });
        }

        public void Compose(EntityCommandBuffer entityCommandBuffer, Entity entity)
        {
            
        }
    }

    public struct ArrowsEntityComponent : IComponentData
    {
        public Entity Value;
    }
}