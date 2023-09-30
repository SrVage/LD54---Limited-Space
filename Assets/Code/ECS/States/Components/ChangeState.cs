using Unity.Entities;

namespace Code.ECS.States.Components
{
    public struct ChangeState:IComponentData
    {
        public Abstract.Enums.State Value;
    }
}