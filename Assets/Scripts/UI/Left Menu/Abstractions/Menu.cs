using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] private RaycastItemSelection raycast;

    [SerializeField] private Table modelTable;
    [SerializeField] private Table cameraTable;
    [SerializeField] private Table labelTable;

    private LabelTabController LabelTabController;

    private CameraModesSelection CameraModesSelection;

    private Color32 unselectedDots = new Color32(221, 223, 229, 255);
    private Color32 selectedDots = new Color32(63, 106, 204, 255);

    public Dictionary<ObjectType, Table> tablesDict = new Dictionary<ObjectType, Table>(); // dictionary to relate object with table to write

    private void Start()
    {
        LabelTabController = GetComponentInChildren<LabelTabController>(true);

        CameraModesSelection = GetComponentInChildren<CameraModesSelection>(true);

        tablesDict[ObjectType.Model] = modelTable;
        tablesDict[ObjectType.Camera] = cameraTable;
        tablesDict[ObjectType.Label] = labelTable;
    }

    public void SelectRow(bool isSelected)
    {
        if (raycast.selectedObject != null)
        {
            foreach (Row item in tablesDict[raycast.raycastSelectableObj.type].rows) // deselect all rows
            {
                Image[] allDots = item.GetComponentsInChildren<Image>();

                allDots[1].color = unselectedDots;
            }

            Image[] selectedDots = tablesDict[raycast.raycastSelectableObj.type].rows[raycast.indexOfSelected].gameObject.GetComponentsInChildren<Image>();

            selectedDots[1].color = isSelected ? this.selectedDots : unselectedDots; // change selection dots color according to selection bool

            if (raycast.raycastSelectableObj.type == ObjectType.Camera) // additional logic for camera selection
            {
                CameraModesSelection.ballCamera = isSelected ? raycast.selectedObject.GetComponentInChildren<Camera>() : null;
            }

            if (raycast.raycastSelectableObj.type == ObjectType.Label) // additional logic for label selection
            {
                LabelTabController.editLabelToggle = tablesDict[ObjectType.Label].rows[raycast.indexOfSelected].GetComponentInChildren<Toggle>();

                TMP_Text[] labelTexts = tablesDict[ObjectType.Label].rows[raycast.indexOfSelected].GetComponentsInChildren<TMP_Text>();

                LabelTabController.currentTitle = labelTexts[1];
                LabelTabController.currentDescription = labelTexts[2];

                LabelTabController.ShowOnScreenLabel(isSelected);
            }

            UpdateRowNumber();
        }
    }

    public void UpdateRowNumber() // assign text number to (index in list + 1)
    {
        foreach (Row item in tablesDict[raycast.raycastSelectableObj.type].rows)
        {
            var numberInCircle = item.GetComponentInChildren<TMP_Text>();

            numberInCircle.text = (tablesDict[raycast.raycastSelectableObj.type].rows.IndexOf(item) + 1).ToString();
        }
    }
}