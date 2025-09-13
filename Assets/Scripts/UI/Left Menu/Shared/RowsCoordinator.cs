using System.Collections.Generic;
using UnityEngine;

public class RowsCoordinator : MonoBehaviour
{
    [SerializeField]
    private TableWidget modelsTableWidget;

    [SerializeField]
    private TableWidget camerasTableWidget;

    [SerializeField]
    private TableWidget labelsTableWidget;

    public Dictionary<AssetType, TableWidget> table = new Dictionary<AssetType, TableWidget>();

    private void Start()
    {
        table[AssetType.Model] = modelsTableWidget;
        table[AssetType.Camera] = camerasTableWidget;
        table[AssetType.Label] = labelsTableWidget;
    }

    public Row AssignRow(SelectableObject selectable)
    {
        return table[selectable.type].CreateRowForSelectable();
    }

    public void RemoveRow(SelectableObject selectable)
    {
        table[selectable.type].DeleteRowItem(selectable.MenuRow);
    }
}