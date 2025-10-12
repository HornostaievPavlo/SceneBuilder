using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Enums;
using Gameplay;
using GLTFast;
using Plain;
using Services.Instantiation;
using Services.SceneObjectsRegistry;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Services.Loading
{
	public class LoadService : ILoadService
	{
		private ReadableTextureCopyInstantiator _textureCopyInstantiator;

		private IInstantiateService _instantiateService;
		private ISceneObjectsRegistry _sceneObjectsRegistry;

		public event Action OnLocalSaveLoaded;

		[Inject]
		private void Construct(IInstantiateService instantiateService, ISceneObjectsRegistry sceneObjectsRegistry)
		{
			_instantiateService = instantiateService;
			_sceneObjectsRegistry = sceneObjectsRegistry;

			_textureCopyInstantiator = new ReadableTextureCopyInstantiator();
		}

		public async Task<bool> LoadModel(string modelPath)
		{
			if (string.IsNullOrEmpty(modelPath))
			{
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
				SetupLocalSaveAssets();
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
			bool isLoadSuccessful = await LoadModel(assetPath);
			
			if (isLoadSuccessful)
			{
				OnLocalSaveLoaded?.Invoke();
			}
		}

		public Texture LoadTexture(string texturePath)
		{
			byte[] textureBytes = File.ReadAllBytes(texturePath);

			Texture2D loadedTexture = new Texture2D(2, 2);
			loadedTexture.LoadImage(textureBytes);

			return loadedTexture;
		}

		private SceneObject InstantiateSceneObject(SceneObjectTypeId typeId)
		{
			GameObject prefab = typeId switch
			{
				SceneObjectTypeId.Model => AssetDatabase.LoadAssetAtPath<GameObject>(Constants.ModelPrefabPath),
				SceneObjectTypeId.Camera => AssetDatabase.LoadAssetAtPath<GameObject>(Constants.CameraPrefabPath),
				SceneObjectTypeId.Label => AssetDatabase.LoadAssetAtPath<GameObject>(Constants.LabelPrefabPath),
				_ => null
			};

			return _instantiateService.InstantiateSceneObject(prefab, _sceneObjectsRegistry.SceneObjectsHolder, typeId);
		}

		private void SetupLocalSaveAssets()
		{
			Transform sceneObjectsHolder = _sceneObjectsRegistry.SceneObjectsHolder;
			SceneObject exportParent = PrepareExportParent(sceneObjectsHolder);
			
			List<Transform> modelHolders = FindModelHolders(sceneObjectsHolder, exportParent.transform);
			List<ModelAssets> extractedAssets = ExtractAssetCopiesFromModels(modelHolders);

			_sceneObjectsRegistry.DeleteObject(exportParent);

			for (int i = 0; i < modelHolders.Count; i++)
			{
				SceneObject sceneObject = ApplyExtractedAssetsToModels(modelHolders[i], extractedAssets[i]);
				AddColliders(sceneObject);
			}
		}

		private SceneObject PrepareExportParent(Transform sceneObjectsHolder)
		{
			var exportParent = sceneObjectsHolder.GetComponentInChildren<SceneObject>();
			exportParent.gameObject.name = "ExportParent";
			return exportParent;
		}

		private List<Transform> FindModelHolders(Transform sceneObjectsHolder, Transform exportParentTransform)
		{
			Transform[] allChildTransforms = sceneObjectsHolder.GetComponentsInChildren<Transform>(true);
			List<Transform> modelHolders = new();

			foreach (Transform childTransform in allChildTransforms)
			{
				bool isModelHolder = childTransform.gameObject.name == Constants.ModelHolderObjectName;
				bool isNotExportParent = childTransform != exportParentTransform;
				
				if (isModelHolder && isNotExportParent)
				{
					modelHolders.Add(childTransform);
				}
			}

			return modelHolders;
		}

		private List<ModelAssets> ExtractAssetCopiesFromModels(List<Transform> modelHolders)
		{
			List<ModelAssets> extractedAssets = new();

			foreach (Transform modelHolder in modelHolders)
			{
				modelHolder.SetParent(_sceneObjectsRegistry.SceneObjectsHolder);
				
				var meshFilter = modelHolder.GetComponentInChildren<MeshFilter>();
				var meshRenderer = modelHolder.GetComponentInChildren<MeshRenderer>();
				
				var meshCopy = new Mesh();
				meshCopy.Clear();
				meshCopy = meshFilter.mesh;
				meshCopy.name = "mesh";
				
				var materialCopy = new Material(meshRenderer.material);

				Texture2D textureCopy = _textureCopyInstantiator.CreateReadableTexture(meshRenderer.material.mainTexture);

				var modelAssets = new ModelAssets(meshCopy, materialCopy, textureCopy);
				extractedAssets.Add(modelAssets);
			}

			return extractedAssets;
		}

		private SceneObject ApplyExtractedAssetsToModels(Transform modelHolder, ModelAssets assets)
		{
			var meshFilter = modelHolder.GetComponentInChildren<MeshFilter>();
			var meshRenderer = modelHolder.GetComponentInChildren<MeshRenderer>();
			
			meshFilter.sharedMesh = assets.Mesh;
			meshRenderer.sharedMaterial = assets.Material;
			meshRenderer.sharedMaterial.mainTexture = assets.Texture;
			
			SceneObject sceneObject = _instantiateService.AddSceneObjectComponent(modelHolder.gameObject, SceneObjectTypeId.Model);
			return sceneObject;
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