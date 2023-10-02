using Unity.Burst;
using Unity.Entities;

namespace Code.ECS.Enemies.Attacking
{
    public partial struct AttackCooldownSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<AttackableComponent>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var attackable in SystemAPI.Query<RefRW<AttackableComponent>>())
            {
                if (attackable.ValueRO.CurrentCooldown <= 0)
                {
                    continue;
                }

                attackable.ValueRW.CurrentCooldown -= SystemAPI.Time.DeltaTime;
            }
        }
    }
}