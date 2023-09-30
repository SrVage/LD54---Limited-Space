using UniRx;

namespace Code.Abstract.Interfaces.UI.Common
{
    public interface IUIService
    {
        public ReactiveProperty<bool> MainUIEnable { set; get; }
    }
}