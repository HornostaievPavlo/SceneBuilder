using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Enums;
using Gameplay;
using GLTFast.Export;
using Services.LocalSaves;
using Services.SceneObjectsRegistry;
using UnityEngine;
using Zenject;

namespace Services.Saving
{
	public class SaveService : ISaveService
	{
		private ReadableTextureCopyInstantiator _textureCopyInstantiator;
		
		private ISceneObjectsRegistry _sceneObjectsRegistry;
		private ILocalSavesService _localSavesService;

		[Inject]
		private void Construct(ISceneObjectsRegistry sceneObjectsRegistry, ILocalSavesService localSavesService)
		{
			_sceneObjectsRegistry = sceneObjectsRegistry;
			_localSavesService = localSavesService;
			
			_textureCopyInstantiator = new ReadableTextureCopyInstantiator();
		}

		public async Task SaveScene(Texture2D preview)
		{
			string directoryPath = CreateLocalSaveDirectory();
			List<SceneObject> saveTargets = _sceneObjectsRegistry.GetSceneObjects(SceneObjectTypeId.Model);

			await SaveModels(directoryPath, saveTargets);
			SaveTextures(directoryPath, saveTargets);
			SavePreview(directoryPath, preview);
		}

		private string CreateLocalSaveDirectory()
		{
			int sceneNumber = _localSavesService.GetLocalSaves().Count + 1;
			string directoryPath = Constants.ScenePath + sceneNumber;
			
			Directory.CreateDirectory(directoryPath);
			return directoryPath;
		}

		private async Task SaveModels(string path, List<SceneObject> modelsList)
		{
			GameObject[] modelsArray = new GameObject[modelsList.Count];

			for (int i = 0; i < modelsList.Count; i++)
			{
				modelsArray[i] = modelsList[i].gameObject;
			}

			var export = new GameObjectExport();
			export.AddScene(modelsArray);

			string filePath = path + Constants.AssetFile;
			await export.SaveToFileAndDispose(filePath);
		}

		private void SaveTextures(string path, List<SceneObject> modelsList)
		{
			for (int i = 0; i < modelsList.Count; i++)
			{
				Renderer renderer = modelsList[i].GetComponentInChildren<Renderer>();
				Material material = renderer.sharedMaterial;

				if (material.mainTexture == null) 
					continue;
				
				Texture2D texture = _textureCopyInstantiator.CreateReadableTexture(material.mainTexture);

				string directoryPath = path + $"/Asset{i + 1}";
				string filePath = directoryPath + Constants.TextureFile;

				SaveTextureToDirectory(texture, directoryPath, filePath);
			}
		}

		private void SavePreview(string path, Texture2D preview)
		{
			string screenshotPath = path + Constants.PreviewFile;
			SaveTextureToDirectory(preview, path, screenshotPath);
		}

		private void SaveTextureToDirectory(Texture2D texture, string directoryPath, string file)
		{
			byte[] textureBytes = texture.EncodeToPNG();

			DirectoryInfo directoryInfo = Directory.CreateDirectory(directoryPath);
			string fullPath = Path.Combine(directoryInfo.FullName, file);

			File.WriteAllBytes(fullPath, textureBytes);
		}
	}
}