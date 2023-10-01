namespace Code.Abstract.Interfaces.UI.Common
{
    public interface IUIFactory
    {
        T InstantiateView<T>() where T : BaseView<T>;
        void DisposeView<T>() where T : BaseView<T>;
    }
}