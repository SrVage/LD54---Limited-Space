using Code.Abstract;
using Unity.Entities;

namespace Code.ECS.Enemies.Spawn
{
    public class TempEnemySpawnPointAuthoring : BaseBakeableAuthoring
    {
        
    }
    
    public class TempEnemySpawnPointBaker : Baker<TempEnemySpawnPointAuthoring>
    {
        public override void Bake(TempEnemySpawnPointAuthoring authoring)
        {
            var entityPrefab = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
            
            foreach (var feature in authoring.Features)
            {
                feature.Compose(this, entityPrefab);
            }
        }
    }
}