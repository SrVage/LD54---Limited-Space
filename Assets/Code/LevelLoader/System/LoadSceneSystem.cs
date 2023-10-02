using Code.ECS.ServiceReferences;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Scenes;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Code.LevelLoader.System
{
    public partial struct LoadSceneSystem:ISystem
    {
        private EntityQuery _newRequests;
        public void OnCreate(ref SystemState state)
        {
            _newRequests = state.GetEntityQuery(typeof(SceneLoaderComponent));
        }

        public void OnUpdate(ref SystemState state)
        {
            var requests = _newRequests.ToComponentDataArray<SceneLoaderComponent>(Allocator.Temp);

            for (int i = 0; i < requests.Length; i += 1)
            {
               SceneSystem.LoadSceneAsync(World.DefaultGameObjectInjectionWorld.Unmanaged, requests[i].Value, new SceneSystem.LoadParameters()
               {
                   AutoLoad = true
               });
            }
            requests.Dispose();
            state.EntityManager.DestroyEntity(_newRequests);
        }
    }
}