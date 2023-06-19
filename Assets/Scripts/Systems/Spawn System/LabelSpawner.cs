using UnityEngine;

public class LabelSpawner : MonoBehaviour
{
    [Tooltip("Prefab that should be spawned")]
    [SerializeField]
    private GameObject _labelPrefab;

    private Transform _labelParent;

    private void Start()
    {
        _labelParent = this.transform;
    }

    /// <summary>
    /// Instantiates label from prefab,
    /// adds SelectableObject component,
    /// sets object type to Label
    /// </summary>
    public void SpawnLabel()
    {
        Vector3 spawnPosition = new Vector3(0, 0.5f, -2.5f);
        var labelPrefabCopy = Instantiate(_labelPrefab, spawnPosition, _labelPrefab.transform.rotation, _labelParent);

        SelectableObject selectableObject = labelPrefabCopy.gameObject.AddComponent<SelectableObject>();
        selectableObject.type = ObjectType.Label;
    }
}