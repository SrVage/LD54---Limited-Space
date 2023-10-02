using UnityEngine;
using Zenject;

namespace Code.Configs
{
    public class ConfigInstaller : MonoInstaller
    {
        [SerializeField] private ConfigsSingle _configsSingle;
    
        public override void InstallBindings()
        {
            Container.Bind<AudioConfig>().FromInstance(_configsSingle.AudioConfig).AsSingle().NonLazy();
            Container.Bind<UIConfig>().FromInstance(_configsSingle.UIConfig).AsSingle().NonLazy();
            Container.Bind<LevelConfig>().FromInstance(_configsSingle.LevelConfig).AsSingle().NonLazy();
            Container.Bind<EffectsConfig>().FromInstance(_configsSingle.EffectsConfig).AsSingle().NonLazy();
        }

        private void Awake()
        {
            if (_configsSingle == null)
            {
                Debug.LogError($"{name}: ConfigsSingle is null!");
            }
            
            ConfigsSingle.Init(_configsSingle);
        }
    }
}