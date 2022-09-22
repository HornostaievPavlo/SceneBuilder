using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LabelStringsInitializer : MonoBehaviour
{
    private LabelTabController LabelTabController;

    void Start()
    {
        LabelTabController = GetComponentInParent<LabelTabController>();

        AddTitleAndDescription();
    }

    public void AddTitleAndDescription() // take title and description for each new row
    {
        TMP_Text[] textsFromSelectedRow = gameObject.GetComponentsInChildren<TMP_Text>();

        textsFromSelectedRow[1].text = LabelTabController.title;
        textsFromSelectedRow[2].text = LabelTabController.description;

        Toggle toggle = gameObject.GetComponentInChildren<Toggle>(); // edit functionality works as toggle

        toggle.onValueChanged.AddListener(delegate { LabelTabController.EditLabel(toggle.isOn); });

        LabelTabController.HideDataMenu();
    }
}