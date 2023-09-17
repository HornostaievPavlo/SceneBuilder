using GLTFast.Export;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    [SerializeField]
    private SavesRowsCoordinator rowsCoordinator;

    [SerializeField]
    private ScreenshotMaker screenshotMaker;

    public string scenePath;

    public void SaveNewScene()
    {
        scenePath = CreateNewSceneDirectory();

        var saveTargets = SaveLoadUtility.CollectSelectableObjects();

        SaveTextures(saveTargets);
        SaveModels(saveTargets);

        rowsCoordinator.CreateRowForNewSaveFile();
    }

    public string CreateNewSceneDirectory()
    {
        int number = SavesRowsCoordinator.scenesCounter;
        number++;

        screenshotMaker.MakePreviewScreenshot(number);

        return SaveLoadUtility.scenePath + number;
    }

    /// <summary>
    /// Saves all Selectables to file
    /// </summary>
    /// <param name="targets">Array of objects to save</param>
    private async void SaveModels(SelectableObject[] targets)
    {
        GameObject[] models = new GameObject[targets.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            models[i] = targets[i].gameObject;
        }

        var export = new GameObjectExport();
        export.AddScene(models);

        string filePath = scenePath + SaveLoadUtility.sceneFile;
        await export.SaveToFileAndDispose(filePath);
    }

    /// <summary>
    /// Saves textures from all Selectables
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
                Texture2D texture = SaveLoadUtility.DuplicateTexture((Texture2D)material.mainTexture);

                string directoryPath = scenePath + @$"\Asset{i + 1}";

                string filePath = directoryPath + SaveLoadUtility.textureFile;

                SaveLoadUtility.CreateDirectoryAndSaveTexture(texture, directoryPath, filePath);
            }
        }
    }
}
