using System.Collections.Generic;
using UnityEngine;

public class CreatedObjectManager : MonoBehaviour
{
    [Tooltip("Tab where objects will be added")]
    [SerializeField]
    private Table _modelTable;

    [Tooltip("Tab where objects will be added")]
    [SerializeField]
    private Table _cameraTable;

    [Tooltip("Tab where objects will be added")]
    [SerializeField]
    private Table _labelTable;

    [HideInInspector] public List<GameObject> models;
    [HideInInspector] public List<GameObject> cameras;
    [HideInInspector] public List<GameObject> labels;

    // dictionary to relate object by type with list to write in
    public Dictionary<ObjectType, List<GameObject>> dictOfLists = new Dictionary<ObjectType, List<GameObject>>();

    // dictionary to relate object by type with table to write in
    public Dictionary<ObjectType, Table> dictOfTables = new Dictionary<ObjectType, Table>();

    private void Start()
    {
        dictOfLists[ObjectType.Model] = models;
        dictOfLists[ObjectType.Camera] = cameras;
        dictOfLists[ObjectType.Label] = labels;

        dictOfTables[ObjectType.Model] = _modelTable;
        dictOfTables[ObjectType.Camera] = _cameraTable;
        dictOfTables[ObjectType.Label] = _labelTable;
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