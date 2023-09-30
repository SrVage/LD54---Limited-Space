using Unity.Entities;

namespace Code.Abstract.Interfaces.Entity
{
    public interface IEntityFeature
    {
        void Compose(IBaker baker, Unity.Entities.Entity entity);
        void Compose(EntityCommandBuffer entityCommandBuffer, Unity.Entities.Entity entity);
    }
}