using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Code.LevelLoader.Components
{
    public class SpawnPointAuthoring : MonoBehaviour
    {
        
    }

    public struct SpawnPointComponent:IComponentData
    {
        public float3 Position;
    }

    public class SpawnPointBaker:Baker<SpawnPointAuthoring>
    {
        public override void Bake(SpawnPointAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new SpawnPointComponent(){Position = authoring.transform.position });
        }
    }
}