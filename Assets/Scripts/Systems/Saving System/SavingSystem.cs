using GLTFast.Export;
using System.IO;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    [SerializeField]
    private SavesRowsCoordinator rowsCoordinator;

    public string scenePath;

    public void SaveNewScene()
    {
        scenePath = CreateNewSceneDirectory();

        var saveTargets = SaveLoadUtility.CollectSelectableObjects();

        SaveTextures(saveTargets);
        SaveModels(saveTargets);

        rowsCoordinator.CreateRowForNewSaveFile();
    }

    public static string CreateNewSceneDirectory()
    {
        int number = SavesRowsCoordinator.scenesCounter;
        number++;

        return SaveLoadUtility.scenesPath + @"\Scene" + $"{number}";
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

        string filePath = scenePath + @"\Asset.gltf";
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

                string directoryPath = scenePath + @$"\Asset {i + 1}";

                string filePath = directoryPath + @"\Texture.png";

                CreateDirectoryAndSaveTexture(texture, directoryPath, filePath);
            }
        }
    }

    /// <summary>
    /// Creates directory and saves given texture
    /// </summary>
    /// <param name="texture">File to save</param>
    /// <param name="directory">Path to directory</param>
    /// <param name="file">Path to file</param>
    private void CreateDirectoryAndSaveTexture(Texture2D texture, string directory, string file)
    {
        byte[] textureBytes = texture.EncodeToPNG();

        var folder = Directory.CreateDirectory(directory);
        var fullPath = Path.Combine(folder.FullName, file);

        File.WriteAllBytes(fullPath, textureBytes);
    }
}
