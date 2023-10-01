using Unity.Mathematics;
using UnityEngine;

namespace Code.MapGenerator
{
    [CreateAssetMenu(fileName = "MapGeneratorConfig", menuName = "Config/MapGenerator/MapGeneratorConfig")]
    public class MapGeneratorConfig:ScriptableObject
    {
        [Header("Tiles")]
        public GameObject SimpleTilePrefab;
        public GameObject TrapTilePrefab;
        public GameObject SpeedDecreaseTilePrefab;
        public GameObject UnWalkTilePrefab;
        public GameObject StartTilePrefab;
        public GameObject SpawnTilePrefab;
        public int2 MapSize;
        public int3 TilesPercents;
        [Header("Walls")]
        public GameObject SimpleWallPrefab;
        public GameObject LightWallPrefab;
        public int WallPercents;
    }
}