using System;
using Enums;
using Services.SceneObjectsRegistry;
using UnityEngine;
using Zenject;

namespace Gameplay
{
	public class SceneObject : MonoBehaviour
	{
		private AssetTypeId _assetTypeId;
		private string _guid;
		
		private ISceneObjectsRegistry _sceneObjectsRegistry;

		public AssetTypeId AssetTypeId => _assetTypeId;
		public string Id => _guid;

		[Inject]
		private void Construct(ISceneObjectsRegistry sceneObjectsRegistry)
		{
			_sceneObjectsRegistry = sceneObjectsRegistry;
		}

		private void Awake()
		{
			GenerateGuid();
			_sceneObjectsRegistry.Register(this);
		}

		private void OnDestroy()
		{
			_sceneObjectsRegistry.Unregister(this);
		}

		public void SetAssetType(AssetTypeId typeId)
		{
			_assetTypeId = typeId;
		}

		private void GenerateGuid()
		{
			_guid = Guid.NewGuid().ToString();
		}

		private void Start()
		{
			// rowsCoordinator = GetComponentInParent<RowsCoordinator>();
			// menuRow = rowsCoordinator.AssignRow(this);
		}

		// private void OnDestroy() => rowsCoordinator.RemoveRow(this);
	}
}