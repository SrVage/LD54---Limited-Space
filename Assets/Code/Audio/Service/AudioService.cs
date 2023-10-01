using System.Threading;
using System.Threading.Tasks;
using Code.Abstract;
using Code.Configs;
using Code.ECS.Common.References;
using UniRx;
using Unity.Entities;
using UnityEngine;
using Zenject;

namespace Code.Audio.Service
{
    public class AudioService : IAudioService
    {
        private CompositeDisposable _disposables = new CompositeDisposable();
        
        private AudioConfig _audioConfig;

        private AudioTag _audioDirectory;
        private MusicTag _music;
        private SoundTag _sound;
        private BaseAudioListener _audioListener;
        
        private bool _secondStep = false;
        
        private CancellationTokenSource _fadeInOutCancellationTokenSource;
        
        private float _timeToTransitionMaxValue;
        private float _timeToTransitionHalfMaxValue;
        private float _currentTransitionTime = 0;
        
        private float _maxOldAmbientVolume;
        private float _maxNewAmbientVolume;
        
        private bool _firstWork = true;
        private bool _clipIsChanged = false;
        
        [Inject]
        public AudioService(
            AudioConfig audioConfig
            )
        {
            _audioConfig = audioConfig;

            _timeToTransitionMaxValue = _audioConfig.MusicTransitionValue;
            _timeToTransitionHalfMaxValue = _timeToTransitionMaxValue / 2;
            
            FindOrCreateAudioDirectory();
            
            InstantiateAudioMonoBehaviours();
            
            SetStartVolume();
            
            var entity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity();
            World.DefaultGameObjectInjectionWorld.EntityManager.AddComponent<AudioServiceReference>(entity);
            World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(entity, new AudioServiceReference()
            {
                Value = this
            });
        }
        
        private void FindOrCreateAudioDirectory()
        {
            _audioDirectory = GameObject.FindObjectOfType<AudioTag>();

            if (_audioDirectory != null) 
                GameObject.Destroy(_audioDirectory.gameObject);
            _audioDirectory = new GameObject("Audio").AddComponent<AudioTag>();
        }
        
        private void InstantiateAudioMonoBehaviours()
        {
            foreach (var audioTag in _audioConfig.AudioTags)
            {
                if (audioTag.GetType() == typeof(MusicTag))
                {
                    _music = GameObject.Instantiate((MusicTag)audioTag, _audioDirectory.transform);
                }
                
                if (audioTag.GetType() == typeof(SoundTag))
                {
                    _sound = GameObject.Instantiate((SoundTag)audioTag, _audioDirectory.transform);
                }
            }
            
            _audioListener = GameObject.Instantiate(_audioConfig.BaseAudioListener, _audioDirectory.transform);
        }
        
        private void SetStartVolume()
        {
            _music.Source.volume = 1;
            _music.CurrentVolumeScale = 1;
            _sound.Source.volume = 1;
            _sound.CurrentVolumeScale = 1;
        }
        
        public void ClickButton()
        {
            PlayOnShotWithVolumeChangeng(_sound, _audioConfig.ClickButton);
        }

        public void SetMainMenuMusic()
        {
            _music.Source.loop = true;
            
            PlayWithVolumeChanging(_music, _audioConfig.MainMenuTheme);
        }

        public void SetGameplayMusic()
        {
            _music.Source.loop = true;
            
            PlayWithVolumeChanging(_music, _audioConfig.GameplayMusic);
        }

        public void PlayStep()
        {
            PlayOnShotWithVolumeChangeng(_sound, _secondStep ? _audioConfig.SecondStep : _audioConfig.Step);
            
            _secondStep = !_secondStep;
        }

        public void PlayHit()
        {
            PlayOnShotWithVolumeChangeng(_sound, _audioConfig.Hit);
        }
        
        private void PlayOnShotWithVolumeChangeng(BaseAudioTag audio, AudioConfig.AudioClipSettings audioClip)
        {
            audio.Source.PlayOneShot(audioClip.Audio, audioClip.VolumeScale);
        }
        
        private async void PlayWithVolumeChanging(BaseAudioTag audio, AudioConfig.AudioClipSettings audioClip, float settingsValue = 1, bool forcedChanging = false)
        {
            if (audio.Source.clip == audioClip.Audio && !forcedChanging)
            {
                return;
            }

            if (audio.Source.clip == null)
            {
                audio.Source.clip = audioClip.Audio;
                audio.Source.volume = audioClip.VolumeScale * settingsValue;
                audio.CurrentVolumeScale = audioClip.VolumeScale;
                audio.Source.Play();
                
                return;
            }

            ClearCancellationToken();

            _fadeInOutCancellationTokenSource = new CancellationTokenSource();

            Task<bool> fadeInTask = FadeIn(audio, _fadeInOutCancellationTokenSource.Token);
            bool isFadedIn = await fadeInTask;

            if (!isFadedIn)
                return;
            audio.Source.clip = audioClip.Audio;
            audio.CurrentVolumeScale = audioClip.VolumeScale;
            audio.Source.Play();

            await FadeOut(audio, audioClip, settingsValue, _fadeInOutCancellationTokenSource.Token);
        }

        private async Task<bool> FadeIn(BaseAudioTag audio, CancellationToken token)
        {
            _maxOldAmbientVolume = audio.Source.volume;
            _currentTransitionTime = 0;

            while (_currentTransitionTime < _timeToTransitionHalfMaxValue)
            {
                if (token.IsCancellationRequested)
                {
                    return false;
                }
                
                audio.Source.volume = _maxOldAmbientVolume -
                                      (_maxOldAmbientVolume * ( 1 * (_currentTransitionTime / _timeToTransitionHalfMaxValue)));
                
                _currentTransitionTime += 0.1f;
                
                await Task.Delay(100);
            }

            return true;
        }

        private async Task FadeOut(BaseAudioTag audio, AudioConfig.AudioClipSettings audioClip, float settingsValue, CancellationToken token)
        {
            _maxNewAmbientVolume = audioClip.VolumeScale * settingsValue;
            _currentTransitionTime = 0;
            
            while (_currentTransitionTime < _timeToTransitionHalfMaxValue)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                
                audio.Source.volume = _maxNewAmbientVolume * (1 * (_currentTransitionTime / _timeToTransitionHalfMaxValue));
                
                _currentTransitionTime += 0.1f;
                
                await Task.Delay(100);
            }
        }

        private void ClearCancellationToken()
        {
            if (_fadeInOutCancellationTokenSource != null)
            {
                _fadeInOutCancellationTokenSource.Cancel();
                _fadeInOutCancellationTokenSource.Dispose();
                _fadeInOutCancellationTokenSource = null;
            }
        }
    }
}