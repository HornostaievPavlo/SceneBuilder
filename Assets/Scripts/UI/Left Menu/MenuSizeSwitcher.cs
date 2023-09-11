using UnityEngine;
using UnityEngine.UI;

public class MenuSizeSwitcher : MonoBehaviour
{
    [SerializeField]
    private RectTransform menuBackground;

    [SerializeField]
    private RectTransform upperSizeToggle;

    [SerializeField]
    private RectTransform lowerSizeToggle;

    [SerializeField]
    private GameObject tabsToggleGroup;

    [SerializeField]
    private RectTransform modelsViewport;

    [SerializeField]
    private RectTransform camerasViewport;

    [SerializeField]
    private RectTransform labelsViewport;

    /// <summary>
    /// Opens left menu according to bool value of toggle
    /// </summary>
    /// <param name="isOpened">Is menu opened or not</param>
    public void OpenLeftMenu(bool isOpened)
    {
        Vector3 toggleRotation = new Vector3(0, 0, isOpened ? 0 : 180);
        upperSizeToggle.eulerAngles = toggleRotation;

        menuBackground.gameObject.SetActive(!isOpened);
        lowerSizeToggle.gameObject.SetActive(!isOpened);

        Toggle[] toggles = tabsToggleGroup.GetComponentsInChildren<Toggle>();

        foreach (Toggle item in toggles)
        {
            item.interactable = !isOpened;
        }

        if (isOpened)
        {
            foreach (Toggle item in toggles)
            {
                item.isOn = false;
            }
        }
    }

    /// <summary>
    /// Expands/collapses menu accrding to its current state
    /// </summary>
    /// <param name="isHalfSize">Is menu half or full view</param>
    public void ChangeLeftMenuSize(bool isHalfSize)
    {
        int toggleHalfSizePosition = 426;
        int toggleFullSizePosition = 29;

        Vector2 halfSizeViewport = new Vector2(0, 0);
        Vector2 fullSizeViewport = new Vector2(0, -401);

        Vector3 toggleRotation = new Vector3(0, 0, isHalfSize ? 0 : 180);
        Vector3 togglePosition = new Vector3(0, isHalfSize ? toggleHalfSizePosition : toggleFullSizePosition, 0);

        Vector2 backgroundPosition = new Vector2(215, isHalfSize ? 198.5f : 0);
        Vector2 backgroundSize = new Vector2(430, isHalfSize ? 491 : 888);

        menuBackground.anchoredPosition = backgroundPosition;
        menuBackground.sizeDelta = backgroundSize;

        lowerSizeToggle.eulerAngles = toggleRotation;
        lowerSizeToggle.anchoredPosition = togglePosition;

        modelsViewport.offsetMin = isHalfSize ? halfSizeViewport : fullSizeViewport;
        camerasViewport.offsetMin = isHalfSize ? halfSizeViewport : fullSizeViewport;
        labelsViewport.offsetMin = isHalfSize ? halfSizeViewport : fullSizeViewport;
    }
}
