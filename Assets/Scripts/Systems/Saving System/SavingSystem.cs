using GLTFast.Export;
using System.IO;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    public void SaveAssets()
    {
        var saveTargets = CollectSelectableObjects();

        SaveTextures(saveTargets);

        SaveModels(saveTargets);
    }

    /// <summary>
    /// Finds all Selectables in scene
    /// </summary>
    /// <returns>Array of Selectables</returns>
    private SelectableObject[] CollectSelectableObjects()
    {
        return SaveLoadUtility.assetsParent.GetComponentsInChildren<SelectableObject>();
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
        await export.SaveToFileAndDispose(SaveLoadUtility.scenePath);
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
                Texture2D texture = DuplicateTexture((Texture2D)material.mainTexture);

                string directoryPath = SaveLoadUtility.assetsSavePath + @$"\Asset {i + 1}";

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

    /// <summary>
    /// Makes readable copy of texture
    /// </summary>
    /// <param name="source">Original texture</param>
    /// <returns>Readable copy</returns>
    private Texture2D DuplicateTexture(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
}
