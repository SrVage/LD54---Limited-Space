using Unity.Entities;

namespace Code.ECS.Enemies.Spawn
{
    public partial class EnemiesSpawningSystemGroup : ComponentSystemGroup
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            //RequireForUpdate<PlayState>();
        }
    }
}