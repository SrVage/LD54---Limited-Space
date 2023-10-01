using Code.Abstract.Interfaces;
using Unity.Entities;

namespace Code.ECS.Moving
{
    public struct RotatableComponent : IComponentData, IEntityFeature, IEnableableComponent
    {
        public void Compose(IBaker baker, Entity entity)
        {
            baker.AddComponent(entity, this);
            baker.SetComponentEnabled<RotatableComponent>(entity, false);
        }

        public void Compose(EntityCommandBuffer entityCommandBuffer, Entity entity)
        {
            entityCommandBuffer.AddComponent(entity, this);
            entityCommandBuffer.SetComponentEnabled<RotatableComponent>(entity, false);
        }
    }
}