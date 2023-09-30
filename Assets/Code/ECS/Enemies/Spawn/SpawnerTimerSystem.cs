using Unity.Burst;
using Unity.Entities;

namespace Code.ECS.Enemies.Spawn
{
    [BurstCompile]
    [UpdateInGroup(typeof(EnemiesSpawningSystemGroup))]
    public partial struct SpawnerTimerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EnemySpawnerComponent>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (RefRW<EnemySpawnerComponent> spawner in SystemAPI.Query<RefRW<EnemySpawnerComponent>>())
            {
                spawner.ValueRW.LevelTimer += SystemAPI.Time.DeltaTime;
                
                if (spawner.ValueRO.CurrentSpawnDelay > 0)
                {
                    spawner.ValueRW.CurrentSpawnDelay -= SystemAPI.Time.DeltaTime;
                }
            }
        }
    }
}