using Code.Abstract.Interfaces.UI.Gameplay;
using UniRx;

namespace Code.Services.UI.Gameplay.Panels
{
    public class PlayerStatusService : IPlayerStatusService
    {
        public ReactiveProperty<bool> MainUIEnable { get; set; } = new ReactiveProperty<bool>();
        public ReactiveProperty<float> PlayerHealth { get; set; } = new ReactiveProperty<float>();
    }
}