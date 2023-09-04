using GLTFast.Export;
using System.IO;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    private SelectableObject[] CollectSelectableObjects()
    {
        SelectableObject[] selectableObjects =
            SaveLoadUtility.assetsParent.GetComponentsInChildren<SelectableObject>();

        foreach (var item in selectableObjects)
        {
            SaveLoadUtility.savingTargets.Add(item.gameObject);
        }

        return selectableObjects;
    }

    public void SaveAssets()
    {
        var saveTargets = CollectSelectableObjects();

        SaveTextures(saveTargets);

        SaveModels();
    }

    private async void SaveModels()
    {
        var export = new GameObjectExport();
        export.AddScene(SaveLoadUtility.savingTargets.ToArray());

        bool success = await export.SaveToFileAndDispose(SaveLoadUtility.scenePath);

        if (success) Debug.Log("Models saved successfully");
    }

    private void SaveTextures(SelectableObject[] targets)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            Renderer renderer = targets[i].GetComponentInChildren<Renderer>();

            Material material = renderer.sharedMaterial;

            if (material.mainTexture != null)
            {
                Texture2D texture = DuplicateTexture((Texture2D)material.mainTexture);

                string directoryPath = SaveLoadUtility.assetSavePath + @$"\Asset {i + 1}";

                string filePath = directoryPath + @"\Texture.png";

                CreateDirectoryAndSaveTexture(texture, directoryPath, filePath);

                Debug.Log($"Texture number ({i + 1}) saved successfully");
            }
        }
    }

    private void CreateDirectoryAndSaveTexture(Texture2D texture, string directory, string file)
    {
        byte[] textureBytes = texture.EncodeToPNG();

        var folder = Directory.CreateDirectory(directory);

        //Debug.Log("Creating folder with name - " + folderName);

        var fullPath = Path.Combine(folder.FullName, file);

        //Debug.Log("Final path - " + fullPath);

        File.WriteAllBytes(fullPath, textureBytes);
    }

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
