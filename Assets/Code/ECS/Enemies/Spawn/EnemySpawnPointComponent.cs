using System.Collections.Generic;
using Code.Abstract;
using Code.Abstract.Interfaces;
using Unity.Entities;
using UnityEngine;

namespace Code.ECS.Enemies.Spawn
{
    public struct EnemySpawnPointComponent : IComponentData
    {
        
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