using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private RaycastItemSelection _raycastItemSelection;

    [Space]

    [SerializeField] private Row _row;

    [SerializeField] private Transform _parent;

    [HideInInspector] public List<Row> rowsList;

    /// <summary>
    /// Creates new row when object is created
    /// </summary>
    public void CreateRowAndAddToList()
    {
        Row newRow = Instantiate(_row, _parent);

        rowsList.Add(newRow);
    }

    /// <summary>
    /// Destroys row when object is deleted from scene
    /// </summary>
    public void DestroyRowAndRemoveFromList()
    {
        if (rowsList[_raycastItemSelection.indexOfSelected] != null)
        {
            Destroy(rowsList[_raycastItemSelection.indexOfSelected].gameObject);
        }

        rowsList.RemoveAt(_raycastItemSelection.indexOfSelected);
    }
}