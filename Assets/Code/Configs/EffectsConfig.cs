using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "EffectConfig", menuName = "Config/EffectConfig")]
    public class EffectsConfig:ScriptableObject
    {
        public GameObject SandPrefab;
    }
}