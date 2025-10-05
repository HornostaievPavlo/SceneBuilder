using System;
using System.Collections.Generic;
using Enums;
using Gameplay;
using UnityEngine;

namespace Services.SceneObjectsRegistry
{
	public interface ISceneObjectsRegistry
	{
		event Action<SceneObject> OnObjectRegistered;
		event Action<SceneObject> OnObjectUnregistered;
		Transform SceneObjectsHolder { get; }
		void Register(SceneObject sceneObject);
		void Unregister(SceneObject sceneObject);
		void RegisterSceneObjectsHolder(Transform assetsHolder);
		List<SceneObject> GetSceneObjects(SceneObjectTypeId sceneObjectTypeId);
		void DeleteObject(SceneObject sceneObject);
	}
}