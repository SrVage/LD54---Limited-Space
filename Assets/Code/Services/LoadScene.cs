using Code.Abstract.Enums;
using Code.ECS.States.Components;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Services
{
    public class LoadScene:MonoBehaviour
    {
        public static LoadScene Instance;
        private void Awake()
        {
            Instance = this;
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
            SceneManager.sceneUnloaded += SceneUnloaded;
            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void SceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.buildIndex!=1)
                return;
            World.DefaultGameObjectInjectionWorld.EntityManager.AddComponent<MainState>(World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity());
            var stateEntity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity();
            World.DefaultGameObjectInjectionWorld.EntityManager.AddComponent<ChangeState>(stateEntity);
            World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(stateEntity, new ChangeState()
            {
                Value = State.MenuState
            });
            //SceneManager.LoadScene(2,LoadSceneMode.Additive);
        }

        private void SceneUnloaded(Scene arg)
        {
            if (arg.buildIndex!=1)
                return;
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
            DefaultWorldInitialization.Initialize("World", false);
        }

        public void ReloadScene()
        {
            //World.DefaultGameObjectInjectionWorld.EntityManager.DestroyEntity(World.DefaultGameObjectInjectionWorld.EntityManager.UniversalQuery);
            World.DefaultGameObjectInjectionWorld.Dispose();
            SceneManager.UnloadSceneAsync(1);
            Time.timeScale = 1;
        }
        private void OnDestroy()
        {
            SceneManager.sceneUnloaded -= SceneUnloaded;
            SceneManager.sceneLoaded -= SceneLoaded;
        }
    }
}