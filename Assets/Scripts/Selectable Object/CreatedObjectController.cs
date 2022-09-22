using UnityEngine;
using System.Collections.Generic;

public class CreatedObjectController : MonoBehaviour
{
    [SerializeField] private Table modelTable;
    [SerializeField] private Table cameraTable;
    [SerializeField] private Table labelTable;

    public List<GameObject> models;
    public List<GameObject> cameras;
    public List<GameObject> labels;

    public Dictionary<ObjectType, List<GameObject>> dictOfLists = new Dictionary<ObjectType, List<GameObject>>(); // dictionary to relate object with list to write
    public Dictionary<ObjectType, Table> dictOfTables = new Dictionary<ObjectType, Table>(); // dictionary to relate object with table to write

    private void Start()
    {
        dictOfLists[ObjectType.Model] = models;
        dictOfLists[ObjectType.Camera] = cameras;
        dictOfLists[ObjectType.Label] = labels;

        dictOfTables[ObjectType.Model] = modelTable;
        dictOfTables[ObjectType.Camera] = cameraTable;
        dictOfTables[ObjectType.Label] = labelTable;
    }

    public void AddCreatedObjectToList(SelectableObject selectableObject) // called on Start() of each SelectableObject
    {
        dictOfLists[selectableObject.type].Add(selectableObject.gameObject);

        dictOfTables[selectableObject.type].CreateRowAndAddToList();
    }

    public void RemoveCreatedObjectFromList(SelectableObject selectableObject) // called on OnDestroy() of each SelectableObject
    {
        dictOfTables[selectableObject.type].DestroyRowAndRemoveFromList();

        dictOfLists[selectableObject.type].Remove(selectableObject.gameObject);
    }
}