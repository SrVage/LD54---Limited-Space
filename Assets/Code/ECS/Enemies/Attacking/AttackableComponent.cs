using Code.Abstract.Interfaces;
using Unity.Entities;

namespace Code.ECS.Enemies.Attacking
{
    public struct AttackableComponent : IComponentData, IEntityFeature
    {
        public float Distance;
        public float StoppingDistance;
        public int Damage;
        public float MaxCooldown;
        public float CurrentCooldown;
        
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