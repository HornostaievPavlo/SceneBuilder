using Enums;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public AssetTypeId TypeId;

    private RowsCoordinator rowsCoordinator;
    private Row menuRow;

    public Row MenuRow { get { return menuRow; } }

    private void Start()
    {
        rowsCoordinator = GetComponentInParent<RowsCoordinator>();
        menuRow = rowsCoordinator.AssignRow(this);
    }

    private void OnDestroy() => rowsCoordinator.RemoveRow(this);
}