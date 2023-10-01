using System.Collections.Generic;
using Code.Abstract;
using Code.Abstract.Interfaces.UI;
using Code.Abstract.Interfaces.UI.Common;
using Code.Configs;
using Zenject;

namespace Code.Services
{
    public class UIPanelsShower : IUIPanelsShower
    {
        private IUIFactory _uiFactory;
        
        private Dictionary<string, BaseView> _instantiatedUIPanels;
        private BaseView _uiPanelBuffer;

        [Inject]
        public UIPanelsShower(IUIFactory uiFactory, UIConfig uiConfig)
        {
            _uiFactory = uiFactory;
            
            _instantiatedUIPanels = new Dictionary<string, BaseView>(uiConfig.UIViews.Length);
        }

        public T ShowView<T>() where T : BaseView<T>
        {
            if (_instantiatedUIPanels.TryGetValue(typeof(T).ToString(), out _uiPanelBuffer))
            {
                _uiPanelBuffer.Show();
                
                return (T)_uiPanelBuffer;
            }
            
            _instantiatedUIPanels.Add(typeof(T).ToString(), _uiFactory.InstantiateView<T>());
            
            return ShowView<T>();
        }

        public void HideView<T>() where T : BaseView<T>
        {
            if (_instantiatedUIPanels.TryGetValue(typeof(T).ToString(), out _uiPanelBuffer))
            {
                _uiPanelBuffer.Hide();
            }
        }
    }
}