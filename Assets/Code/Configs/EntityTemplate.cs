using System;
using System.Collections;
using System.Collections.Generic;
using Code.Abstract.Interfaces.Entity;
using UnityEngine;

namespace Code.Configs
{
    [Serializable]
    [CreateAssetMenu(fileName = nameof(EntityTemplate), menuName = "Config/Templates/EntityTemplate")]
    public class EntityTemplate : ScriptableObject, IEnumerable<IEntityFeature>
    {
        [field: SerializeField] public string Name { private set; get; }
        
        [SerializeReference] private List<IEntityFeature> _features = new List<IEntityFeature>();

        public IEnumerator<IEntityFeature> GetEnumerator()
        {
            return _features.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}