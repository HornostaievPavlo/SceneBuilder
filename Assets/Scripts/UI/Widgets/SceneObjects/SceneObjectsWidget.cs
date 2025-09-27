using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets.SceneObjects
{
    public class SceneObjectsWidget : MonoBehaviour
    {
        [Header("Content")]
        [SerializeField] private Toggle openToggle;
        [SerializeField] private Toggle expandToggle;
        [SerializeField] private RectTransform content;
    
        [Header("Tabs")]
        [SerializeField] private ToggleGroup tabsToggleGroup;
        [SerializeField] private GameObject lineSeparator;

        [Header("Header")]
        [SerializeField] private Image headerImage;
        [SerializeField] private Sprite collapsedSprite;
        [SerializeField] private Sprite openedSprite;

        private Toggle[] tabsToggles;
    
        private float initialBackgroundWidth;
        private float expandedBackgroundHeight;
    
        private void Awake()
        {
            initialBackgroundWidth = content.sizeDelta.x;
            expandedBackgroundHeight = content.sizeDelta.y;
        
            content.sizeDelta = new Vector2(initialBackgroundWidth, 0f);
        
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
                HandleCollapsed();
        
            float targetHeight = value 
                ? expandedBackgroundHeight / 2 
                : 0f;

            content.sizeDelta = new Vector2(initialBackgroundWidth, targetHeight);
        
            openToggle.transform.eulerAngles = new Vector3(0, 0, value ? 180 : 0);
            expandToggle.gameObject.SetActive(value);
            lineSeparator.SetActive(value);
        
            headerImage.sprite = value 
                ? openedSprite 
                : collapsedSprite;

            foreach (Toggle toggle in tabsToggles)
            {
                toggle.interactable = value;
            }
        }

        private void HandleExpandToggleValueChanged(bool value)
        {
            float targetHeight = value 
                ? expandedBackgroundHeight 
                : expandedBackgroundHeight / 2;

            content.sizeDelta = new Vector2(initialBackgroundWidth, targetHeight);
            expandToggle.transform.eulerAngles = new Vector3(0, 0, value ? 180 : 0);
        }

        private void HandleCollapsed()
        {
            expandToggle.SetIsOnWithoutNotify(false);
            expandToggle.transform.eulerAngles = Vector3.zero;
            expandToggle.gameObject.SetActive(false);
            
            tabsToggleGroup.SetAllTogglesOff();
        }
    }
}
