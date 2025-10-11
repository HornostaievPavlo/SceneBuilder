using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enums;
using Gameplay;
using GLTFast;
using LocalSaves;
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

			SceneObject modelAsset = InstantiateSceneObject(SceneObjectTypeId.Model);
			var gltfAsset = modelAsset.gameObject.AddComponent<GltfAsset>();

			bool isSuccess = await gltfAsset.Load(modelPath);

			if (isSuccess == false)
				return false;

			List<Transform> assets;

			if (modelPath.Contains(Constants.DataPath))
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
	        InstantiateSceneObject(SceneObjectTypeId.Camera);
        }

		public void LoadLabel()
        {
	        InstantiateSceneObject(SceneObjectTypeId.Label);
        }

		private SceneObject InstantiateSceneObject(SceneObjectTypeId typeId)
		{
			// try to remove switch 
			switch (typeId)
			{
			    case SceneObjectTypeId.Model:
			    {
				    var modelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(Constants.ModelPrefabPath);
				    SceneObject model = _instantiateService.InstantiateSceneObject(modelPrefab, _sceneObjectsRegistry.SceneObjectsHolder, typeId);
			        
				    return model;
			    }
			    case SceneObjectTypeId.Camera:
			    {
				    var cameraPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(Constants.CameraPrefabPath);
				    SceneObject camera = _instantiateService.InstantiateSceneObject(cameraPrefab, _sceneObjectsRegistry.SceneObjectsHolder, typeId);
			        
				    camera.name = "Asset";
					
				    return camera;
			    }
			    case SceneObjectTypeId.Label:
			    {
				    var labelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(Constants.LabelPrefabPath);
				    SceneObject label = _instantiateService.InstantiateSceneObject(labelPrefab, _sceneObjectsRegistry.SceneObjectsHolder, typeId);
					
				    label.name = "Asset";
				    
				    return label;
			    }
			    default:
			    {
				    return null;
			    }
			}
		}

		public async void LoadLocalSave(LocalSave localSave)
		{
			string assetPath = localSave.DirectoryPath + Constants.AssetFile;

			bool isLoadedSuccessfully = await LoadModel(assetPath);
			
			if (isLoadedSuccessfully)
			{
				// AssignTextures(sceneNumber);
			}
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
					selectable.SetTypeId(SceneObjectTypeId.Model);

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
			// for (int i = 0; i < modelsFromSingleSaveFile.Count; i++)
			// {
			// 	Renderer renderer = modelsFromSingleSaveFile[i].gameObject.GetComponentInChildren<MeshRenderer>();
			//
			// 	if (renderer != null)
			// 	{
			// 		Material material = renderer.material;
			//
			// 		if (material != null)
			// 		{
			// 			string currentAssetPath = $"/Asset{i + 1}" + Constants.TextureFile;
			// 			string fullPath = Constants.ScenePath + sceneNumber + currentAssetPath;
			//
			// 			material.mainTexture = IOUtility.OpenDirectoryAndLoadTexture(fullPath);
			// 		}
			// 	}
			// }
			//
			// modelsFromSingleSaveFile.Clear();
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