using System.Collections.Generic;
using Enums;
using UnityEngine;

public class RowsCoordinator : MonoBehaviour
{
    [SerializeField]
    private TableWidget modelsTableWidget;

    [SerializeField]
    private TableWidget camerasTableWidget;

    [SerializeField]
    private TableWidget labelsTableWidget;

    public Dictionary<AssetTypeId, TableWidget> table = new Dictionary<AssetTypeId, TableWidget>();

    private void Start()
    {
        table[AssetTypeId.Model] = modelsTableWidget;
        table[AssetTypeId.Camera] = camerasTableWidget;
        table[AssetTypeId.Label] = labelsTableWidget;
    }

    public Row AssignRow(SelectableObject selectable)
    {
        return table[selectable.TypeId].CreateRowForSelectable();
    }

    public void RemoveRow(SelectableObject selectable)
    {
        table[selectable.TypeId].DeleteRowItem(selectable.MenuRow);
    }
}