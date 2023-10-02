using Code.ECS.States.Components;
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
            state.RequireForUpdate<PlayState>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var spawner in SystemAPI.Query<RefRW<EnemySpawnerComponent>>())
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