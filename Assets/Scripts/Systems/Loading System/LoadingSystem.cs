using GLTFast;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class LoadingSystem : MonoBehaviour//, ILoadable
{
    private GltfAsset CreateAsset()
    {
        GameObject asset = new GameObject
        {
            name = "Asset"
        };

        asset.transform.SetParent(SaveLoadUtility.placeholder);

        var gltfAsset = asset.AddComponent<GltfAsset>();

        return gltfAsset;
    }

    public async void LoadAsset()
    {
        bool success = await LoadModel();

        if (success)
        {
            Debug.Log("Model loaded successfully");

            LoadTexture();
        }
    }
    #region utka
    public async void Duck()
    {
        await LoadDuck();
    }
    public async Task<bool> LoadDuck()
    {
        var asset = CreateAsset();

        var success = await asset.Load(SaveLoadUtility.duckModelPath);
        if (success) asset.gameObject.AddComponent<SelectableObject>();

        return success;
    }
    #endregion

    private async Task<bool> LoadModel()
    {
        var asset = CreateAsset();

        var success = await asset.Load(SaveLoadUtility.modelPath);
        if (success) asset.gameObject.AddComponent<SelectableObject>();

        return success;
    }

    private void LoadTexture()
    {
        var model = SaveLoadUtility.placeholder.gameObject.GetComponentInChildren<SelectableObject>();

        Renderer renderer = model.gameObject.GetComponentInChildren<MeshRenderer>();

        if (renderer != null)
        {
            Material material = renderer.material;

            if (material != null)
            {
                byte[] loadedBytes = File.ReadAllBytes(SaveLoadUtility.texturePath);

                Texture2D textureFromBytes = new Texture2D(2, 2);
                textureFromBytes.LoadImage(loadedBytes);

                material.mainTexture = textureFromBytes;

                Debug.Log("Texture added successfully");
            }
        }
    }
}
