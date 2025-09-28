using Enums;
using Gameplay;
using UnityEngine;

namespace Services.Instantiation
{
	public interface IInstantiateService
	{
		T Instantiate<T>(GameObject prefab, Transform parent = null) where T : Component;
		SceneObject InstantiateSceneObject(GameObject prefab, Transform parent, SceneObjectTypeId typeId);
	}
}