using Code.Configs;
using Unity.Entities;
using UnityEngine;

namespace Code.Abstract
{
    public class BaseEntityAuthoring:MonoBehaviour
    {
        public EntityTemplate EntityTemplate;
    }
    
    public class EntityBaker : Baker<BaseEntityAuthoring>
    {
        public override void Bake(BaseEntityAuthoring authoring)
        {
            var entityPrefab = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
            
            foreach (var feature in authoring.EntityTemplate)
            {
                feature.Compose(this, entityPrefab);
            }
        }
    }
}