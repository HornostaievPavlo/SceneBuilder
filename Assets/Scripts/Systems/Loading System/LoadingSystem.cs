using GLTFast;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class LoadingSystem : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private GameObject cameraAssetPrefab;

    [SerializeField]
    private GameObject labelAssetPrefab;

    [SerializeField] private List<Transform> modelsInScene = new List<Transform>();

    public async void LoadAssetsFromDirectory() => await LoadModels(inputField.text);

    public async void LoadAssetsFromSaveFile()
    {
        bool success = await LoadModels(SaveLoadUtility.scenePath);

        if (success) AssignTextures();
    }

    public void LoadCameraAsset()
    {
        CreateAsset(AssetType.Camera);
    }

    public void LoadLabelAsset()
    {
        CreateAsset(AssetType.Label);
    }

    /// <summary>
    /// General model loading procedure 
    /// Handles adding of colliders to models
    /// </summary>
    /// <param name="modelPath">Local storage or save file</param>
    /// <returns>Success of loading</returns>
    private async Task<bool> LoadModels(string modelPath)
    {
        modelsInScene.Clear();

        var asset = CreateAsset(AssetType.Model).GetComponent<GltfAsset>();

        var success = await asset.Load(modelPath);

        if (success)
        {
            if (modelPath == SaveLoadUtility.scenePath)
            {
                var assets = InitializeImportedAssets();
                AddCollidersToAssets(assets);
            }
            else
            {
                Transform[] children = SaveLoadUtility.assetsParent.gameObject.GetComponentsInChildren<Transform>();

                for (int i = 0; i < children.Length; i++)
                {
                    if (children[i].gameObject.name == "Asset") modelsInScene.Add(children[i]);
                }

                AddCollidersToAssets(modelsInScene);

            }
        }
        return success;
    }

    /// <summary>
    /// Creates new Selecetable asset of specified type
    /// </summary>
    /// <param name="type">Specifies AssetType of created object</param>
    /// <returns>Asset instance</returns>
    private GameObject CreateAsset(AssetType type)
    {
        switch (type)
        {
            case AssetType.Model:

                GameObject asset = new GameObject
                {
                    name = "Asset"
                };

                asset.transform.SetParent(SaveLoadUtility.assetsParent);

                SelectableObject modelSelectable = asset.AddComponent<SelectableObject>();
                modelSelectable.type = type;

                asset.AddComponent<GltfAsset>();
                return asset;

            case AssetType.Camera:

                var camera = Instantiate(cameraAssetPrefab, SaveLoadUtility.assetsParent);
                camera.name = "Asset";

                SelectableObject cameraSelectable = camera.AddComponent<SelectableObject>();
                cameraSelectable.type = type;
                return camera;

            case AssetType.Label:

                var label = Instantiate(labelAssetPrefab, SaveLoadUtility.assetsParent);
                label.name = "Asset";

                SelectableObject labelSelectable = label.AddComponent<SelectableObject>();
                labelSelectable.type = type;
                return label;

            default: return null;
        }
    }

    // setting assets up from a scene
    public Transform[] children;
    private List<Transform> InitializeImportedAssets() // gabella
    {
        // setting scene obj as child of placeholder
        //Transform sceneObj = GameObject.Find("Scene").transform;
        //if (sceneObj != null) sceneObj.SetParent(SaveLoadUtility.assetsParent);

        // removing spawner
        //var spawner = SaveLoadUtility.assetsParent.gameObject.GetComponentInChildren<GltfAsset>();
        //Destroy(spawner.gameObject.GetComponent<SelectableObject>());
        //spawner.gameObject.name = "glTF Asset";

        GltfAsset[] spawners = SaveLoadUtility.assetsParent.gameObject.GetComponentsInChildren<GltfAsset>();
        foreach (GltfAsset spawner in spawners)
        {
            Destroy(spawner.gameObject.GetComponent<SelectableObject>());
            spawner.gameObject.name = "glTF Asset";
        }

        Array.Clear(children, 0, children.Length);
        children = SaveLoadUtility.assetsParent.gameObject.GetComponentsInChildren<Transform>();

        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].gameObject.name == "Asset") modelsInScene.Add(children[i]);
        }

        foreach (var asset in modelsInScene)
        {
            asset.SetParent(SaveLoadUtility.assetsParent);
            var selectable = asset.gameObject.AddComponent<SelectableObject>();
            selectable.type = AssetType.Model;
        }

        //Destroy(sceneObj.gameObject);

        return modelsInScene;
    }

    /// <summary>
    /// Assigns textures to corresponding materials
    /// </summary>
    private void AssignTextures()
    {
        for (int i = 0; i < modelsInScene.Count; i++)
        {
            Renderer renderer = modelsInScene[i].gameObject.GetComponentInChildren<MeshRenderer>();

            if (renderer != null)
            {
                Material material = renderer.material;

                if (material != null)
                {
                    string currentAssetPath = @$"\Asset {i + 1}" + @"\Texture.png";
                    string fullPath = SaveLoadUtility.assetsSavePath + currentAssetPath;

                    material.mainTexture = OpenDirectoryAndLoadTexture(fullPath);
                }
            }
        }
    }

    /// <summary>
    /// Loads texture from given path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private Texture2D OpenDirectoryAndLoadTexture(string path)
    {
        byte[] loadedBytes = File.ReadAllBytes(path);

        Texture2D textureFromBytes = new Texture2D(2, 2);
        textureFromBytes.LoadImage(loadedBytes);

        return textureFromBytes;
    }

    /// <summary>
    /// Adds convex mesh collider to
    /// all renderers in List of targets
    /// </summary>
    /// <param name="assets">Collection of targets</param>
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
