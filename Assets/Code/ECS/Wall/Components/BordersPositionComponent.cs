using Code.Abstract.Interfaces;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Code.ECS.Wall.Components
{
    public struct BordersPositionComponent:IComponentData, IEntityFeature
    {
        [HideInInspector] public float3 StartPosition;
        [HideInInspector] public float3 EndPosition;
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