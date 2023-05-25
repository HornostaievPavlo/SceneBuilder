using UnityEngine;

public class MainTabs : MonoBehaviour
{
    [SerializeField] private GameObject _modelsTab;
    [SerializeField] private GameObject _camerasTab;
    [SerializeField] private GameObject _labelsTab;

    /// <summary>
    /// Toggles models tab on/off
    /// </summary>
    /// <param name="isModelsTab">Is tab selected or unselected</param>
    public void SelectModelsTab(bool isModelsTab)
    {
        _modelsTab.SetActive(isModelsTab);
    }

    /// <summary>
    /// Toggles cameras tab on/off
    /// </summary>
    /// <param name="isCamerasTab">Is tab selected or unselected</param>
    public void SelectCamerasTab(bool isCamerasTab)
    {
        _camerasTab.SetActive(isCamerasTab);
    }

    /// <summary>
    /// Toggles labels tab on/off
    /// </summary>
    /// <param name="isLabelsTab">Is tab selected or unselected</param>
    public void SelectLabelsTab(bool isLabelsTab)
    {
        _labelsTab.SetActive(isLabelsTab);
    }
}