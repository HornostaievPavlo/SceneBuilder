using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Gameplay;
using UnityEngine;

namespace Services.SceneObjectsRegistry
{
	public class SceneObjectsRegistry : ISceneObjectsRegistry
	{
		private readonly Dictionary<string, SceneObject> _sceneObjectsById = new();

		public event Action<SceneObject> OnObjectRegistered;
		public event Action<SceneObject> OnObjectUnregistered;

		public void Register(SceneObject sceneObject)
		{
			if (_sceneObjectsById.ContainsKey(sceneObject.Id))
			{
				Debug.LogError($"Trying to register a SceneObject which is already registered: {sceneObject.name}");
				return;
			}
			
			_sceneObjectsById.Add(sceneObject.Id, sceneObject);
			OnObjectRegistered?.Invoke(sceneObject);
		}
	
		public void Unregister(SceneObject sceneObject)
		{
			if (_sceneObjectsById.ContainsKey(sceneObject.Id) == false)
			{
				Debug.LogError($"Trying to unregister a SceneObject that is not registered: {sceneObject.name}");
				return;
			}
			
			_sceneObjectsById.Remove(sceneObject.Id);
			OnObjectUnregistered?.Invoke(sceneObject);
		}
		
		public List<SceneObject> GetSceneObjects(AssetTypeId assetTypeId)
		{
			return _sceneObjectsById.Values.Where(sceneObject => sceneObject.AssetTypeId == assetTypeId).ToList();
		}
	}
}