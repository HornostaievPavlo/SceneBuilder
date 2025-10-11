using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Object = UnityEngine.Object;

namespace Services.Loading
{
	public class LoadService : ILoadService
	{
		// private List<Transform> modelsFromSingleSaveFile = new();

		// private Transform[] children;

		private IInstantiateService _instantiateService;
		private ISceneObjectsRegistry _sceneObjectsRegistry;

		[Inject]
		private void Construct(IInstantiateService instantiateService, ISceneObjectsRegistry sceneObjectsRegistry)
		{
			_instantiateService = instantiateService;
			_sceneObjectsRegistry = sceneObjectsRegistry;
		}

		public async Task<bool> LoadModel(string modelPath, string localSaveDirectoryPath = "")
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

			bool isLoadedFromLocalSave = modelPath.Contains(Constants.ApplicationDataPath);

			if (isLoadedFromLocalSave)
			{
				// SetupLocalSaveAssets(localSaveDirectoryPath);
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

				// AddColliders(modelAsset);

				// assets = modelAsset.GetComponentsInChildren<Transform>().ToList();
			}
			
			AddColliders(modelAsset);

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
			await LoadModel(assetPath, localSave.DirectoryPath);
		}

        private void SetupLocalSaveAssets(string localSaveDirectoryPath)
		{
			List<Transform> resultList = new();
			List<Transform> localSaveModels = new();
			List<Transform> children = new();

			bool isSingleAsset = GameObject.Find("Scene") == null;
			Transform sceneObj = null;

			if (!isSingleAsset)
			{
				sceneObj = GameObject.Find("Scene").transform;
				sceneObj.SetParent(_sceneObjectsRegistry.SceneObjectsHolder);
			}

			var spawner = _sceneObjectsRegistry.SceneObjectsHolder.gameObject.GetComponentInChildren<GltfAsset>();
			Object.Destroy(spawner.gameObject.GetComponent<SceneObject>());
			spawner.gameObject.name = "glTF Asset";

			GltfAsset[] spawners = _sceneObjectsRegistry.SceneObjectsHolder.gameObject.GetComponentsInChildren<GltfAsset>();
			foreach (GltfAsset item in spawners)
			{
				Object.Destroy(item.gameObject.GetComponent<SceneObject>());
				item.gameObject.name = "glTF Asset";
			}

			children = _sceneObjectsRegistry.SceneObjectsHolder.gameObject.GetComponentsInChildren<Transform>().ToList();

			for (int i = 0; i < children.Count; i++)
			{
				if (children[i].gameObject.name == "Asset") resultList.Add(children[i]);
			}

			foreach (var asset in resultList)
			{
				asset.SetParent(_sceneObjectsRegistry.SceneObjectsHolder);

				bool hasSelectable = asset.GetComponentInChildren<SceneObject>() != null;

				if (hasSelectable == false)
				{
					SceneObject sceneObject = asset.gameObject.AddComponent<SceneObject>();
					sceneObject.Register(SceneObjectTypeId.Model);

					localSaveModels.Add(asset.transform);
				}
			}

			if (sceneObj)
			{
				Object.Destroy(sceneObj.gameObject);
			}
			
			// AssignTextures(localSaveModels, localSaveDirectoryPath);
			localSaveModels.Clear();
		}

        // private void AssignTextures(int sceneNumber)
        private void AssignTextures(List<Transform> localSaveModels, string localSaveDirectoryPath)
		{
			for (int i = 0; i < localSaveModels.Count; i++)
			{
				Renderer renderer = localSaveModels[i].gameObject.GetComponentInChildren<MeshRenderer>();

				if (renderer == null) 
					continue;
				
				Material material = renderer.material;

				if (material == null) 
					continue;
				
				string texturePath = $"/Asset{i + 1}" + Constants.TextureFile;
				string fullPath = localSaveDirectoryPath + texturePath;
			
				material.mainTexture = LoadTexture(fullPath);
			}
			
			AddCollidersToAssets(localSaveModels);
		}

		private void AssignTextures(string localSaveDirectoryPath)
		{
			
		}

        private void AddCollidersToAssets(List<Transform> assets)
		{
			foreach (Transform asset in assets)
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
		
		public Texture LoadTexture(string path)
		{
			byte[] loadedBytes = File.ReadAllBytes(path);

			Texture2D textureFromBytes = new Texture2D(2, 2);
			textureFromBytes.LoadImage(loadedBytes);

			return textureFromBytes;
		}
	}
}