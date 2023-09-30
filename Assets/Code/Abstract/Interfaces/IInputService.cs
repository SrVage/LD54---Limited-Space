using System;
using Code.Abstract.Interfaces.UI.Common;
using UniRx;
using UnityEditor.Timeline.Actions;

namespace Code.Abstract.Interfaces
{
    public interface IInputService : IUIService
    {
        public ReactiveProperty<ActionContext> ActionContext { set; get; }
        public ReactiveProperty<Action> ActionButtonsAction { get; }
    }
}