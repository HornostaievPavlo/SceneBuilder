using GLTFast.Export;
using System.Threading.Tasks;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    [SerializeField]
    private SavePanelsCoordinator rowsCoordinator;

    [SerializeField]
    private ScreenshotMaker screenshotMaker;

    private string scenePath;

    /// <summary>
    /// Saves all scene assets, 
    /// creates save file row in UI
    /// </summary>
    public async Task SaveCurrentScene()
    {
        scenePath = CreateNewSaveDirectory();
        var saveTargets = IOUtility.CollectSelectableObjects();

        SaveTextures(saveTargets);
        await SaveModels(saveTargets);

        rowsCoordinator.CreateRowForNewSaveFile();
    }

    /// <summary>
    /// Creates directory for save file,
    /// adds preview screenshot
    /// </summary>
    /// <returns>Path to created directory</returns>
    private string CreateNewSaveDirectory()
    {
        int number = SavePanelsCoordinator.panelsCounter;
        number++;

        screenshotMaker.MakePreviewScreenshot(number);

        return IOUtility.scenePath + number;
    }

    /// <summary>
    /// Saves all Models to file
    /// </summary>
    /// <param name="targets">Array of objects to save</param>
    private async Task<bool> SaveModels(SelectableObject[] targets)
    {
        GameObject[] models = new GameObject[targets.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            models[i] = targets[i].gameObject;
        }

        var export = new GameObjectExport();
        export.AddScene(models);

        string filePath = scenePath + IOUtility.sceneFile;
        bool success = await export.SaveToFileAndDispose(filePath);

        return success;
    }

    /// <summary>
    /// Saves textures of all Models in the scene
    /// </summary>
    /// <param name="targets">Array of objects with textures</param>
    private void SaveTextures(SelectableObject[] targets)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            Renderer renderer = targets[i].GetComponentInChildren<Renderer>();
            Material material = renderer.sharedMaterial;

            if (material.mainTexture != null)
            {
                Texture2D texture = IOUtility.DuplicateTexture((Texture2D)material.mainTexture);

                string directoryPath = scenePath + $"/Asset{i + 1}";
                string filePath = directoryPath + IOUtility.textureFile;

                IOUtility.CreateDirectoryAndSaveTexture(texture, directoryPath, filePath);
            }
        }
    }
}
