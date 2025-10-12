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
		
		private ReadableTextureCopyInstantiator _textureCopyInstantiator;

		private IInstantiateService _instantiateService;
		private ISceneObjectsRegistry _sceneObjectsRegistry;

		[Inject]
		private void Construct(IInstantiateService instantiateService, ISceneObjectsRegistry sceneObjectsRegistry)
		{
			_instantiateService = instantiateService;
			_sceneObjectsRegistry = sceneObjectsRegistry;
			
			_textureCopyInstantiator = new ReadableTextureCopyInstantiator();
		}

		public async Task<bool> LoadModel(string modelPath, string localSaveDirectoryPath = "")
		{
			if (string.IsNullOrEmpty(modelPath))
			{
				// Debug.LogError($"Trying to load model from empty path");
				// return false;
				
				modelPath = Constants.DuckModelPath;
			}

			SceneObject sceneObject = InstantiateSceneObject(SceneObjectTypeId.Model);
			
			var gltfAsset = sceneObject.gameObject.AddComponent<GltfAsset>();
			var instantiationSettings = new InstantiationSettings()
			{
				SceneObjectCreation = SceneObjectCreation.Never
			};

			gltfAsset.InstantiationSettings = instantiationSettings;

			bool isSuccess = await gltfAsset.Load(modelPath);

			if (isSuccess == false)
				return false;

			bool isLoadedFromLocalSave = modelPath.Contains(Constants.ApplicationDataPath);

			if (isLoadedFromLocalSave)
			{
				SetupLocalSaveAssets(localSaveDirectoryPath);
			}
			else
			{
				AddColliders(sceneObject);
			}
			
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

		public async void LoadLocalSave(LocalSave localSave)
		{
			string assetPath = localSave.DirectoryPath + Constants.AssetFile;
			await LoadModel(assetPath, localSave.DirectoryPath);
		}

		public Texture LoadTexture(string path)
		{
			byte[] loadedBytes = File.ReadAllBytes(path);

			Texture2D textureFromBytes = new Texture2D(2, 2);
			textureFromBytes.LoadImage(loadedBytes);

			return textureFromBytes;
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

		// private void SetupLocalSaveAssets(string localSaveDirectoryPath)
		// {
		// 	List<Transform> resultList = new();
		// 	List<Transform> localSaveModels = new();
		// 	List<Transform> children = new();
		//
		// 	bool isSingleAsset = GameObject.Find("Scene") == null;
		// 	Transform sceneObj = null;
		//
		// 	if (!isSingleAsset)
		// 	{
		// 		sceneObj = GameObject.Find("Scene").transform;
		// 		sceneObj.SetParent(_sceneObjectsRegistry.SceneObjectsHolder);
		// 	}
		//
		// 	var spawner = _sceneObjectsRegistry.SceneObjectsHolder.gameObject.GetComponentInChildren<GltfAsset>();
		// 	Object.Destroy(spawner.gameObject.GetComponent<SceneObject>());
		// 	spawner.gameObject.name = "glTF Asset";
		//
		// 	GltfAsset[] spawners = _sceneObjectsRegistry.SceneObjectsHolder.gameObject.GetComponentsInChildren<GltfAsset>();
		// 	foreach (GltfAsset item in spawners)
		// 	{
		// 		Object.Destroy(item.gameObject.GetComponent<SceneObject>());
		// 		item.gameObject.name = "glTF Asset";
		// 	}
		//
		// 	children = _sceneObjectsRegistry.SceneObjectsHolder.gameObject.GetComponentsInChildren<Transform>().ToList();
		//
		// 	for (int i = 0; i < children.Count; i++)
		// 	{
		// 		if (children[i].gameObject.name == "Asset") resultList.Add(children[i]);
		// 	}
		//
		// 	foreach (var asset in resultList)
		// 	{
		// 		asset.SetParent(_sceneObjectsRegistry.SceneObjectsHolder);
		//
		// 		bool hasSelectable = asset.GetComponentInChildren<SceneObject>() != null;
		//
		// 		if (hasSelectable == false)
		// 		{
		// 			SceneObject sceneObject = asset.gameObject.AddComponent<SceneObject>();
		// 			sceneObject.Register(SceneObjectTypeId.Model);
		//
		// 			localSaveModels.Add(asset.transform);
		// 		}
		// 	}
		//
		// 	if (sceneObj)
		// 	{
		// 		Object.Destroy(sceneObj.gameObject);
		// 	}
		// 	
		// 	// AssignTextures(localSaveModels, localSaveDirectoryPath);
		// 	localSaveModels.Clear();
		// }

		private void SetupLocalSaveAssets(string localSaveDirectoryPath)
		{
			Transform sceneObjectsHolder = _sceneObjectsRegistry.SceneObjectsHolder;
			Transform[] childTransforms = sceneObjectsHolder.GetComponentsInChildren<Transform>(true);

			var exportParent = sceneObjectsHolder.GetComponentInChildren<SceneObject>();
			exportParent.gameObject.name = "ExportParent";
			
			List<Transform> modelHolders = new();

			foreach (Transform childTransform in childTransforms)
			{
				if (childTransform.gameObject.name != Constants.ModelHolderObjectName || childTransform == exportParent.transform) 
					continue;
					
				modelHolders.Add(childTransform);
			}
			
			List<Mesh> modelsMeshes = new();
			List<Material> modelsMaterials = new();
			List<Texture> modelsTextures = new();

			foreach (Transform modelHolder in modelHolders)
			{
				modelHolder.SetParent(sceneObjectsHolder);
				
				var modelMeshFilter = modelHolder.GetComponentInChildren<MeshFilter>();
				var modelMeshRenderer = modelHolder.GetComponentInChildren<MeshRenderer>();
				
				var meshCopy = new Mesh();
				meshCopy.Clear();
				meshCopy = modelMeshFilter.mesh;
				meshCopy.name = "mesh";
				
				modelsMeshes.Add(meshCopy);
				
				var materialCopy = new Material(modelMeshRenderer.material);
				modelsMaterials.Add(materialCopy);

				var textureCopy = _textureCopyInstantiator.CreateReadableTexture(modelMeshRenderer.material.mainTexture);
				modelsTextures.Add(textureCopy);
			}

			_sceneObjectsRegistry.DeleteObject(exportParent);

			for (var i = 0; i < modelHolders.Count; i++)
			{
				Transform modelHolder = modelHolders[i];
				
				var modelMeshFilter = modelHolder.GetComponentInChildren<MeshFilter>();
				var modelMeshRenderer = modelHolder.GetComponentInChildren<MeshRenderer>();
				
				modelMeshFilter.sharedMesh = modelsMeshes[i];
				modelMeshRenderer.sharedMaterial = modelsMaterials[i];
				modelMeshRenderer.sharedMaterial.mainTexture = modelsTextures[i];
				
				SceneObject sceneObject =_instantiateService.AddSceneObjectComponent(modelHolder.gameObject, SceneObjectTypeId.Model);
				AddColliders(sceneObject);
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