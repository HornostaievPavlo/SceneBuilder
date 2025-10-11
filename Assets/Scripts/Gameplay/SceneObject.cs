using System;
using Enums;
using Services.SceneObjectsRegistry;
using UnityEngine;
using Zenject;

namespace Gameplay
{
	public class SceneObject : MonoBehaviour
	{
		private SceneObjectTypeId _typeId;
		private string _guid;
		
		private ISceneObjectsRegistry _sceneObjectsRegistry;

		public SceneObjectTypeId TypeId => _typeId;
		public string Id => _guid;

		[Inject]
		private void Construct(ISceneObjectsRegistry sceneObjectsRegistry)
		{
			_sceneObjectsRegistry = sceneObjectsRegistry;
		}

		public void Register(SceneObjectTypeId typeId) 
		{
			GenerateGuid();
			SetTypeId(typeId);
			_sceneObjectsRegistry.Register(this);
		}

		private void OnDestroy()
		{
			if (_sceneObjectsRegistry == null)
				return;
			
			_sceneObjectsRegistry.Unregister(this);
		}

		private void SetTypeId(SceneObjectTypeId typeId)
		{
			_typeId = typeId;
		}

		private void GenerateGuid()
		{
			_guid = Guid.NewGuid().ToString();
		}
	}
}