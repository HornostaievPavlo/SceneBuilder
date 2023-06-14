using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Menu : MonoBehaviour
{
    [SerializeField] private SelectionSystem _raycast;

    [Space]

    [Tooltip("Models tab gameObject")]
    [SerializeField]
    private Table _modelsTable;

    [Tooltip("Cameras tab gameObject")]
    [SerializeField]
    private Table _camerasTable;

    [Tooltip("Labels tab gameObject")]
    [SerializeField]
    private Table _labelsTable;

    private LabelTabController _labelTabController;

    private CameraModesSelection _cameraModesSelection;

    private Color32 _unselectedDots = new Color32(221, 223, 229, 255);
    private Color32 _selectedDots = new Color32(63, 106, 204, 255);

    // dictionary to relate object with table to write
    public Dictionary<ObjectType, Table> tablesDict = new Dictionary<ObjectType, Table>();

    private void Start()
    {
        _labelTabController = GetComponentInChildren<LabelTabController>(true);

        _cameraModesSelection = GetComponentInChildren<CameraModesSelection>(true);

        tablesDict[ObjectType.Model] = _modelsTable;
        tablesDict[ObjectType.Camera] = _camerasTable;
        tablesDict[ObjectType.Label] = _labelsTable;
    }

    /// <summary>
    /// Highlights ui row corresponding to currently selected object,
    /// updates its number is list
    /// </summary>
    /// <param name="isSelected">Is object selected or not</param>
    public void SelectRow(bool isSelected)
    {
        if (_raycast.selectedObject != null)
        {
            foreach (Row item in tablesDict[_raycast.raycastSelectableObj.type].rowsList)
            {
                Image[] allDots = item.GetComponentsInChildren<Image>();

                allDots[1].color = _unselectedDots;
            }

            Image[] selectedDots = tablesDict[_raycast.raycastSelectableObj.type].rowsList[_raycast.indexOfSelected].gameObject.GetComponentsInChildren<Image>();

            // changing selection dots color according to selection bool
            selectedDots[1].color = isSelected ? this._selectedDots : _unselectedDots;

            // additional logic for camera selection
            if (_raycast.raycastSelectableObj.type == ObjectType.Camera)
            {
                _cameraModesSelection.ballCamera = isSelected ? _raycast.selectedObject.GetComponentInChildren<Camera>() : null;
            }

            // additional logic for label selection
            if (_raycast.raycastSelectableObj.type == ObjectType.Label)
            {
                _labelTabController.editLabelToggle = tablesDict[ObjectType.Label].rowsList[_raycast.indexOfSelected].GetComponentInChildren<Toggle>();

                TMP_Text[] labelTexts = tablesDict[ObjectType.Label].rowsList[_raycast.indexOfSelected].GetComponentsInChildren<TMP_Text>();

                _labelTabController.currentTitle = labelTexts[1];
                _labelTabController.currentDescription = labelTexts[2];

                _labelTabController.ShowOnScreenLabel(isSelected);
            }

            UpdateRowNumber();
        }
    }

    /// <summary>
    /// Assigns number of selected object to UI circle text
    /// </summary>
    public void UpdateRowNumber() // assign text number to (index in list + 1)
    {
        foreach (Row item in tablesDict[_raycast.raycastSelectableObj.type].rowsList)
        {
            var circleText = item.GetComponentInChildren<TMP_Text>();

            int numberOfObject = tablesDict[_raycast.raycastSelectableObj.type].rowsList.IndexOf(item) + 1;

            circleText.text = numberOfObject.ToString();
        }
    }
}