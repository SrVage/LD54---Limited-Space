using Code.Abstract.Interfaces.UI.Common;
using UniRx;

namespace Code.Abstract.Interfaces.UI.Gameplay
{
    public interface IPlayerStatusService : IUIService
    {
        public ReactiveProperty<float> PlayerHealth { set; get; }
    }
}