using Unity.Entities;

namespace Code.ECS.Input.Components
{
    public struct AttackCooldownTImer:IComponentData
    {
        public float Value;
    }
}