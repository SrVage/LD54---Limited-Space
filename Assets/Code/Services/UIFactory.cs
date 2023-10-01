using Code.Abstract;
using Code.Abstract.Interfaces.UI;
using Code.Abstract.Interfaces.UI.Common;
using Code.Configs;
using Code.UI;
using UnityEngine;
using Zenject;

namespace Code.Services
{
    public class UIFactory : IUIFactory
    {
        private DiContainer _diContainer;
        private UITag _uiTag;
        private UIConfig _uiConfig;
    
        [Inject]
        public UIFactory(DiContainer diContainer, UITag uiTag, UIConfig uiConfig)
        {
            _diContainer = diContainer;
            _uiTag = uiTag;
            _uiConfig = uiConfig;
        }
    
        public T InstantiateView<T>() where T : BaseView<T>
        {
            foreach (var abstractViewPrefab in _uiConfig.UIViews)
            {
                if (abstractViewPrefab.GetType() == typeof(T))
                {
                    var instantiatedPrefab = GameObject.Instantiate((T)abstractViewPrefab, _uiTag.transform, false);
                    instantiatedPrefab.Init(_diContainer);
                    return instantiatedPrefab;
                }
            }
        
            Debug.LogError($"{typeof(T)} wasn't found in UIConfig!");
        
            return null;
        }

        public void DisposeView<T>() where T : BaseView<T>
        {
        
        }
    }
}