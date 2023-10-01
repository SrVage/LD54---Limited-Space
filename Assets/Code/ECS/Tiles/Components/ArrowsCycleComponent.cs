using Code.Abstract.Interfaces;
using Unity.Entities;
using UnityEngine;

namespace Code.ECS.Tiles.Components
{
    public struct ArrowsCycleComponent:IComponentData, IEntityFeature
    {
        public float AttackTime;
        public float SafeTime;
        [HideInInspector] public bool Reverse;
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