using Code.Abstract.Interfaces;
using Unity.Entities;

namespace Code.ECS.Moving
{
    public struct Rotatable : IComponentData, IEntityFeature, IEnableableComponent
    {
        public float ValueInDegrees;
        
        public void Compose(IBaker baker, Entity entity)
        {
            baker.AddComponent(entity, this);
            baker.SetComponentEnabled<Rotatable>(entity, false);
        }

        public void Compose(EntityCommandBuffer entityCommandBuffer, Entity entity)
        {
            entityCommandBuffer.AddComponent(entity, this);
            entityCommandBuffer.SetComponentEnabled<Rotatable>(entity, false);
        }
    }
}