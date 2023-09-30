using Code.Abstract.Enums;
using Code.ECS.States.Components;
using Unity.Entities;
using UnityEngine;

namespace Code.ECS.Helper
{
    public partial struct ChangeStateFromKeyboard:ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                var entity = state.EntityManager.CreateEntity();
                state.EntityManager.AddComponent<ChangeState>(entity);
                state.EntityManager.SetComponentData(entity, new ChangeState()
                {
                    Value = State.PlayState
                });
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                var entity = state.EntityManager.CreateEntity();
                state.EntityManager.AddComponent<ChangeState>(entity);
                state.EntityManager.SetComponentData(entity, new ChangeState()
                {
                    Value = State.MenuState
                });
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                var entity = state.EntityManager.CreateEntity();
                state.EntityManager.AddComponent<ChangeState>(entity);
                state.EntityManager.SetComponentData(entity, new ChangeState()
                {
                    Value = State.RechargeState
                });
            }
        }
    }
}