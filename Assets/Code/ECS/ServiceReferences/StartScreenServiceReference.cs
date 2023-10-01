using Code.Abstract.Interfaces.UI.MainMenu;
using Unity.Entities;


namespace Code.ECS.ServiceReferences
{
    public class StartScreenServiceReference:IComponentData
    {
        public IStartScreenService Value;
    }
}