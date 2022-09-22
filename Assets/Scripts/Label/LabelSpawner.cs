using UnityEngine;

public class LabelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject labelPrefab;

    private Transform labelParent;

    private void Start()
    {
        labelParent = this.transform;
    }

    public void SpawnLabel()
    {
        Vector3 spawnPosition = new Vector3(0, 0.5f, -2.5f);
        var labelPrefabCopy = Instantiate(labelPrefab, spawnPosition, labelPrefab.transform.rotation, labelParent);

        SelectableObject selectableObject = labelPrefabCopy.gameObject.AddComponent<SelectableObject>();
        selectableObject.type = ObjectType.Label;
    }
}