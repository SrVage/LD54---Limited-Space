using Code.Abstract;
using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "UIConfig", menuName = "Config/UIConfig")]
    public class UIConfig : ScriptableObject
    {
        [field: SerializeField] public BaseView[] UIViews { private set; get; }
    }
}