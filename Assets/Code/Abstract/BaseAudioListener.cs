using UnityEngine;

namespace Code.Abstract
{
    public class BaseAudioListener : MonoBehaviour
    {
        [field: SerializeField] public AudioListener Source { private set; get; }
    }
}