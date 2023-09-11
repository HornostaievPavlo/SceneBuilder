using System.Collections.Generic;
using UnityEngine;

public class RowsCoordinator : MonoBehaviour
{
    [SerializeField]
    private Table modelsTable;

    [SerializeField]
    private Table camerasTable;

    [SerializeField]
    private Table labelsTable;

    public Dictionary<AssetType, Table> table = new Dictionary<AssetType, Table>();

    private void Start()
    {
        table[AssetType.Model] = modelsTable;
        table[AssetType.Camera] = camerasTable;
        table[AssetType.Label] = labelsTable;
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