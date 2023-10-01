using UniRx;

namespace Code.Abstract.Interfaces.UI.Common
{
    public interface IUIManager
    {
        ReactiveProperty<bool> IsCurrentState { get; set; }
    }
}