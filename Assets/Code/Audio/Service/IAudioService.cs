namespace Code.Audio.Service
{
    public interface IAudioService
    {
        void ClickButton();
        void SetMainMenuMusic();
        void SetGameplayMusic();
        void PlayStep();
        void PlayHit();
    }
}