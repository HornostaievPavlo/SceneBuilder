using GLTFast;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class LoadingSystem : MonoBehaviour
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

    private void Start() // CUSTOM!!!!!!!!!!!!!!
    {
        LoadAssetFromDirectory();
    }
    public async void LoadAssetFromDirectory()
    {
        inputField.text = "https://raw.githubusercontent.com/KhronosGroup/glTF-Sample-Models/master/2.0/Duck/glTF/Duck.gltf";
        if (inputField.text != string.Empty)
        {
            bool success = await LoadModel(inputField.text);

            //if (success) Debug.Log("Model loaded from INPUT FIELD successfully");
        }
    }

    public async void LoadAssetFromSaveFile()
    {
        bool success = await LoadModel(SaveLoadUtility.modelPath);

        if (success)
        {
            //Debug.Log("Model loaded from SAVE FILE successfully");

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
