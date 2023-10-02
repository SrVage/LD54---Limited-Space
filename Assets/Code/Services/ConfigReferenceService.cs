using Code.Abstract.Interfaces;
using Code.Configs;
using Code.ECS.Common.References;
using Unity.Entities;
using Zenject;

namespace Code.Services
{
    public class ConfigReferenceService : IConfigReferenceService
    {
        public UIConfig UIConfig { get; }
        public LevelConfig LevelConfig { get; }
        public EffectsConfig EffectsConfig { get; }

        [Inject]
        public ConfigReferenceService(LevelConfig levelConfig, UIConfig uiConfig, EffectsConfig effectsConfig)
        {
            UIConfig = uiConfig;
            LevelConfig = levelConfig;
            EffectsConfig = effectsConfig;
            var entity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity();
            World.DefaultGameObjectInjectionWorld.EntityManager.AddComponent<ReferenceConfigReferenceService>(entity);
            World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(entity, new ReferenceConfigReferenceService()
            {
                Value = this
            });
        }
    }
}