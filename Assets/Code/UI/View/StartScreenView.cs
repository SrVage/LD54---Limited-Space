using Code.Abstract;
using Code.Abstract.Extensions;
using Code.Abstract.Interfaces.UI.MainMenu;
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
        
        public override void Init(DiContainer container)
        {
            _startScreenService = container.Resolve<IStartScreenService>();
            
            AddBinding();
        }

        protected override void AddBinding()
        {
            _startGameButton.RxSubscribe(_startScreenService.StartGamePressed).AddTo(this);
        }
    }
}