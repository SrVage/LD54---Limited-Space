using Code.Abstract;
using Code.Abstract.Interfaces.UI.Gameplay;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.View
{
    public class PlayerStatusView : BaseView<PlayerStatusView>
    {
        [SerializeField] private RectTransform _healthBarRect;

        private IPlayerStatusService _playerStatusService;

        private Vector2 _cachedAnchorMin;
        private Vector2 _cachedAnchorMax;
        private float _cachedHealthPercent;
        
        public override void Init(DiContainer container)
        {
            _playerStatusService = container.Resolve<IPlayerStatusService>();
            
            AddBinding();
        }

        protected override void AddBinding()
        {
            _playerStatusService.PlayerCurrentHealth.Subscribe(OnHealthChanged).AddTo(this);
        }

        private void OnHealthChanged(int value)
        {
            _cachedHealthPercent = value / _playerStatusService.PlayerMaxHealth.Value;
            
            _cachedAnchorMin = _healthBarRect.anchorMin;
            _cachedAnchorMax = _healthBarRect.anchorMax;

            _cachedAnchorMin.x = (1 - _cachedHealthPercent) / 2;
            _cachedAnchorMax.x = (1 + _cachedHealthPercent) / 2;

            _healthBarRect.anchorMin = _cachedAnchorMin;
            _healthBarRect.anchorMax = _cachedAnchorMax;
        }
    }
}