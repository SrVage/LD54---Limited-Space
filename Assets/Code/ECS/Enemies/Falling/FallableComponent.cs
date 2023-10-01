using Code.Abstract.Interfaces;
using Unity.Entities;

namespace Code.ECS.Enemies.Falling
{
    public struct FallableComponent : IComponentData, IEntityFeature, IEnableableComponent
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