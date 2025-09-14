using System.Collections.Generic;
using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TableWidget : MonoBehaviour
{
    [SerializeField] private Row rowPrefab;
    [SerializeField] private Transform rowParent;

    private List<Row> rows = new();

    private Color32 selectedRowColor = new Color32(63, 106, 204, 255);
    private Color32 unselectedRowColor = new Color32(221, 223, 229, 255);

    private void OnEnable()
    {
        SelectionSystem.OnObjectSelected += OnObjectSelected;
        SelectionSystem.OnObjectDeselected += OnObjectDeselected;
    }

    private void OnDisable()
    {
        SelectionSystem.OnObjectSelected -= OnObjectSelected;
        SelectionSystem.OnObjectDeselected -= OnObjectDeselected;
    }

    private void OnObjectSelected(SceneObject scene)
    {
        // HighlightRow(scene.MenuRow, true);
    }

    private void OnObjectDeselected()
    {
        HighlightRow(null, false);
    }

    public Row CreateRowForSelectable()
    {
        var row = Instantiate(rowPrefab, rowParent);
        rows.Add(row);

        AssignRowsNumbers(rows);

        return row;
    }

    public void DeleteRowItem(Row row)
    {
        rows.Remove(row);
        
        if (row != null)
            Destroy(row.gameObject);

        AssignRowsNumbers(rows);
    }

    private void AssignRowsNumbers(List<Row> rows)
    {
        foreach (var row in rows)
        {
            TMP_Text text = row.GetComponentInChildren<TMP_Text>();
            text.text = (rows.IndexOf(row) + 1).ToString();
        }
    }

    private void HighlightRow(Row row, bool isSelected)
    {
        if (row != null)
        {
            DeselectAllRows();
            GetHighlightDots(row).color = isSelected ? selectedRowColor : unselectedRowColor;
        }
        else
            DeselectAllRows();
    }

    private void DeselectAllRows()
    {
        foreach (var item in rows)
        {
            GetHighlightDots(item).color = unselectedRowColor;
        }
    }

    private Image GetHighlightDots(Row row)
    {
        Image[] images = row.GetComponentsInChildren<Image>();
        Image highlightningDots = images[1];

        return highlightningDots;
    }
}