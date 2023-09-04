using GLTFast;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class LoadingSystem : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    List<Transform> currentAssetsInScene = new List<Transform>();

    //how to do input field reading without direct reference?

    public async void LoadAssetFromDirectory() => await LoadModels(inputField.text);

    public async void LoadAssetsFromSaveFile()
    {
        bool success = await LoadModels(SaveLoadUtility.scenePath);

        if (success) LoadTexture();
    }

    private async Task<bool> LoadModels(string modelPath)
    {
        var asset = CreateAsset(AssetType.Model);

        var success = await asset.Load(modelPath);

        if (success)
        {
            var assets = new List<Transform>();

            if (modelPath == SaveLoadUtility.scenePath) assets = InitializeImportedAssets();

            AddCollidersToAssets(assets);
        }

        return success;
    }

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

    private List<Transform> InitializeImportedAssets() // gabella
    {
        // setting scene obj as child of placeholder
        Transform sceneObj = GameObject.Find("Scene").transform;
        sceneObj.SetParent(SaveLoadUtility.assetsParent);

        // removing spawner
        var spawner = SaveLoadUtility.assetsParent.gameObject.GetComponentInChildren<GltfAsset>();
        Destroy(spawner.gameObject.GetComponent<SelectableObject>());
        spawner.gameObject.name = "SPAWNER";

        // setting assets up from a scene
        Transform[] children = sceneObj.gameObject.GetComponentsInChildren<Transform>();

        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].gameObject.name == "Asset") currentAssetsInScene.Add(children[i]);
        }

        foreach (var asset in currentAssetsInScene)
        {
            asset.SetParent(SaveLoadUtility.assetsParent);
            var selectable = asset.gameObject.AddComponent<SelectableObject>();
            selectable.type = AssetType.Model;
        }

        Destroy(sceneObj.gameObject);

        return currentAssetsInScene;
    }

    private void LoadTexture()
    {
        for (int i = 0; i < currentAssetsInScene.Count; i++)
        {
            Renderer renderer = currentAssetsInScene[i].gameObject.GetComponentInChildren<MeshRenderer>();

            if (renderer != null)
            {
                Material material = renderer.material;

                if (material != null)
                {
                    string pathToTexture =
                        SaveLoadUtility.assetSavePath + @$"\Asset {i + 1}" + @"\Texture.png";

                    byte[] loadedBytes = File.ReadAllBytes(pathToTexture);

                    Texture2D textureFromBytes = new Texture2D(2, 2);
                    textureFromBytes.LoadImage(loadedBytes);

                    material.mainTexture = textureFromBytes;
                }
            }
        }
    }

    private void AddCollidersToAssets(List<Transform> assets)
    {
        foreach (var asset in assets)
        {
            MeshRenderer[] meshRenderers = asset.gameObject.GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer renderer in meshRenderers)
            {
                var itemCollider = renderer.gameObject.AddComponent<MeshCollider>();
                itemCollider.convex = true;
            }
        }
    }
}
