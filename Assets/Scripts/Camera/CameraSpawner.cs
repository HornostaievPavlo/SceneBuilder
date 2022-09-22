using UnityEngine;

public class CameraSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cameraPrefab;

    private Transform cameraParent;

    private void Start()
    {
        cameraParent = this.transform;
    }

    public void SpawnCamera()
    {
        Vector3 spawnPosition = new Vector3(0, 0.5f, -5);
        var cameraPrefabCopy = Instantiate(cameraPrefab, spawnPosition, cameraPrefab.transform.rotation, cameraParent);

        SelectableObject selectableObject = cameraPrefabCopy.gameObject.AddComponent<SelectableObject>();
        selectableObject.type = ObjectType.Camera;
    }
}