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
		private Transform _sceneObjectsHolder;
		private readonly Dictionary<string, SceneObject> _sceneObjectsById = new();

		public event Action<SceneObject> OnObjectRegistered;
		public event Action<SceneObject> OnObjectUnregistered;
		
		public Transform SceneObjectsHolder => _sceneObjectsHolder;

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
		
		public void RegisterSceneObjectsHolder(Transform assetsHolder)
		{
			_sceneObjectsHolder = assetsHolder;
		}
		
		public List<SceneObject> GetSceneObjects(SceneObjectTypeId sceneObjectTypeId)
		{
			return _sceneObjectsById.Values.Where(sceneObject => sceneObject.TypeId == sceneObjectTypeId).ToList();
		}

		public void DeleteObject(SceneObject sceneObject)
		{
			UnityEngine.Object.Destroy(sceneObject.gameObject);
		}
	}
}