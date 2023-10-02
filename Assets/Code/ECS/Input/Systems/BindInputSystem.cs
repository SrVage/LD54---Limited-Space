using Client.Code.ECS.Input;
using Code.ECS.Input.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.ECS.Input.Systems
{
    public partial class BindInputSystem:SystemBase
    {
        private InputAction _moveAction;
        private InputAction _attackAction;
        private float2 _inputValue;

        protected override void OnCreate()
        {
            _moveAction = new InputAction("move", binding: "<Gamepad>/rightStick");
            _moveAction.AddCompositeBinding("Dpad")
                .With("Up", binding: "<Keyboard>/w")
                .With("Down", binding: "<Keyboard>/s")
                .With("Left", binding: "<Keyboard>/a")
                .With("Right", binding: "<Keyboard>/d");
            _moveAction.performed += context => { CreateInputEntity(context.ReadValue<Vector2>()); };
            _moveAction.started += context => { CreateInputEntity(context.ReadValue<Vector2>()); };
            _moveAction.canceled += context => { CreateInputEntity(context.ReadValue<Vector2>()); };
            _moveAction.Enable();
            
            _attackAction = new InputAction("attack", binding: "<Keyboard>/q");
            _attackAction.performed += context => { Attack(); };
            _attackAction.started += context => { Attack(); };
            _attackAction.canceled += context => { Attack(); };
            _attackAction.Enable();
            
            
            var entity = EntityManager.CreateEntity();
            EntityManager.AddComponent<InputDirectionComponent>(entity);
        }
        

        private void CreateInputEntity(float2 readValue)
        {
            _inputValue = readValue;
        }

        private void Attack()
        {
             EntityManager.AddComponent<AttackComponent>(EntityManager.CreateEntity());
        }

        protected override void OnUpdate()
        {
            Entities.ForEach((ref InputDirectionComponent input) =>
            {
                input.Value = _inputValue;
            }).WithoutBurst().Run();
        }

        protected override void OnDestroy()
        {
            _moveAction.Dispose();
            _moveAction.Disable();
            _attackAction.Dispose();
            _attackAction.Disable();
        }
    }
}