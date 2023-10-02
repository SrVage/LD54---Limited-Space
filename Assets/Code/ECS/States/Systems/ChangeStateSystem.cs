using System;
using Code.Abstract.Enums;
using Code.ECS.States.Components;
using Unity.Entities;
using UnityEngine;


namespace Code.ECS.States.Systems
{
    public partial struct ChangeStateSystem:ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ChangeState>();
            state.RequireForUpdate<MainState>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var entity = SystemAPI.GetSingletonEntity<MainState>();
            if (!SystemAPI.HasComponent<ExitState>(entity))
            {
                state.EntityManager.AddComponent<ExitState>(entity);
                return;
            }
            var newState = SystemAPI.GetSingleton<ChangeState>().Value;
            state.EntityManager.DestroyEntity(entity);
            var stateEntity = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponent<MainState>(stateEntity);
            state.EntityManager.AddComponent<EnterState>(stateEntity);
            switch (newState)
            {
                case State.MenuState:
                    state.EntityManager.AddComponent<MenuState>(stateEntity);
                    break;
                case State.PlayState:
                    state.EntityManager.AddComponent<PlayState>(stateEntity);
                    break;
                case State.RechargeState:
                    state.EntityManager.AddComponent<RechargeState>(stateEntity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            state.EntityManager.DestroyEntity(SystemAPI.GetSingletonEntity<ChangeState>());
            
            Debug.Log(newState);
        }
    }
}