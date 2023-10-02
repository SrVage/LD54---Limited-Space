using Code.Abstract.Interfaces;
using Code.ECS.ServiceReferences;
using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Scenes;
using Zenject;

namespace Code.Services
{
    public class SceneReferenceService:ISceneReferenceService
    {
        [Inject]
        public SceneReferenceService(SubScene subScene)
        {
            EntitySceneReference scene = new EntitySceneReference(subScene.SceneAsset);
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var entity = entityManager.CreateEntity();
            entityManager.AddComponent<SceneLoaderComponent>(entity);
            entityManager.SetComponentData(entity, new SceneLoaderComponent()
            {
                Value = scene
            });
        }
    }
}