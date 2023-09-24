using GLTFast;
using GLTFast.Export;
using GLTFast.Logging;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    [SerializeField]
    private SavePanelsCoordinator rowsCoordinator;

    [SerializeField]
    private ScreenshotMaker screenshotMaker;

    [SerializeField]
    private GameObject savePopUp;

    private string scenePath;


    /////

    public TMP_Text text;

    public GameObject[] save;

    public async void TryLoadFromPDP()
    {
        string saveFilePath = Application.persistentDataPath + "/Model.glb";

        var gltf = new GltfImport();

        bool success = await gltf.Load(saveFilePath);

        if (success)
        {
            text.text += "/// Load successful";

            success = await gltf.InstantiateMainSceneAsync(IOUtility.assetsParent);
        }
    }

    public async void TrySaveToPDP()
    {
        text.text = Application.persistentDataPath;

        string saveFilePath = Application.persistentDataPath + "/Model.glb";

        using (FileStream fileStream = new FileStream(saveFilePath, FileMode.CreateNew))
        {
            var logger = new CollectingLogger();
            var exportSettings = new ExportSettings
            {
                Format = GltfFormat.Binary,
                FileConflictResolution = FileConflictResolution.Overwrite,
                ComponentMask = ~(ComponentType.Camera | ComponentType.Animation),
                LightIntensityFactor = 100f,
            };

            var gameObjectExportSettings = new GameObjectExportSettings
            {
                OnlyActiveInHierarchy = false,
                DisabledComponents = true,
                LayerMask = LayerMask.GetMask("Default", "MyCustomLayer"),
            };

            var export = new GameObjectExport(exportSettings, gameObjectExportSettings, logger: logger);
            export.AddScene(save);

            bool success = await export.SaveToStreamAndDispose(fileStream);

            if (success) text.text += " //Save successful";
        }
    }

    public async void SaveNewScene()
    {
        savePopUp.SetActive(true);

        scenePath = CreateNewSceneDirectory();

        var saveTargets = IOUtility.CollectSelectableObjects();

        SaveTextures(saveTargets);
        await SaveModels(saveTargets);

        savePopUp.SetActive(false);

        rowsCoordinator.CreateRowForNewSaveFile();
    }

    public string CreateNewSceneDirectory()
    {
        int number = SavePanelsCoordinator.panelsCounter;
        number++;

        screenshotMaker.MakePreviewScreenshot(number);

        return IOUtility.scenePath + number;
    }

    /// <summary>
    /// Saves all Selectables to file
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
                Texture2D texture = IOUtility.DuplicateTexture((Texture2D)material.mainTexture);

                string directoryPath = scenePath + $"/Asset{i + 1}";

                string filePath = directoryPath + IOUtility.textureFile;

                IOUtility.CreateDirectoryAndSaveTexture(texture, directoryPath, filePath);
            }
        }
    }
}
