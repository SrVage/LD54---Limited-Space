using Code.Configs;

namespace Code.Abstract.Interfaces
{
    public interface IConfigReferenceService
    {
        UIConfig UIConfig { get; }
        LevelConfig LevelConfig { get; }
    }
}