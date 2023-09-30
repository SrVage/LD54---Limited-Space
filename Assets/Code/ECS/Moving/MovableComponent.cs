using Code.Abstract.Interfaces;
using Unity.Entities;

namespace Code.ECS.Moving
{
    public struct MovableComponent : IComponentData, IEntityFeature, IEnableableComponent
    {
        public float Speed;
        
        public void Compose(IBaker baker, Entity entity)
        {
            baker.AddComponent(entity, this);
            baker.SetComponentEnabled<MovableComponent>(entity, false);
        }

        public void Compose(EntityCommandBuffer entityCommandBuffer, Entity entity)
        {            
            entityCommandBuffer.AddComponent(entity, this);
            entityCommandBuffer.SetComponentEnabled<MovableComponent>(entity, false);
        }
    }
}