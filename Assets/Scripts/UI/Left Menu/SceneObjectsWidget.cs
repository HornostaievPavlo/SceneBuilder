using UnityEngine;
using UnityEngine.UI;

public class SceneObjectsWidget : MonoBehaviour
{
    [SerializeField] private Toggle openToggle;
    [SerializeField] private Toggle expandToggle;
    [SerializeField] private RectTransform content;
    
    [SerializeField] private ToggleGroup tabsToggleGroup;
    [SerializeField] private GameObject lineSeparator;

    private Toggle[] tabsToggles;
    
    private float backgroundWidth;
    private float backgroundHeight;
    
    private void Awake()
    {
        backgroundWidth = content.sizeDelta.x;
        backgroundHeight = content.sizeDelta.y;
        
        content.sizeDelta = new Vector2(backgroundWidth, 0f);
        
        expandToggle.gameObject.SetActive(false);
        lineSeparator.SetActive(false);

        tabsToggles = tabsToggleGroup.gameObject.GetComponentsInChildren<Toggle>(true);
    }

    private void OnEnable()
    {
        openToggle.onValueChanged.AddListener(HandleOpenToggleValueChanged);
        expandToggle.onValueChanged.AddListener(HandleExpandToggleValueChanged);
    }
    
    private void OnDisable()
    {
        openToggle.onValueChanged.RemoveListener(HandleOpenToggleValueChanged);
        expandToggle.onValueChanged.RemoveListener(HandleExpandToggleValueChanged);
    }

    private void HandleOpenToggleValueChanged(bool value)
    {
        if (value == false)
        {
            expandToggle.SetIsOnWithoutNotify(false);
            expandToggle.transform.eulerAngles = Vector3.zero;
            expandToggle.gameObject.SetActive(false);
            
           tabsToggleGroup.SetAllTogglesOff();
        }
        
        float targetHeight = value 
            ? backgroundHeight / 2 
            : 0f;

        content.sizeDelta = new Vector2(backgroundWidth, targetHeight);
        
        openToggle.transform.eulerAngles = new Vector3(0, 0, value ? 180 : 0);
        expandToggle.gameObject.SetActive(value);
        lineSeparator.SetActive(value);

        foreach (Toggle toggle in tabsToggles)
        {
            toggle.interactable = value;
        }
    }

    private void HandleExpandToggleValueChanged(bool value)
    {
        float targetHeight = value 
            ? backgroundHeight 
            : backgroundHeight / 2;

        content.sizeDelta = new Vector2(backgroundWidth, targetHeight);
        expandToggle.transform.eulerAngles = new Vector3(0, 0, value ? 180 : 0);
    }
}
