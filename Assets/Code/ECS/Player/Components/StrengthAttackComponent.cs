using Code.Abstract.Interfaces;
using Unity.Entities;

namespace Code.ECS.Player.Components
{
    public struct StrengthAttackComponent:IComponentData, IEntityFeature
    {
        public int Value;
        public float Distance;
        public float Range;
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