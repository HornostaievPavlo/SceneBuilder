using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class LeftMenu : MonoBehaviour
{
    [HideInInspector] public Table modelsTable;
    [HideInInspector] public Table camerasTable;
    [HideInInspector] public Table labelsTable;

    private SelectionSystem _selectionSystem;

    private LabelTabController _labelTabController;

    private CameraModesSwitcher _cameraModesSelection;

    private Color32 _unselectedDots = new Color32(221, 223, 229, 255);
    private Color32 _selectedDots = new Color32(63, 106, 204, 255);

    // dictionary to relate object with table to write
    public Dictionary<AssetType, Table> tablesDict = new Dictionary<AssetType, Table>();

    private void Awake()
    {
        var applicationUI = GetComponentInParent<ApplicationUI>();

        //_selectionSystem = applicationUI.selectionSystem;

        _labelTabController = GetComponentInChildren<LabelTabController>(true);

        _cameraModesSelection = GetComponentInChildren<CameraModesSwitcher>(true);

        Table[] tables = GetComponentsInChildren<Table>(true);
        modelsTable = tables[0];
        camerasTable = tables[1];
        labelsTable = tables[2];

        tablesDict[AssetType.Model] = modelsTable;
        tablesDict[AssetType.Camera] = camerasTable;
        tablesDict[AssetType.Label] = labelsTable;
    }

    /// <summary>
    /// Highlights ui row corresponding to currently selected object,
    /// updates its number is list
    /// </summary>
    /// <param name="isSelected">Is object selected or not</param>
    //public void SelectRow(bool isSelected)
    //{
    //    if (_selectionSystem.selectedObject != null)
    //    {
    //        foreach (Row item in tablesDict[_selectionSystem.selectableObject.type].rowsList)
    //        {
    //            Image[] allDots = item.GetComponentsInChildren<Image>();

    //            allDots[1].color = _unselectedDots;
    //        }

    //        Image[] selectedDots = tablesDict[_selectionSystem.selectableObject.type].rowsList[_selectionSystem.indexOfSelected].gameObject.GetComponentsInChildren<Image>();

    //        // changing selection dots color according to selection bool
    //        selectedDots[1].color = isSelected ? this._selectedDots : _unselectedDots;

    //        // additional logic for camera selection
    //        if (_selectionSystem.selectableObject.type == AssetType.Camera)
    //        {
    //            _cameraModesSelection.ballCamera = isSelected ? _selectionSystem.selectedObject.GetComponentInChildren<Camera>() : null;
    //        }

    //        // additional logic for label selection
    //        if (_selectionSystem.selectableObject.type == AssetType.Label)
    //        {
    //            _labelTabController.editLabelToggle = tablesDict[AssetType.Label].rowsList[_selectionSystem.indexOfSelected].GetComponentInChildren<Toggle>();

    //            TMP_Text[] labelTexts = tablesDict[AssetType.Label].rowsList[_selectionSystem.indexOfSelected].GetComponentsInChildren<TMP_Text>();

    //            _labelTabController.currentTitle = labelTexts[1];
    //            _labelTabController.currentDescription = labelTexts[2];

    //            _labelTabController.ShowOnScreenLabel(isSelected);
    //        }

    //        UpdateRowNumber();
    //    }
    //}

    /// <summary>
    /// Assigns number of selected object to UI circle text
    /// </summary>
    //public void UpdateRowNumber() // assign text number to (index in list + 1)
    //{
    //    foreach (Row item in tablesDict[_selectionSystem.selectableObject.type].rowsList)
    //    {
    //        var circleText = item.GetComponentInChildren<TMP_Text>();

    //        int numberOfObject = tablesDict[_selectionSystem.selectableObject.type].rowsList.IndexOf(item) + 1;

    //        circleText.text = numberOfObject.ToString();
    //    }
    //}
}