using UnityEngine;
using UnityEngine.UI;

public class SceneObjectsControlsWidget : MonoBehaviour
{
    [SerializeField] private Toggle openToggle;
    [SerializeField] private Toggle expandToggle;
    
    [SerializeField] private RectTransform background;
    
    private float backgroundWidth;
    
    private const float CollapsedBackgroundHeight = 72f;
    private const float OpenedBackgroundHeight = 400f;
    private const float ExpandedBackgroundHeight = 890f;

    private void Awake()
    {
        backgroundWidth = background.sizeDelta.x;
        expandToggle.gameObject.SetActive(false);
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
            expandToggle.isOn = false;
            expandToggle.transform.eulerAngles = Vector3.zero;
            expandToggle.gameObject.SetActive(false);
        }
        
        float targetHeight = value 
            ? OpenedBackgroundHeight 
            : CollapsedBackgroundHeight;

        background.sizeDelta = new Vector2(backgroundWidth, targetHeight);
        
        openToggle.transform.eulerAngles = new Vector3(0, 0, value ? 180 : 0);
        expandToggle.gameObject.SetActive(value);
        
       // Toggle[] toggles = tabsToggleGroup.GetComponentsInChildren<Toggle>();

        // foreach (Toggle item in toggles)
        // {
        //     item.interactable = isClosed;
        // }

        // if (!isClosed)
        // {
        //     foreach (Toggle item in toggles)
        //     {
        //         item.isOn = false;
        //     }
        // }
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
