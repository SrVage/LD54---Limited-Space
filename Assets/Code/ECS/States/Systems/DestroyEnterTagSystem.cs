using Code.ECS.States.Components;
using Unity.Entities;

namespace Code.ECS.States.Systems
{
    [UpdateBefore(typeof(ChangeStateSystem))]
    public partial struct DestroyEnterTagSystem:ISystem
    {
        private EntityQuery _enterQuery;

        public void OnCreate(ref SystemState state)
        {
            _enterQuery = SystemAPI.QueryBuilder().WithAll<EnterState>().Build();
            state.RequireForUpdate<EnterState>();
        }

        public void OnUpdate(ref SystemState state)
        {
            state.EntityManager.RemoveComponent<EnterState>(_enterQuery);
        }
    }
}