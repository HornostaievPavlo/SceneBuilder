using GLTFast.Export;
using System.IO;
using UnityEngine;

public class SavingSystem : MonoBehaviour//, ISavable
{
    private void CollectSelectableObjects()
    {
        SelectableObject[] selectableObjects =
            SaveLoadUtility.placeholder.GetComponentsInChildren<SelectableObject>();

        foreach (var item in selectableObjects)
        {
            SaveLoadUtility.savingTargets.Add(item.gameObject);
        }
    }

    public void SaveAsset()
    {
        CollectSelectableObjects();

        SaveTexture();

        SaveModel();
    }

    private async void SaveModel()
    {
        var export = new GameObjectExport();
        export.AddScene(SaveLoadUtility.savingTargets.ToArray());

        bool success = await export.SaveToFileAndDispose(SaveLoadUtility.modelPath);

        if (success) Debug.Log("Model saved successfully");
    }

    private void SaveTexture()
    {
        Renderer renderer = SaveLoadUtility.savingTargets[0].GetComponentInChildren<Renderer>();

        if (renderer != null)
        {
            Material material = renderer.sharedMaterial;

            if (material != null && material.mainTexture != null)
            {
                Texture2D texture = (Texture2D)material.mainTexture;

                Texture2D readableCopy = DuplicateTexture(texture);

                SaveTextureToFile(readableCopy);

                Debug.Log("Texture saved successfully");
            }
        }

        //foreach (var target in savingTargets)
        //{
        //    Destroy(target);
        //}
    }

    private void SaveTextureToFile(Texture2D texture)
    {
        byte[] textureBytes = texture.EncodeToPNG();
        File.WriteAllBytes(SaveLoadUtility.texturePath, textureBytes);
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
