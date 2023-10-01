using Code.Abstract.Interfaces;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Code.ECS.Enemies.Spawn
{
    public struct EnemySpawnPointComponent : IComponentData, IEntityFeature
    {
        public float3 OffsetFromOriginalEntity;
        
        [HideInInspector] public float3 EndValue;
        
        
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