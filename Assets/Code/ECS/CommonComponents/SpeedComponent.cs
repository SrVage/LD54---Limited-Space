using Code.Abstract.Interfaces;
using Unity.Entities;

namespace Code.ECS.CommonComponents
{
    public struct SpeedComponent:IComponentData, IEntityFeature
    {
        public float Value;
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