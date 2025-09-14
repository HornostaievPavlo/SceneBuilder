using UnityEngine;
using Zenject;

namespace Services.Instantiation
{
    public class InstantiateService : IInstantiateService
    {
        private readonly IInstantiator _instantiator;

        public InstantiateService(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }
        
        public T Instantiate<T>(GameObject prefab) where T : Component
        {
            T instantiated = _instantiator.InstantiatePrefabForComponent<T>(prefab);
            instantiated.name = instantiated.name.Replace("(Clone)", string.Empty);
            
            instantiated.transform.position = Vector3.zero;
            instantiated.transform.rotation = Quaternion.identity;
            
            return instantiated;
        }
    }
}