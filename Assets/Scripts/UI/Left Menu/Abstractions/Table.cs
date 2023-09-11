using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] 
    private Row rowPrefab;

    [SerializeField] 
    private Transform rowParent;

    private void Start()
    {
        //var applicationUI = GetComponentInParent<ApplicationUI>();
        //_selectionSystem = applicationUI.selectionSystem;
    }

    public Row CreateRowForSelectable()
    {
        var row = Instantiate(rowPrefab, rowParent);

        return row;
    }

    //public void DestroyRow()
    //{
    //    if (rowsList[_selectionSystem.indexOfSelected] != null)
    //    {
    //        Destroy(rowsList[_selectionSystem.indexOfSelected].gameObject);
    //    }

    //    rowsList.RemoveAt(_selectionSystem.indexOfSelected);
    //}
}