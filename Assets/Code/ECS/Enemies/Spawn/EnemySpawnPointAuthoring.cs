using System.Collections.Generic;
using Code.Abstract;
using Code.Abstract.Interfaces;
using Unity.Entities;
using UnityEngine;

namespace Code.ECS.Enemies.Spawn
{
    public struct EnemySpawnPointComponent : IComponentData, IEntityFeature
    {
        public void Compose(IBaker baker, Entity entity)
        {
            baker.AddComponent(entity, this);
        }

        public void Compose(EntityCommandBuffer entityCommandBuffer, Entity entity)
        {
            entityCommandBuffer.AddComponent(entity, this);
        }
    }

    public class EnemySpawnPointAuthoring : BaseBakeableAuthoring
    {
        
    }

    public class EnemySpawnPointBaked : Baker<EnemySpawnPointAuthoring>
    {
        public override void Bake(EnemySpawnPointAuthoring pointAuthoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
            foreach (var feature in pointAuthoring.Features)
            {
                feature?.Compose(this, entity);
            }
        }
    }
}