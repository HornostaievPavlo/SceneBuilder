using System.Collections.Generic;
using UnityEngine;

public class RowsInitializer : MonoBehaviour
{
    private Table _modelsTable;
    private Table _camerasTable;
    private Table _labelsTable;

    [HideInInspector] public List<GameObject> models;
    [HideInInspector] public List<GameObject> cameras;
    [HideInInspector] public List<GameObject> labels;

    // dictionary to relate object by type with list to write in
    public Dictionary<AssetType, List<GameObject>> dictOfLists = new Dictionary<AssetType, List<GameObject>>();

    // dictionary to relate object by type with table to write in
    public Dictionary<AssetType, Table> dictOfTables = new Dictionary<AssetType, Table>();

    private void Start()
    {
        var menu = GetComponent<LeftMenu>();

        _modelsTable = menu.modelsTable;
        _camerasTable = menu.camerasTable;
        _labelsTable = menu.labelsTable;

        dictOfLists[AssetType.Model] = models;
        dictOfLists[AssetType.Camera] = cameras;
        dictOfLists[AssetType.Label] = labels;

        dictOfTables[AssetType.Model] = _modelsTable;
        dictOfTables[AssetType.Camera] = _camerasTable;
        dictOfTables[AssetType.Label] = _labelsTable;
    }

    /// <summary>
    /// Adds created object to corresponding list,
    /// instantiates UI row in corresponding table
    /// </summary>
    /// <param name="selectableObject">Object to add</param>
    public void AddCreatedObjectToList(SelectableObject selectableObject)
    {
        dictOfLists[selectableObject.type].Add(selectableObject.gameObject);

        dictOfTables[selectableObject.type].CreateRowAndAddToList();
    }

    /// <summary>
    /// Removes object from corresponding list,
    /// removes UI row from corresponding table
    /// </summary>
    /// <param name="selectableObject">Object to remove</param>
    public void RemoveCreatedObjectFromList(SelectableObject selectableObject)
    {
        dictOfTables[selectableObject.type].DestroyRowAndRemoveFromList();

        dictOfLists[selectableObject.type].Remove(selectableObject.gameObject);
    }
}