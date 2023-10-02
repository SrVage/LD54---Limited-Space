using Code.Abstract.Interfaces;
using Unity.Entities;

namespace Code.ECS.Tiles.Components
{
    public struct DamageComponent:IComponentData, IEntityFeature, IEnableableComponent
    {
        public int Value;
        public void Compose(IBaker baker, Entity entity)
        {
            baker.AddComponent(entity, this);
            baker.SetComponentEnabled<DamageComponent>(entity, false);
            baker.AddBuffer<DamagedEntityBuffer>(entity);
        }

        public void Compose(EntityCommandBuffer entityCommandBuffer, Entity entity)
        {
            entityCommandBuffer.AddComponent(entity, this);
            entityCommandBuffer.SetComponentEnabled<DamageComponent>(entity, false);
            entityCommandBuffer.AddBuffer<DamagedEntityBuffer>(entity);
        }
    }

    public struct DamagedEntityBuffer : IBufferElementData
    {
        public Entity Entity;
    }
}