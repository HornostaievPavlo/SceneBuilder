using UnityEngine;

public class CameraSpawner : MonoBehaviour
{
    [Tooltip("Prefab that should be spawned")]
    [SerializeField]
    private GameObject _cameraPrefab;

    private Transform _cameraParent;

    private void Start()
    {
        _cameraParent = this.transform;
    }

    /// <summary>
    /// Instantiates camera from prefab,
    /// adds SelectableObject component,
    /// sets object type to Camera
    /// </summary>
    public void SpawnCamera()
    {
        Vector3 spawnPosition = new Vector3(0, 0.5f, -5);
        var cameraPrefabCopy = Instantiate(_cameraPrefab, spawnPosition, _cameraPrefab.transform.rotation, _cameraParent);

        SelectableObject selectableObject = cameraPrefabCopy.gameObject.AddComponent<SelectableObject>();
        selectableObject.type = AssetType.Camera;
    }
}