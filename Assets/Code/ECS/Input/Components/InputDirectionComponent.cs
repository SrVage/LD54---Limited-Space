using Unity.Entities;
using Unity.Mathematics;

namespace Client.Code.ECS.Input
{
    public struct InputDirectionComponent:IComponentData
    {
        public float2 Value;
    }
}