using Code.Audio.Service;
using Unity.Entities;

namespace Code.ECS.Common.References
{
    public class AudioServiceReference : ReferenceComponent<IAudioService>, IComponentData
    {
        
    }
}