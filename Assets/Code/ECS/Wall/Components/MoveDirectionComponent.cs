using Code.Abstract.Interfaces.Entity;
using Unity.Entities;
using Unity.Mathematics;

namespace Code.ECS.Wall.Components
{
    public struct MoveDirectionComponent:IComponentData, IEntityFeature
    {
        public float3 Value;
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