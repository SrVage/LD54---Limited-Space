using Unity.Entities;
using Unity.Entities.Serialization;

namespace Code.ECS.ServiceReferences
{
    public struct SceneLoaderComponent:IComponentData
    {
        public EntitySceneReference Value;
    }
}