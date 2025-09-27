using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enums;
using Gameplay;
using GLTFast;
using Services.Instantiation;
using UnityEngine;
using Zenject;

namespace Services.Loading
{
	public class LoadService : ILoadService
	{
		// prefabs paths to constants
		// [SerializeField] private GameObject modelAssetHolderPrefab;
		// [SerializeField] private GameObject cameraAssetPrefab;
		// [SerializeField] private GameObject labelAssetPrefab;

		private List<Transform> modelsFromSingleSaveFile = new();

		private Transform[] children;

		private IInstantiateService _instantiateService;

		[Inject]
		private void Construct(IInstantiateService instantiateService)
		{
			_instantiateService = instantiateService;
		}

        /// <summary>
        ///     Handles loading assets from local save file
        /// </summary>
        /// <param name="sceneNumber">Index of save file</param>
        public async void LoadAssetsFromSaveFile(int sceneNumber)
		{
			string saveFilePath =
				IOUtility.scenePath + sceneNumber.ToString() + IOUtility.sceneFile;

			bool success = await LoadModel(saveFilePath);
			if (success) AssignTextures(sceneNumber);
		}

        /// <summary>
        ///     Adds camera asset to a scene
        /// </summary>
        public void LoadCameraAsset() => CreateAsset(AssetTypeId.Camera);

        /// <summary>
        ///     Adds label asset to a scene
        /// </summary>
        public void LoadLabelAsset() => CreateAsset(AssetTypeId.Label);

        /// <summary>
        ///     General model loading procedure.
        ///     Handles adding of colliders to models
        /// </summary>
        /// <param name="modelPath">Path to .glb file in local storage</param>
        /// <returns>Success of loading</returns>
        public async Task<bool> LoadModel(string modelPath)
		{
			if (string.IsNullOrEmpty(modelPath))
			{
				// Debug.LogError($"Trying to load model from empty path");
				// return false;
				modelPath = IOUtility.duckModelPath;
			}

			GameObject modelAsset = CreateAsset(AssetTypeId.Model);
			var gltfAsset = modelAsset.AddComponent<GltfAsset>();

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

				modelAsset.transform.SetParent(IOUtility.assetsParent);
				AddColliders(modelAsset);

				// assets = modelAsset.GetComponentsInChildren<Transform>().ToList();
			}

			// check if it is needed on save/load refactor
			// AddCollidersToAssets(assets);

			return true;
		}

        /// <summary>
        ///     Creates new selectable asset of specified type
        /// </summary>
        /// <param name="typeId">Specifies AssetType of created object</param>
        /// <returns>Created instance</returns>
        private GameObject CreateAsset(AssetTypeId typeId)
		{
			return new GameObject();

			// switch (typeId)
			// {
			//     case AssetTypeId.Model:
			//
			//         var model = _instantiateService.Instantiate<SceneObject>(modelAssetHolderPrefab);
			//         model.SetAssetType(AssetTypeId.Model);
			//         
			//         return model.gameObject;
			//
			//     case AssetTypeId.Camera:
			//
			//         GameObject camera = Instantiate(cameraAssetPrefab, IOUtility.assetsParent);
			//         camera.name = "Asset";
			//
			//         var cameraScene = camera.AddComponent<SceneObject>();
			//         cameraScene.SetAssetType(typeId);
			//         return camera;
			//
			//     case AssetTypeId.Label:
			//
			//         GameObject label = Instantiate(labelAssetPrefab, IOUtility.assetsParent);
			//         label.name = "Asset";
			//
			//         var labelScene = label.AddComponent<SceneObject>();
			//         labelScene.SetAssetType(typeId);
			//         return label;
			//
			//     default: return null;
			// }
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
				sceneObj.SetParent(IOUtility.assetsParent);
			}

			// some Destroy???
			var spawner = IOUtility.assetsParent.gameObject.GetComponentInChildren<GltfAsset>();
			// Destroy(spawner.gameObject.GetComponent<SceneObject>());
			spawner.gameObject.name = "glTF Asset";

			GltfAsset[] spawners = IOUtility.assetsParent.gameObject.GetComponentsInChildren<GltfAsset>();
			foreach (GltfAsset item in spawners)
			{
				// Destroy(item.gameObject.GetComponent<SceneObject>());
				item.gameObject.name = "glTF Asset";
			}

			Array.Clear(children, 0, children.Length);
			children = IOUtility.assetsParent.gameObject.GetComponentsInChildren<Transform>();

			for (int i = 0; i < children.Length; i++)
			{
				if (children[i].gameObject.name == "Asset") resultList.Add(children[i]);
			}

			foreach (var asset in resultList)
			{
				asset.SetParent(IOUtility.assetsParent);

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
						string currentAssetPath = $"/Asset{i + 1}" + IOUtility.textureFile;
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

		private void AddColliders(GameObject asset)
		{
			MeshRenderer[] meshRenderers = asset.GetComponentsInChildren<MeshRenderer>();

			foreach (MeshRenderer meshRenderer in meshRenderers)
			{
				var meshCollider = meshRenderer.gameObject.AddComponent<MeshCollider>();
				meshCollider.convex = true;
			}
		}
	}
}