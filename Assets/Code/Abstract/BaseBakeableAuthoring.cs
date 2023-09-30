using System.Collections.Generic;
using Code.Abstract.Interfaces;
using UnityEngine;

namespace Code.Abstract
{
    public abstract class BaseBakeableAuthoring : MonoBehaviour
    {
        [SerializeReference] public List<IEntityFeature> Features = new List<IEntityFeature>();
    }
}