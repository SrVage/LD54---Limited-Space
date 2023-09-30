using Code.Abstract;
using Unity.Entities;

namespace Code.ECS.Enemies
{
    public class EnemyAuthoring : BaseAuthoring
    {
        
    }

    public class EnemyBaker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            var entityPrefab = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
            
            foreach (var feature in authoring.EntityTemplate)
            {
                feature.Compose(this, entityPrefab);
            }
        }
    }
}