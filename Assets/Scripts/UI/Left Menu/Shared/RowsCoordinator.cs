using System.Collections.Generic;
using Enums;
using Gameplay;
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

    public Row AssignRow(SceneObject scene)
    {
        return table[scene.AssetTypeId].CreateRowForSelectable();
    }

    public void RemoveRow(SceneObject scene)
    {
        // table[scene.AssetTypeId].DeleteRowItem(scene.MenuRow);
    }
}