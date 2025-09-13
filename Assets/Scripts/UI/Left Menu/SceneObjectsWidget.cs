using UnityEngine;
using UnityEngine.UI;

public class SceneObjectsWidget : MonoBehaviour
{
    [SerializeField] private Toggle openToggle;
    [SerializeField] private Toggle expandToggle;
    [SerializeField] private RectTransform background;
    
    [SerializeField] private ToggleGroup tabsToggleGroup;
    [SerializeField] private GameObject lineSeparator;

    private Toggle[] tabsToggles;
    
    private float backgroundWidth;
    
    private const float CollapsedBackgroundHeight = 72f;
    private const float OpenedBackgroundHeight = 400f;
    private const float ExpandedBackgroundHeight = 890f;

    private void Awake()
    {
        backgroundWidth = background.sizeDelta.x;
        background.sizeDelta = new Vector2(backgroundWidth, CollapsedBackgroundHeight);
        
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
            ? OpenedBackgroundHeight 
            : CollapsedBackgroundHeight;

        background.sizeDelta = new Vector2(backgroundWidth, targetHeight);
        
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
            ? ExpandedBackgroundHeight 
            : OpenedBackgroundHeight;

        background.sizeDelta = new Vector2(backgroundWidth, targetHeight);
        expandToggle.transform.eulerAngles = new Vector3(0, 0, value ? 180 : 0);
    }
}
