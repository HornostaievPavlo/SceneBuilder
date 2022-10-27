using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LabelStringsInitializer : MonoBehaviour
{
    private LabelTabController _labelTabController;

    private void Start()
    {
        _labelTabController = GetComponentInParent<LabelTabController>();

        AddTitleAndDescription();
    }

    /// <summary>
    /// Adds entered text to UI row fields 
    /// </summary>
    public void AddTitleAndDescription() // take title and description for each new row
    {
        TMP_Text[] textsFromSelectedRow = gameObject.GetComponentsInChildren<TMP_Text>();

        textsFromSelectedRow[1].text = _labelTabController.title;
        textsFromSelectedRow[2].text = _labelTabController.description;

        Toggle toggle = gameObject.GetComponentInChildren<Toggle>();

        toggle.onValueChanged.AddListener(delegate { _labelTabController.EditLabel(toggle.isOn); });

        _labelTabController.HideDataMenu();
    }
}