using Code.Abstract.Interfaces;
using Unity.Entities;

namespace Code.ECS.Tiles.Components
{
    public struct ActiveTileTag:IComponentData, IEntityFeature
    {
        public void Compose(IBaker baker, Entity entity)
        {
            baker.AddComponent(entity, this);
        }

        public void Compose(EntityCommandBuffer entityCommandBuffer, Entity entity)
        {
            entityCommandBuffer.AddComponent(entity, this);
        }
    }
}