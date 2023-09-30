using System;
using Code.Abstract.Interfaces;
using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Config/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        public EnemiesCount Enemies;
        
        [Serializable]
        public struct EnemiesCount
        {
            public int Cap;
            public int SpawnPerTime;
            public int PerHalfMinuteMultiply;
            [SerializeReference] public IEntityTag[] Tags;
        }
    }
}