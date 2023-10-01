namespace Code.Abstract.Interfaces.UI.Common
{
    public interface IUIPanelsShower
    {
        T ShowView<T>() where T : BaseView<T>;
        void HideView<T>() where T : BaseView<T>;
    }
}