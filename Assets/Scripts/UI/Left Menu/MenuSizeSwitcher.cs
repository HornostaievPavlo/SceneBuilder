using UnityEngine;
using UnityEngine.UI;

public class MenuSizeSwitcher : MonoBehaviour
{
   [Space]
    [Header("Toggles")]
    [SerializeField]
    private RectTransform openToggle;

    [SerializeField]
    private RectTransform expandToggle;

    

    [Space]
    [Header("Viewports")]
    [SerializeField]
    private RectTransform modelsScrollView;

    [SerializeField]
    private RectTransform camerasScrollView;

    [SerializeField]
    private RectTransform labelsScrollView;
       

    private void UpdateExpandToggleTransform(bool isCollapsed)
    {
        // Transform expandToggleParent = isCollapsed ? expandedBackground : normalBackground;
        // expandToggle.transform.SetParent(expandToggleParent);

        float offsetFromBottomOfMenu = 15f;
        expandToggle.anchoredPosition = new Vector3(0, offsetFromBottomOfMenu, 0);
        expandToggle.eulerAngles = new Vector3(0, 0, isCollapsed ? 0 : 180);
    }

    private void ResizeScrollView(RectTransform scrollView, bool isCollapsed)
    {
        Vector2 normalScrollView = new Vector2(scrollView.offsetMin.x, scrollView.offsetMin.y);

        float offsetFromBottomOfMenu = 40f;
        Vector2 expandedScrollView = new Vector2(scrollView.offsetMin.x, offsetFromBottomOfMenu);

        // scrollView.SetParent(isCollapsed ? expandedBackground : normalBackground);
        scrollView.offsetMin = isCollapsed ? expandedScrollView : normalScrollView;
    }
}