using System;
using Code.Abstract;
using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Config/AudioConfig")]
    public class AudioConfig : ScriptableObject
    {
        [field: SerializeField] public float MusicTransitionValue { private set; get; } = 2;
        
        [field: SerializeField] public BaseAudioTag[] AudioTags { private set; get; }
        [field: SerializeField] public BaseAudioListener BaseAudioListener { private set; get; }
        
        [field: Header("Menu")]
        [field: SerializeField] public AudioClipSettings ClickButton { private set; get; }
        [field: Header("Music")]
        [field: SerializeField] public AudioClipSettings MainMenuTheme { private set; get; }
        [field: SerializeField] public AudioClipSettings GameplayMusic { private set; get; }
        [field: Header("Gameplay")]
        [field: SerializeField] public AudioClipSettings Step { private set; get; }
        [field: SerializeField] public AudioClipSettings SecondStep { private set; get; }
        [field: SerializeField] public AudioClipSettings Hit { private set; get; }
        
        [Serializable]
        public class AudioClipSettings
        {
            [field: SerializeField] public AudioClip Audio { private set; get; }
            [field: SerializeField] [field: Range(0, 1)] public float VolumeScale { private set; get; } = 1;
        }
    }
}