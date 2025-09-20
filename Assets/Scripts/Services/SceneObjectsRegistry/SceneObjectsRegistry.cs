using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace Services.SceneObjectsRegistry
{
	public class SceneObjectsRegistry : ISceneObjectsRegistry
	{
		private readonly Dictionary<string, SceneObject> _sceneObjects = new();
	
		public void Register(SceneObject sceneObject)
		{
			if (_sceneObjects.ContainsKey(sceneObject.Id))
			{
				Debug.LogError($"Trying to register a SceneObject which is already registered: {sceneObject.name}");
				return;
			}
			
			_sceneObjects.Add(sceneObject.Id, sceneObject);
		}
	
		public void Unregister(SceneObject sceneObject)
		{
			if (_sceneObjects.ContainsKey(sceneObject.Id) == false)
			{
				Debug.LogError($"Trying to unregister a SceneObject that is not registered: {sceneObject.name}");
				return;
			}
			
			_sceneObjects.Remove(sceneObject.Id);
		}
	}
}