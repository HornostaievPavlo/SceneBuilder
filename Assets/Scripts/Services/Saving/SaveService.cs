using System.Threading.Tasks;
using Gameplay;
using GLTFast.Export;
using UnityEngine;

namespace Services.Saving
{
	public class SaveService : ISaveService
	{
		private string scenePath;

        /// <summary>
        ///     Saves all scene assets,
        ///     creates save file row in UI
        /// </summary>
        public async Task SaveCurrentScene()
		{
			scenePath = CreateNewSaveDirectory();
			var saveTargets = IOUtility.CollectSelectableObjects();

			SaveTextures(saveTargets);
			await SaveModels(saveTargets);

			// rowsCoordinator.CreateRowForNewSaveFile();
		}

        /// <summary>
        ///     Creates directory for save file,
        ///     adds preview screenshot
        /// </summary>
        /// <returns>Path to created directory</returns>
        private string CreateNewSaveDirectory()
		{
			int number = SavePanelsCoordinator.panelsCounter;
			number++;

			// screenshotMaker.MakePreviewScreenshot(number);

			return IOUtility.scenePath + number;
		}

        /// <summary>
        ///     Saves all Models to file
        /// </summary>
        /// <param name="targets">Array of objects to save</param>
        private async Task<bool> SaveModels(SceneObject[] targets)
		{
			GameObject[] models = new GameObject[targets.Length];

			for (int i = 0; i < targets.Length; i++)
			{
				models[i] = targets[i].gameObject;
			}

			var export = new GameObjectExport();
			export.AddScene(models);

			string filePath = scenePath + Constants.SceneFile;
			bool success = await export.SaveToFileAndDispose(filePath);

			return success;
		}

        /// <summary>
        ///     Saves textures of all Models in the scene
        /// </summary>
        /// <param name="targets">Array of objects with textures</param>
        private void SaveTextures(SceneObject[] targets)
		{
			for (int i = 0; i < targets.Length; i++)
			{
				Renderer renderer = targets[i].GetComponentInChildren<Renderer>();
				Material material = renderer.sharedMaterial;

				if (material.mainTexture != null)
				{
					Texture2D texture = IOUtility.DuplicateTexture((Texture2D) material.mainTexture);

					string directoryPath = scenePath + $"/Asset{i + 1}";
					string filePath = directoryPath + Constants.TextureFile;

					IOUtility.CreateDirectoryAndSaveTexture(texture, directoryPath, filePath);
				}
			}
		}
	}
}