using Unity.Entities;
using UnityEngine;

namespace Code.ECS.Common
{
    public abstract class CommonComponents : MonoBehaviour
    {
        public virtual void Bake(IBaker baker, Entity entity)
        {
            
        }
    }
}