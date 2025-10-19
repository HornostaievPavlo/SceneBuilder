using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Enums;
using Gameplay;
using GLTFast.Export;
using Plain;
using Services.LocalSavesRepository;
using Services.SceneObjectsRegistry;
using UnityEngine;

namespace Services.Saving
{
	public class SaveService : ISaveService
	{
		private readonly ISceneObjectsRegistry _sceneObjectsRegistry;
		private readonly ILocalSavesRepository _localSavesRepository;

		public SaveService(ISceneObjectsRegistry sceneObjectsRegistry, ILocalSavesRepository localSavesRepository)
		{
			_sceneObjectsRegistry = sceneObjectsRegistry;
			_localSavesRepository = localSavesRepository;
		}

		public async Task CreateLocalSave(Texture2D preview)
		{
			List<SceneObject> saveTargets = _sceneObjectsRegistry.GetSceneObjects(SceneObjectTypeId.Model);
			
			if (saveTargets.Count == 0)
			{
				Debug.Log($"Trying to save empty scene");
				return;
			}
			
			string directoryPath = CreateLocalSaveDirectory();

			await SaveModels(directoryPath, saveTargets);
			SavePreview(directoryPath, preview);
			
			_localSavesRepository.AddLocalSave(new LocalSave(directoryPath, preview));
		}
		
		public void DeleteLocalSave(LocalSave localSave)
		{
			if (Directory.Exists(localSave.DirectoryPath) == false)
				return;
			
			Directory.Delete(localSave.DirectoryPath, true);
			RearrangeDirectories();
			
			_localSavesRepository.RemoveLocalSave(localSave);
		}

		private string CreateLocalSaveDirectory()
		{
			int sceneNumber = _localSavesRepository.GetLocalSaves().Count + 1;
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
		
		private void RearrangeDirectories()
		{
			string[] directories = Directory.GetDirectories(Constants.ApplicationDataPath);
			
			for (int i = 0; i < directories.Length; i++)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(directories[i]);

				string currentName = Constants.ApplicationDataPath + @"/" + directoryInfo.Name;
				string targetName = Constants.ScenePath + (i + 1);

				if (currentName != targetName)
				{
					Directory.Move(currentName, targetName);
				}
			}
		}
	}
}