using Code.Abstract.Interfaces;
using Unity.Entities;

namespace Code.ECS.CommonComponents
{
    public struct SpeedComponent:IComponentData, IEntityFeature
    {
        public float Value;
        public float Multiply;
        public void Compose(IBaker baker, Entity entity)
        {
            Multiply = 1;
            baker.AddComponent(entity, this);
        }

        public void Compose(EntityCommandBuffer entityCommandBuffer, Entity entity)
        {
            Multiply = 1;
            entityCommandBuffer.AddComponent(entity, this);
        }
    }
}