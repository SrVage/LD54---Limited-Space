using Client.Code.ECS.Input;
using Code.ECS.Input.Systems;
using Code.ECS.Player.Components;
using Code.ECS.States.Components;
using Rukhanka;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Code.ECS.Animations.Systems
{
    [UpdateAfter(typeof(CheckInputSystem))]
    public partial struct PlayerAnimationSystem:ISystem
    {
        private uint _transitionHash;
        private FastAnimatorParameter _attackHash;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayState>();
            state.RequireForUpdate<InputDirectionComponent>();
            var transitionName = new FixedString512Bytes("Run");
            var attackName = new FixedString512Bytes("Attack");
            _transitionHash = transitionName.CalculateHash32();
            _attackHash = new FastAnimatorParameter(attackName);
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var player in SystemAPI.Query<AnimatorParametersAspect>().WithAll<PlayerComponent>())
            {
                player.SetParameterValue(_transitionHash, !SystemAPI.QueryBuilder().WithAll<MoveSignal>().Build().IsEmpty);
            }
            foreach (var player in SystemAPI.Query<AnimatorParametersAspect>().WithAll<PlayerComponent>().WithAll<AttackEvent>())
            {
                player.SetTrigger(_attackHash);
            }
        }
    }
}