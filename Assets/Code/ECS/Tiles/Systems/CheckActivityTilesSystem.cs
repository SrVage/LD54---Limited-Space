using System.Collections.Generic;
using Code.ECS.CommonComponents;
using Code.ECS.Enemies;
using Code.ECS.Player.Components;
using Code.ECS.Player.Systems;
using Code.ECS.States.Components;
using Code.ECS.Tiles.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Code.ECS.Tiles.Systems
{
    [UpdateBefore(typeof(PlayerMoveSystem))]
    public partial struct CheckActivityTilesSystem:ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayState>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var physicWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().PhysicsWorld;
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (transform, entity) in SystemAPI.Query<RefRO<LocalTransform>>()
                         .WithAll<SpeedComponent>()
                         .WithAny<PlayerComponent, EnemyComponent>()
                         .WithEntityAccess())
            {
                NativeList<DistanceHit> list = new NativeList<DistanceHit>(Allocator.TempJob);
                CastRayJob job = new CastRayJob()
                {
                    Position = transform.ValueRO.Position,
                    World = physicWorld,
                    RaycastHitRefList = list
                };
                job.Schedule().Complete();
                foreach (var comp in list)
                {
                   if (state.EntityManager.HasComponent<SpeedMultiplyer>(comp.Entity))
                   {
                       var multiplier = state.EntityManager.GetComponentData<SpeedMultiplyer>(comp.Entity).Value;
                       ecb.AddComponent(entity, new MultiplyComponent()
                       {
                           Value = multiplier
                       });
                   }
                   if (state.EntityManager.HasComponent<DamageComponent>(comp.Entity)&&state.EntityManager.IsComponentEnabled<DamageComponent>(comp.Entity))
                   {
                       var damage = state.EntityManager.GetComponentData<DamageComponent>(comp.Entity).Value;
                       ecb.AddComponent(entity, new HitComponent()
                       {
                           Value = damage
                       });
                   }
                }
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
    
    [BurstCompile]
    public struct CastRayJob : IJob
    {
        public float3 Position;
        [ReadOnly] public PhysicsWorld World;
        public NativeList<DistanceHit> RaycastHitRefList;

        public void Execute()
        {
            World.OverlapSphere(Position, 0.1f, ref RaycastHitRefList, CollisionFilter.Default);
        }
    }
}