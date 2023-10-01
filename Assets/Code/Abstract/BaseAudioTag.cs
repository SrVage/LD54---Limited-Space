using UnityEngine;

namespace Code.Abstract
{
    public abstract class BaseAudioTag : MonoBehaviour
    {
        [field: SerializeField] public AudioSource Source { protected set; get; }
        public float CurrentVolumeScale = 1;
    }
}