using Code.ECS.Animations.Systems;
using Code.ECS.Input.Components;
using Code.ECS.Player.Components;
using Code.ECS.States.Components;
using Unity.Collections;
using Unity.Entities;

namespace Code.ECS.Input.Systems
{
    [UpdateAfter(typeof(BindInputSystem))]
    [UpdateBefore(typeof(PlayerAnimationSystem))]
    public partial struct AttackHandleSystem:ISystem
    {
        private EntityQuery _attackComponentQuery;
        private EntityQuery _attackEventQuery;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayState>();
            _attackComponentQuery = SystemAPI.QueryBuilder().WithAll<AttackComponent>().Build();
            _attackEventQuery = SystemAPI.QueryBuilder().WithAll<AttackEvent>().Build();
        }

        public void OnUpdate(ref SystemState state)
        {
            if (!_attackEventQuery.IsEmpty) 
                state.EntityManager.RemoveComponent<AttackEvent>(_attackEventQuery);
            if (_attackComponentQuery.IsEmpty)
                return;
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (player, entity) in SystemAPI.Query<PlayerComponent>()
                         .WithAll<PlayerComponent>()
                         .WithNone<AttackCooldownTImer>()
                         .WithEntityAccess())
            {
                ecb.AddComponent<AttackEvent>(entity);
                ecb.AddComponent(entity, new AttackCooldownTImer()
                {
                    Value = 2
                });
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
            state.EntityManager.DestroyEntity(_attackComponentQuery);
        }
    }
}