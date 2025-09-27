using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enums;
using Gameplay;
using GLTFast;
using Services.Instantiation;
using Services.SceneObjectsRegistry;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Services.Loading
{
	public class LoadService : ILoadService
	{
		private List<Transform> modelsFromSingleSaveFile = new();

		private Transform[] children;

		private IInstantiateService _instantiateService;
		private ISceneObjectsRegistry _sceneObjectsRegistry;

		[Inject]
		private void Construct(IInstantiateService instantiateService, ISceneObjectsRegistry sceneObjectsRegistry)
		{
			_instantiateService = instantiateService;
			_sceneObjectsRegistry = sceneObjectsRegistry;
		}

		public async Task<bool> LoadModel(string modelPath)
		{
			if (string.IsNullOrEmpty(modelPath))
			{
				// Debug.LogError($"Trying to load model from empty path");
				// return false;
				
				modelPath = Constants.DuckModelPath;
			}

			SceneObject modelAsset = InstantiateSceneObject(AssetTypeId.Model);
			var gltfAsset = modelAsset.gameObject.AddComponent<GltfAsset>();

			bool isSuccess = await gltfAsset.Load(modelPath);

			if (isSuccess == false)
				return false;

			List<Transform> assets;

			if (modelPath.Contains(IOUtility.dataPath))
			{
				// check if it is needed on save/load refactor
				// assets = InitializeImportedAssets();
			}
			else
			{
				// List<Transform> assets = new();
				// Transform[] children = IOUtility.assetsParent.gameObject.GetComponentsInChildren<Transform>();
				//
				// for (int i = 0; i < children.Length; i++)
				// {
				//     if (children[i].gameObject.name == "Asset")
				//     {
				//         assets.Add(children[i]);
				//     }
				// }

				AddColliders(modelAsset);

				// assets = modelAsset.GetComponentsInChildren<Transform>().ToList();
			}

			// check if it is needed on save/load refactor
			// AddCollidersToAssets(assets);

			return true;
		}

		public void LoadCamera()
        {
	        InstantiateSceneObject(AssetTypeId.Camera);
        }

		public void LoadLabel()
        {
	        InstantiateSceneObject(AssetTypeId.Label);
        }

		private SceneObject InstantiateSceneObject(AssetTypeId typeId)
		{
			// try to remove switch 
			switch (typeId)
			{
			    case AssetTypeId.Model:
			    {
				    var modelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(Constants.ModelPrefabPath);
				    var model = _instantiateService.Instantiate<SceneObject>(modelPrefab, _sceneObjectsRegistry.SceneObjectsHolder);
			        
				    model.SetAssetType(AssetTypeId.Model);
			        
				    return model;
			    }
			    case AssetTypeId.Camera:
			    {
				    var cameraPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(Constants.CameraPrefabPath);
				    var camera = _instantiateService.Instantiate<SceneObject>(cameraPrefab, _sceneObjectsRegistry.SceneObjectsHolder);
			        
				    camera.name = "Asset";
				    camera.SetAssetType(AssetTypeId.Camera);
					
				    return camera;
			    }
			    case AssetTypeId.Label:
			    {
				    var labelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(Constants.LabelPrefabPath);
				    var label = _instantiateService.Instantiate<SceneObject>(labelPrefab, _sceneObjectsRegistry.SceneObjectsHolder);
					
				    label.name = "Asset";
				    label.SetAssetType(AssetTypeId.Label);
				    
				    return label;
			    }
			    default:
			    {
				    Debug.LogError($"Trying to create scene object of unsupported type: {typeId}");
				    return null;
			    }
			}
		}

		/// <summary>
		///     Handles loading assets from local save file
		/// </summary>
		/// <param name="sceneNumber">Index of save file</param>
		public async void LoadAssetsFromSaveFile(int sceneNumber)
		{
			string saveFilePath =
				IOUtility.scenePath + sceneNumber + Constants.SceneFile;

			bool success = await LoadModel(saveFilePath);
			if (success) AssignTextures(sceneNumber);
		}

		/// <summary>
        ///     Rearranges imported assets in proper hierarchy
        /// </summary>
        /// <returns>Collection of assets in scene</returns>
        private List<Transform> InitializeImportedAssets()
		{
			List<Transform> resultList = new();

			modelsFromSingleSaveFile.Clear();

			bool isSingleAsset = GameObject.Find("Scene") == null;
			Transform sceneObj = null;

			if (!isSingleAsset)
			{
				sceneObj = GameObject.Find("Scene").transform;
				sceneObj.SetParent(_sceneObjectsRegistry.SceneObjectsHolder);
			}

			// some Destroy???
			var spawner = _sceneObjectsRegistry.SceneObjectsHolder.gameObject.GetComponentInChildren<GltfAsset>();
			// Destroy(spawner.gameObject.GetComponent<SceneObject>());
			spawner.gameObject.name = "glTF Asset";

			GltfAsset[] spawners = _sceneObjectsRegistry.SceneObjectsHolder.gameObject.GetComponentsInChildren<GltfAsset>();
			foreach (GltfAsset item in spawners)
			{
				// Destroy(item.gameObject.GetComponent<SceneObject>());
				item.gameObject.name = "glTF Asset";
			}

			Array.Clear(children, 0, children.Length);
			children = _sceneObjectsRegistry.SceneObjectsHolder.gameObject.GetComponentsInChildren<Transform>();

			for (int i = 0; i < children.Length; i++)
			{
				if (children[i].gameObject.name == "Asset") resultList.Add(children[i]);
			}

			foreach (var asset in resultList)
			{
				asset.SetParent(_sceneObjectsRegistry.SceneObjectsHolder);

				bool hasSelectable = asset.GetComponentInChildren<SceneObject>() != null;

				if (!hasSelectable)
				{
					var selectable = asset.gameObject.AddComponent<SceneObject>();
					selectable.SetAssetType(AssetTypeId.Model);

					modelsFromSingleSaveFile.Add(asset.transform);
				}
			}

			if (sceneObj)
			{
				// Destroy(sceneObj.gameObject);
			}

			return resultList;
		}

		/// <summary>
        ///     Assigns textures to corresponding materials
        /// </summary>
        private void AssignTextures(int sceneNumber)
		{
			for (int i = 0; i < modelsFromSingleSaveFile.Count; i++)
			{
				Renderer renderer = modelsFromSingleSaveFile[i].gameObject.GetComponentInChildren<MeshRenderer>();

				if (renderer != null)
				{
					Material material = renderer.material;

					if (material != null)
					{
						string currentAssetPath = $"/Asset{i + 1}" + Constants.TextureFile;
						string fullPath = IOUtility.scenePath + sceneNumber + currentAssetPath;

						material.mainTexture = IOUtility.OpenDirectoryAndLoadTexture(fullPath);
					}
				}
			}

			modelsFromSingleSaveFile.Clear();
		}

		/// <summary>
        ///     Adds convex mesh collider to
        ///     all renderers in List of targets
        /// </summary>
        /// <param name="assets">Collection of targets</param>
        private void AddCollidersToAssets(List<Transform> assets)
		{
			foreach (var asset in assets)
			{
				MeshRenderer[] meshRenderers = asset.gameObject.GetComponentsInChildren<MeshRenderer>();

				foreach (MeshRenderer meshRenderer in meshRenderers)
				{
					var meshCollider = meshRenderer.gameObject.AddComponent<MeshCollider>();
					meshCollider.convex = true;
				}
			}
		}

		private void AddColliders(SceneObject sceneObject)
		{
			MeshRenderer[] meshRenderers = sceneObject.gameObject.GetComponentsInChildren<MeshRenderer>();

			foreach (MeshRenderer meshRenderer in meshRenderers)
			{
				var meshCollider = meshRenderer.gameObject.AddComponent<MeshCollider>();
				meshCollider.convex = true;
			}
		}
	}
}