using Code.Abstract.Interfaces.UI.Common;
using UniRx;

namespace Code.Abstract.Interfaces.UI.Gameplay
{
    public interface IPlayerStatusService : IUIService
    {
        public ReactiveProperty<int> PlayerMaxHealth { set; get; }
        public ReactiveProperty<int> PlayerCurrentHealth { set; get; }
    }
}