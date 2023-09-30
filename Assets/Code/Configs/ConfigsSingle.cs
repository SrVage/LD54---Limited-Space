using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "ConfigsSingle", menuName = "Config/ConfigsSingle")]
    public class ConfigsSingle : ScriptableObject
    {
        private static ConfigsSingle _instance = null;
        public static ConfigsSingle Instance => _instance;
        
        [field: SerializeField] public LevelConfig LevelConfig { private set; get; }
        [field: SerializeField] public UIConfig UIConfig { private set; get; }
        
        public static void Init(ConfigsSingle configsSingle)
        {
            _instance = configsSingle;
        }
    }
}