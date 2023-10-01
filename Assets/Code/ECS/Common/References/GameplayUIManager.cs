using Code.Abstract;
using Code.Services.UI.Gameplay;
using Unity.Entities;

namespace Code.ECS.Common.References
{
    public class GameplayUIManager : ReferenceComponent<BaseUIManager<Gameplay>>, IComponentData
    {
        
    }
}