using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    [HideInInspector]
    public AssetType type;

    private RowsInitializer rowsInitializer;

    private void Start()
    {
        //rowsInitializer = FindObjectOfType<RowsInitializer>();

        //rowsInitializer.AddCreatedObjectToList(this);
    }

    private void OnDestroy()
    {
        //rowsInitializer.RemoveCreatedObjectFromList(this);
    }
}