using Code.ECS.ServiceReferences;
using Code.ECS.States.Components;
using Code.ECS.States.Systems;
using Unity.Entities;
using UnityEngine;

namespace Code.ECS.States
{
    [UpdateAfter(typeof(ChangeStateSystem))]
    public partial struct HideUISystem:ISystem
    {
        private EntityQuery _menuExit;

        public void OnCreate(ref SystemState state)
        {
            _menuExit = SystemAPI.QueryBuilder().WithAll<MenuState>().Build();
            state.RequireForUpdate<ExitState>();
        }

        public void OnUpdate(ref SystemState state)
        {
            if (!_menuExit.IsEmpty)
            {
                foreach (var serviceReference in SystemAPI.Query<StartScreenServiceReference>())
                {
                    serviceReference.Value.MainUIEnable.Value = false;
                }
                
            }
        }
    }
}