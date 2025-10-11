using Enums;
using Gameplay;
using UnityEngine;
using Zenject;

namespace Services.Instantiation
{
    public class InstantiateService : IInstantiateService
    {
        private readonly IInstantiator _instantiator;
        private readonly DiContainer _container;

        public InstantiateService(IInstantiator instantiator, DiContainer container)
        {
            _instantiator = instantiator;
            _container = container;
        }
        
        public T Instantiate<T>(GameObject prefab, Transform parent = null) where T : Component
        {
            T instantiated = _instantiator.InstantiatePrefabForComponent<T>(prefab);
            instantiated.name = instantiated.name.Replace("(Clone)", string.Empty);
            
            instantiated.transform.position = Vector3.zero;
            instantiated.transform.rotation = Quaternion.identity;
            
            if (parent != null)
            {
                instantiated.transform.SetParent(parent, true);
            }

            return instantiated;
        }
        
        public SceneObject InstantiateSceneObject(GameObject prefab, Transform parent, SceneObjectTypeId typeId)
        {
            SceneObject instantiated = _instantiator.InstantiatePrefabForComponent<SceneObject>(prefab);
            instantiated.name = instantiated.name.Replace("(Clone)", string.Empty);
            
            instantiated.transform.position = Vector3.zero;
            instantiated.transform.rotation = Quaternion.identity;
            
            instantiated.transform.SetParent(parent, true);

            instantiated.Register(typeId);

            return instantiated;
        }

        public void AddSceneObjectComponent(GameObject existingGameObject, SceneObjectTypeId typeId)
        {
            SceneObject sceneObject = existingGameObject.AddComponent<SceneObject>();
            _container.InjectGameObject(existingGameObject);
            
            sceneObject.Register(typeId);
        }
    }
}