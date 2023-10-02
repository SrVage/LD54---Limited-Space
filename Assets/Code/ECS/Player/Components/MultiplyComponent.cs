using Unity.Entities;

namespace Code.ECS.Player.Components
{
    public struct MultiplyComponent:IComponentData, IEnableableComponent
    {
        public float Value;
    }
}