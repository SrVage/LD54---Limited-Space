using Code.Abstract;
using UnityEngine;
using Zenject;

namespace Code.UI.View
{
    public class PlayerStatusView : BaseView<PlayerStatusView>
    {
        public override void Init(DiContainer container)
        {
            AddBinding();
        }

        protected override void AddBinding()
        {
            
        }
    }
}