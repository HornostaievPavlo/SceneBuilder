using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private Row rowPrefab;

    [SerializeField] private Transform rowParent;

    private SelectionSystem _selectionSystem;

    [HideInInspector] public List<Row> rowsList;

    private void Start()
    {
        var applicationUI = GetComponentInParent<ApplicationUI>();

        //_selectionSystem = applicationUI.selectionSystem;
    }

    /// <summary>
    /// Creates new row when object is created
    /// </summary>
    public void CreateRowAndAddToList()
    {
        Row newRow = Instantiate(rowPrefab, rowParent);

        rowsList.Add(newRow);
    }

    /// <summary>
    /// Destroys row when object is deleted from scene
    /// </summary>
    //public void DestroyRowAndRemoveFromList()
    //{
    //    if (rowsList[_selectionSystem.indexOfSelected] != null)
    //    {
    //        Destroy(rowsList[_selectionSystem.indexOfSelected].gameObject);
    //    }

    //    rowsList.RemoveAt(_selectionSystem.indexOfSelected);
    //}
}