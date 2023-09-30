using UnityEngine;
using Zenject;

namespace Code.Abstract
{
    public abstract class BaseView : MonoBehaviour
    {
        [SerializeField] protected Canvas _selfCanvas;

        public abstract void Init(DiContainer container);

        protected abstract void AddBinding();

        protected void OnUIEnableChanged(bool value)
        {
            if (value)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        public virtual void Show()
        {
            _selfCanvas.enabled = true;
        }

        public virtual void Hide()
        {
            _selfCanvas.enabled = false;
        }
    }

    public abstract class BaseView<T> : BaseView where T : BaseView<T>
    {

    }
}