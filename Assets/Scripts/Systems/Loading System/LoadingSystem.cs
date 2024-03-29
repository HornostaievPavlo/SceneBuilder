using GLTFast;
using System;
using System.Collections.Generic;
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

    [SerializeField]
    private GameObject loadPopUp;

    private List<Transform> assetsInScene = new List<Transform>();

    private List<Transform> modelsFromSingleSaveFile = new List<Transform>();

    [HideInInspector]
    public Transform[] children;

    /// <summary>
    /// Handles loading assets from local path
    /// provided in input field
    /// </summary>
    public async void LoadAssetsFromDirectory()
    {
        var input = inputField.text;
        if (input == string.Empty) return;

        await LoadModelsFromPath(input);
    }

    /// <summary>
    /// Handles loading assets from local save file
    /// </summary>
    /// <param name="sceneNumber">Index of save file</param>
    public async void LoadAssetsFromSaveFile(int sceneNumber)
    {
        string saveFilePath =
            IOUtility.scenePath + sceneNumber.ToString() + IOUtility.sceneFile;

        bool success = await LoadModelsFromPath(saveFilePath);
        if (success) AssignTextures(sceneNumber);
    }

    /// <summary>
    /// Adds camera asset to a scene
    /// </summary>
    public void LoadCameraAsset() => CreateAsset(AssetType.Camera);

    /// <summary>
    /// Adds label asset to a scene
    /// </summary>
    public void LoadLabelAsset() => CreateAsset(AssetType.Label);

    /// <summary>
    /// General model loading procedure. 
    /// Handles adding of colliders to models
    /// </summary>
    /// <param name="modelPath">Path to .glb file in local storage</param>
    /// <returns>Success of loading</returns>
    private async Task<bool> LoadModelsFromPath(string modelPath)
    {
        loadPopUp.SetActive(true);

        assetsInScene.Clear();

        var asset = CreateAsset(AssetType.Model).GetComponent<GltfAsset>();

        var success = await asset.Load(modelPath);

        if (success)
        {
            if (modelPath.Contains(IOUtility.dataPath))
            {
                var assets = InitializeImportedAssets();
                AddCollidersToAssets(assets);
            }
            else
            {
                Transform[] children = IOUtility.assetsParent.gameObject.GetComponentsInChildren<Transform>();

                for (int i = 0; i < children.Length; i++)
                {
                    if (children[i].gameObject.name == "Asset") assetsInScene.Add(children[i]);
                }

                AddCollidersToAssets(assetsInScene);
            }

            loadPopUp.SetActive(false);
        }
        return success;
    }

    /// <summary>
    /// Creates new selectable asset of specified type
    /// </summary>
    /// <param name="type">Specifies AssetType of created object</param>
    /// <returns>Created instance</returns>
    private GameObject CreateAsset(AssetType type)
    {
        switch (type)
        {
            case AssetType.Model:

                GameObject asset = new GameObject { name = "Asset" };

                asset.transform.SetParent(IOUtility.assetsParent);

                SelectableObject modelSelectable = asset.AddComponent<SelectableObject>();
                modelSelectable.type = type;

                asset.AddComponent<GltfAsset>();
                return asset;

            case AssetType.Camera:

                var camera = Instantiate(cameraAssetPrefab, IOUtility.assetsParent);
                camera.name = "Asset";

                SelectableObject cameraSelectable = camera.AddComponent<SelectableObject>();
                cameraSelectable.type = type;
                return camera;

            case AssetType.Label:

                var label = Instantiate(labelAssetPrefab, IOUtility.assetsParent);
                label.name = "Asset";

                SelectableObject labelSelectable = label.AddComponent<SelectableObject>();
                labelSelectable.type = type;
                return label;

            default: return null;
        }
    }

    /// <summary>
    /// Rearranges imported assets in proper hierarchy 
    /// </summary>
    /// <returns>Collection of assets in scene</returns>
    private List<Transform> InitializeImportedAssets()
    {
        modelsFromSingleSaveFile.Clear();

        bool isSingleAsset = GameObject.Find("Scene") == null;
        Transform sceneObj = null;

        if (!isSingleAsset)
        {
            sceneObj = GameObject.Find("Scene").transform;
            sceneObj.SetParent(IOUtility.assetsParent);
        }

        var spawner = IOUtility.assetsParent.gameObject.GetComponentInChildren<GltfAsset>();
        Destroy(spawner.gameObject.GetComponent<SelectableObject>());
        spawner.gameObject.name = "glTF Asset";

        GltfAsset[] spawners = IOUtility.assetsParent.gameObject.GetComponentsInChildren<GltfAsset>();
        foreach (GltfAsset item in spawners)
        {
            Destroy(item.gameObject.GetComponent<SelectableObject>());
            item.gameObject.name = "glTF Asset";
        }

        Array.Clear(children, 0, children.Length);
        children = IOUtility.assetsParent.gameObject.GetComponentsInChildren<Transform>();

        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].gameObject.name == "Asset") assetsInScene.Add(children[i]);
        }

        foreach (var asset in assetsInScene)
        {
            asset.SetParent(IOUtility.assetsParent);

            bool hasSelectable = asset.GetComponentInChildren<SelectableObject>() != null;

            if (!hasSelectable)
            {
                var selectable = asset.gameObject.AddComponent<SelectableObject>();
                selectable.type = AssetType.Model;

                modelsFromSingleSaveFile.Add(asset.transform);
            }
        }

        if (sceneObj) Destroy(sceneObj.gameObject);

        return assetsInScene;
    }

    /// <summary>
    /// Assigns textures to corresponding materials
    /// </summary>
    private void AssignTextures(int sceneNumber)
    {
        for (int i = 0; i < modelsFromSingleSaveFile.Count; i++)
        {
            Renderer renderer = modelsFromSingleSaveFile[i].gameObject.GetComponentInChildren<MeshRenderer>();

            if (renderer != null)
            {
                Material material = renderer.material;

                if (material != null)
                {
                    string currentAssetPath = $"/Asset{i + 1}" + IOUtility.textureFile;
                    string fullPath = IOUtility.scenePath + sceneNumber + currentAssetPath;

                    material.mainTexture = IOUtility.OpenDirectoryAndLoadTexture(fullPath);
                }
            }
        }
        modelsFromSingleSaveFile.Clear();
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
