using GLTFast;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class LoadingSystem : MonoBehaviour//, ILoadable
{
    [SerializeField] private TMP_InputField inputField;

    private GltfAsset CreateAsset(AssetType type)
    {
        GameObject asset = new GameObject
        {
            name = "Asset"
        };

        asset.transform.SetParent(SaveLoadUtility.assetsParent);

        SelectableObject selectable = asset.AddComponent<SelectableObject>();
        selectable.type = type;

        var gltfAsset = asset.AddComponent<GltfAsset>();

        return gltfAsset;
    }

    //how to do input field reading without direct reference?
    public async void LoadAssetFromDirectory()
    {
        if(inputField.text != string.Empty)
        {
            bool success = await LoadModel(inputField.text);

            if (success) Debug.Log("Model loaded from INPUT FIELD successfully");
        }       
    }

    public async void LoadAssetFromSaveFile()
    {
        bool success = await LoadModel(SaveLoadUtility.modelPath);

        if (success)
        {
            Debug.Log("Model loaded from SAVE FILE successfully");

            LoadTexture();
        }
    }

    /// <summary>
    /// Base Task for loading model from any source path
    /// </summary>
    /// <param name="modelPath">Directory or save file</param>
    /// <returns>Result of loading process</returns>
    private async Task<bool> LoadModel(string modelPath)
    {
        var asset = CreateAsset(AssetType.Model);

        var success = await asset.Load(modelPath);

        if (success)
        {
            //var selectable = asset.gameObject.AddComponent<SelectableObject>();
            //selectable.type = ObjectType.Model;

            AddCollidersToAsset(asset);
        }

        return success;
    }

    private void LoadTexture()
    {
        var model = SaveLoadUtility.assetsParent.gameObject.GetComponentInChildren<SelectableObject>();

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

    private void AddCollidersToAsset(GltfAsset asset)
    {
        MeshRenderer[] meshRenderers = asset.gameObject.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer item in meshRenderers)
        {
            var itemCollider = item.gameObject.AddComponent<MeshCollider>();

            itemCollider.convex = true;
        }
    }
}
