using Code.Abstract.Interfaces;
using Unity.Entities;

namespace Code.ECS.Enemies.Tag
{
    public struct Blank : IComponentData, IEntityTag, IEntityFeature
    {
        public void Compose(IBaker baker, Entity entity)
        {
            baker?.AddComponent(entity, this);
        }

        public void Compose(EntityCommandBuffer entityCommandBuffer, Entity entity)
        {
            entityCommandBuffer.AddComponent(entity, this);
        }
    }
}