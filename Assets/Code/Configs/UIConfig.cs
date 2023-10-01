using Code.Abstract;
using Code.UI;
using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "UIConfig", menuName = "Config/UIConfig")]
    public class UIConfig : ScriptableObject
    {
        [field: SerializeField] public UITag MainUI { private set; get; }
        [field: SerializeField] public BaseView[] UIViews { private set; get; }
    }
}