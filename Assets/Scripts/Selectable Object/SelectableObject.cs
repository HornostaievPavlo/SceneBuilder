using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public AssetType type;

    public Row row;

    private RowsCoordinator rowsCoordinator;

    private void Start()
    {
        rowsCoordinator = GetComponentInParent<RowsCoordinator>();
        row = rowsCoordinator.AssignRow(this);
    }

    private void OnDestroy()
    {
        rowsCoordinator.DestroyRow(this);
    }
}