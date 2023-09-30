using System;
using Code.Abstract;
using Code.Abstract.Extensions;
using Code.Abstract.Interfaces;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.View
{
    public class InputView : BaseView<InputView>
    {
        [SerializeField] private Button _attackButton;

        private Action _attackButtonAction;
        private IInputService _inputService;

        public override void Init(DiContainer container)
        {
            _inputService = container.Resolve<IInputService>();
            AddBinding();
        }

        protected override void AddBinding()
        {
            _attackButton.RxSubscribe(OnAttackButtonPressed).AddTo(this);
            _inputService.ActionButtonsAction.Subscribe(action => _attackButtonAction = action).AddTo(this);
        }

        public override void Show()
        {
            _selfCanvas.enabled = true;
        }

        public override void Hide()
        {
            _selfCanvas.enabled = false;
        }

        private void OnAttackButtonPressed()
        {
            if (_attackButtonAction == null) return;
            
            _attackButtonAction.Invoke();
        }
    }
}