using System;
using Code.Abstract.Interfaces.UI;
using Code.Abstract.Interfaces.UI.Common;
using UniRx;

namespace Code.Abstract
{
    public abstract class BaseUIManager : IUIManager, IDisposable
    {
        public ReactiveProperty<bool> IsCurrentState { get; set; } = new ReactiveProperty<bool>();
        
        protected CompositeDisposable _disposables = new CompositeDisposable();

        protected IUIPanelsShower _iuiPanelsShower;

        protected bool _isEnabled = false;
        
        public BaseUIManager(IUIPanelsShower iuiPanelsShower)
        {
            _iuiPanelsShower = iuiPanelsShower;

            SubOnCurrentState();
        }

        protected virtual void SubOnCurrentState()
        {
            IsCurrentState.Subscribe(value =>
            {
                if (value && !_isEnabled)
                {
                    Enable();
                }
                else if (!value && _isEnabled)
                {
                    Disable();
                }
            });
        }

        protected virtual void Enable()
        {
            _isEnabled = true;
            
            ShowRequiredElements();
            
            AddBinding();
        }

        protected virtual void Disable()
        {
            _isEnabled = false;
            
            Clear();
        }

        protected abstract void AddBinding();

        protected abstract void ShowRequiredElements();
        
        protected virtual void SetVisible<T>(bool enable) where T : BaseView<T>
        {
            if (enable)
            {
                _iuiPanelsShower.ShowView<T>();
            }
            else
            {
                _iuiPanelsShower.HideView<T>();
            }
        }

        protected virtual void Clear()
        {
            _disposables.Clear();
        }

        public virtual void Dispose()
        {
            Clear();
            IsCurrentState?.Dispose();
            _disposables.Dispose();
        }
    }
    
    public abstract class BaseUIManager<T> : BaseUIManager where T : BaseUIManager<T>
    {
        protected BaseUIManager(IUIPanelsShower iuiPanelsShower) : base(iuiPanelsShower)
        {
            
        }
    }
}