using Code.Abstract;
using Code.Abstract.Extensions;
using Code.Abstract.Interfaces.UI.MainMenu;
using Code.Audio.Service;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.View
{
    public class StartScreenView : BaseView<StartScreenView>
    {
        [SerializeField] private Button _startGameButton;
        
        private IStartScreenService _startScreenService;
        private IAudioService _audioService;
        
        public override void Init(DiContainer container)
        {
            _startScreenService = container.Resolve<IStartScreenService>();
            _audioService = container.Resolve<IAudioService>();
            
            AddBinding();
        }

        protected override void AddBinding()
        {
            _startGameButton.RxSubscribe(OnStartPressed).AddTo(this);
        }

        private void OnStartPressed()
        {
            _startScreenService.StartGamePressed();
            
            _audioService.ClickButton();
        }
    }
}