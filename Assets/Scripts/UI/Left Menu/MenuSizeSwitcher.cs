using UnityEngine;
using UnityEngine.UI;

public class MenuSizeSwitcher : MonoBehaviour
{
    [Header("Backgrounds")]
    [SerializeField]
    private RectTransform normalBackground;

    [SerializeField]
    private RectTransform expandedBackground;

    [Space]
    [Header("Toggles")]
    [SerializeField]
    private RectTransform openToggle;

    [SerializeField]
    private RectTransform expandToggle;

    [SerializeField]
    private GameObject tabsToggleGroup;

    [Space]
    [Header("Viewports")]
    [SerializeField]
    private RectTransform modelsScrollView;

    [SerializeField]
    private RectTransform camerasScrollView;

    [SerializeField]
    private RectTransform labelsScrollView;

    private bool isExpanded = false;

    /// <summary>
    /// Opens left menu according to bool value of toggle
    /// </summary>
    /// <param name="isClosed">Is menu opened or not</param>
    public void OpenMenu(bool isClosed)
    {
        if (isExpanded) ExpandMenu(false);

        Vector3 toggleRotation = new Vector3(0, 0, isClosed ? 0 : 180);
        openToggle.eulerAngles = toggleRotation;

        normalBackground.gameObject.SetActive(isClosed);
        expandToggle.gameObject.SetActive(isClosed);

        Toggle[] toggles = tabsToggleGroup.GetComponentsInChildren<Toggle>();

        foreach (Toggle item in toggles)
        {
            item.interactable = isClosed;
        }

        if (!isClosed)
        {
            foreach (Toggle item in toggles)
            {
                item.isOn = false;
            }
        }
    }

    /// <summary>
    /// Expands/collapses menu according to its state
    /// </summary>
    /// <param name="isCollapsed">Is menu half or full view</param>
    public void ExpandMenu(bool isCollapsed)
    {
        isExpanded = isCollapsed ? true : false;

        normalBackground.gameObject.SetActive(!isCollapsed);
        expandedBackground.gameObject.SetActive(isCollapsed);

        UpdateExpandToggleTransform(isCollapsed);

        ResizeScrollView(modelsScrollView, isCollapsed);
        ResizeScrollView(camerasScrollView, isCollapsed);
        ResizeScrollView(labelsScrollView, isCollapsed);
    }

    private void UpdateExpandToggleTransform(bool isCollapsed)
    {
        Transform expandToggleParent = isCollapsed ? expandedBackground : normalBackground;
        expandToggle.transform.SetParent(expandToggleParent);

        float offsetFromBottomOfMenu = 15f;
        expandToggle.anchoredPosition = new Vector3(0, offsetFromBottomOfMenu, 0);
        expandToggle.eulerAngles = new Vector3(0, 0, isCollapsed ? 0 : 180);
    }

    private void ResizeScrollView(RectTransform scrollView, bool isCollapsed)
    {
        Vector2 normalScrollView = new Vector2(scrollView.offsetMin.x, scrollView.offsetMin.y);

        float offsetFromBottomOfMenu = 40f;
        Vector2 expandedScrollView = new Vector2(scrollView.offsetMin.x, offsetFromBottomOfMenu);

        scrollView.SetParent(isCollapsed ? expandedBackground : normalBackground);
        scrollView.offsetMin = isCollapsed ? expandedScrollView : normalScrollView;
    }
}


//int toggleHalfSizePosition = 426;
//int toggleFullSizePosition = 29;

//Vector2 halfSizeViewport = new Vector2(0, 0);
//Vector2 fullSizeViewport = new Vector2(0, -401);

//Vector3 toggleRotation = new Vector3(0, 0, isCollapsed ? 0 : 180);
//Vector3 togglePosition = new Vector3(0, isCollapsed ? toggleHalfSizePosition : toggleFullSizePosition, 0);

//Vector2 backgroundPosition = new Vector2(215, isCollapsed ? 198.5f : 0);
//Vector2 backgroundSize = new Vector2(430, isCollapsed ? 491 : 888);

//normalBackground.anchoredPosition = backgroundPosition;
//normalBackground.sizeDelta = backgroundSize;

//expandToggle.eulerAngles = toggleRotation;
//expandToggle.anchoredPosition = togglePosition;

//scrollView.offsetMin = isCollapsed ? halfSizeViewport : fullSizeViewport;
//camerasViewport.offsetMin = isCollapsed ? halfSizeViewport : fullSizeViewport;
//labelsViewport.offsetMin = isCollapsed ? halfSizeViewport : fullSizeViewport;
