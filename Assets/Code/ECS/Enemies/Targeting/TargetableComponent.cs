using Code.Abstract.Interfaces;
using Unity.Entities;

namespace Code.ECS.Enemies.Targeting
{
    public struct TargetableComponent : IComponentData, IEntityFeature, IEnableableComponent
    {
        public Entity Target;
        public float Distance;
        
        public void Compose(IBaker baker, Entity entity)
        {
            baker.AddComponent(entity, this);
            baker.SetComponentEnabled<TargetableComponent>(entity, false);
        }

        public void Compose(EntityCommandBuffer entityCommandBuffer, Entity entity)
        {
            entityCommandBuffer.AddComponent(entity, this);
            entityCommandBuffer.SetComponentEnabled<TargetableComponent>(entity, false);
        }
    }
}