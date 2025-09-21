using System;
using System.Collections.Generic;
using Enums;
using Gameplay;

namespace Services.SceneObjectsRegistry
{
	public interface ISceneObjectsRegistry
	{
		event Action<SceneObject> OnObjectRegistered;
		event Action<SceneObject> OnObjectUnregistered;
		void Register(SceneObject sceneObject);
		void Unregister(SceneObject sceneObject);
	}
}