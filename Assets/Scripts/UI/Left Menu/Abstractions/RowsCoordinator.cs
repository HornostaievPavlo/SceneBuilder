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

    //[HideInInspector] public List<GameObject> models;
    //[HideInInspector] public List<GameObject> cameras;
    //[HideInInspector] public List<GameObject> labels;

    // dictionary to relate object by type with list to write in
    //public Dictionary<AssetType, List<GameObject>> dictOfLists = new Dictionary<AssetType, List<GameObject>>();

    // dictionary to relate object by type with table to write in
    public Dictionary<AssetType, Table> table = new Dictionary<AssetType, Table>();

    private void Start()
    {
        //var menu = GetComponent<LeftMenu>();

        //modelsTable = menu.modelsTable;
        //camerasTable = menu.camerasTable;
        //labelsTable = menu.labelsTable;

        //dictOfLists[AssetType.Model] = models;
        //dictOfLists[AssetType.Camera] = cameras;
        //dictOfLists[AssetType.Label] = labels;

        table[AssetType.Model] = modelsTable;
        table[AssetType.Camera] = camerasTable;
        table[AssetType.Label] = labelsTable;
    }

    /// <summary>
    /// Adds created object to corresponding list,
    /// instantiates UI row in corresponding table
    /// </summary>
    /// <param name="selectableObject">Object to add</param>
    public void AddCreatedObjectToList(SelectableObject selectableObject)
    {
        //dictOfLists[selectableObject.type].Add(selectableObject.gameObject);

        //table[selectableObject.type].CreateRowForSelectable();
    }

    public Row AssignRow(SelectableObject selectable) 
    {
        return table[selectable.type].CreateRowForSelectable();    
    }

    public void DestroyRow(SelectableObject selectable)
    {
        Destroy(selectable.row.gameObject);
    }
}