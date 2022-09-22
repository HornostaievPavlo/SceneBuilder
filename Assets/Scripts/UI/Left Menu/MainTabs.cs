using UnityEngine;

public class MainTabs : MonoBehaviour
{
    [SerializeField] private GameObject modelsTab;
    [SerializeField] private GameObject camerasTab;
    [SerializeField] private GameObject labelsTab;


    public void SelectModelsTab(bool isModelsTab) // cannot make one method because toggle event is not taking two parameters
    {
        modelsTab.SetActive(isModelsTab);
    }

    public void SelectCamerasTab(bool isCamerasTab)
    {
        camerasTab.SetActive(isCamerasTab);
    }

    public void SelectLabelsTab(bool isLabelsTab)
    {
        labelsTab.SetActive(isLabelsTab);
    }
}