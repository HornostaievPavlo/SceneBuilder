using UnityEngine;
using System.Collections.Generic;

public class Table : MonoBehaviour
{
    [SerializeField] private RaycastItemSelection RaycastItemSelection;

    [SerializeField] private Row row;

    [SerializeField] private Transform parent;

    public List<Row> rows;

    public void CreateRowAndAddToList() // add row on Awake() of object
    {
        Row copy = Instantiate(row, parent);

        rows.Add(copy);
    }

    public void DestroyRowAndRemoveFromList() // remove row on OnDestroy() of object
    {
        if (rows[RaycastItemSelection.indexOfSelected] != null)
        {
            Destroy(rows[RaycastItemSelection.indexOfSelected].gameObject);
        }

        rows.RemoveAt(RaycastItemSelection.indexOfSelected);
    }
}