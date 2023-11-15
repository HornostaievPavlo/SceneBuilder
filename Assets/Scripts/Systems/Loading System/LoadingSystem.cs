using GLTFast;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject cameraAssetPrefab;

    [SerializeField]
    private GameObject labelAssetPrefab;

    [SerializeField]
    private GameObject loadPopUp;

    [SerializeField]
    private Transform assetsParent;

    private void OnEnable()
    {
        ModelUploadingSystem.OnModelUploaded += OnModelUploaded;
    }

    private void OnDisable()
    {
        ModelUploadingSystem.OnModelUploaded -= OnModelUploaded;
    }

    private void OnModelUploaded(byte[] data) => LoadAssetFromBytes(data);

    public void LoadCameraAsset() => CreateAsset(AssetType.Camera);

    public void LoadLabelAsset() => CreateAsset(AssetType.Label);

    /// <summary>
    /// Handles loading of model from byte array 
    /// </summary>
    /// <param name="bytes">Bytes to load into scene</param>
    private async void LoadAssetFromBytes(byte[] bytes)
    {
        loadPopUp.SetActive(true);

        var asset = CreateAsset(AssetType.Model);

        var gltf = new GltfImport();

        bool success = await gltf.LoadGltfBinary(bytes);

        if (success)
        {
            await gltf.InstantiateMainSceneAsync(asset.transform);

            List<Transform> assets = new List<Transform>();

            SelectableObject[] loadedAssets = assetsParent.GetComponentsInChildren<SelectableObject>();

            foreach (var selectable in loadedAssets)
            {
                assets.Add(selectable.gameObject.transform);
            }

            AddCollidersToAssets(assets);

            loadPopUp.SetActive(false);
        }
    }

    /// <summary>
    /// Creates new Selectable asset of specified type
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

                asset.transform.SetParent(assetsParent);

                SelectableObject modelSelectable = asset.AddComponent<SelectableObject>();
                modelSelectable.type = type;

                asset.AddComponent<GltfAsset>();
                return asset;

            case AssetType.Camera:

                var camera = Instantiate(cameraAssetPrefab, assetsParent);
                camera.name = "Asset";

                SelectableObject cameraSelectable = camera.AddComponent<SelectableObject>();
                cameraSelectable.type = type;
                return camera;

            case AssetType.Label:

                var label = Instantiate(labelAssetPrefab, assetsParent);
                label.name = "Asset";

                SelectableObject labelSelectable = label.AddComponent<SelectableObject>();
                labelSelectable.type = type;
                return label;

            default: return null;
        }
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
