using UnityEngine;
using UnityEngine.UI;

public class SizeController : MonoBehaviour
{
    [SerializeField] private RectTransform _modelsViewport;

    [SerializeField] private RectTransform _camerasViewport;

    [SerializeField] private RectTransform _labelsViewport;

    [SerializeField] private RectTransform _openToggle;

    [SerializeField] private RectTransform _sizeToggle;

    private RectTransform _mainBackground;

    private GameObject _toggleGroup;

    private GameObject _leftMenu;

    private void Awake()
    {
        _mainBackground = GetComponentInChildren<Image>(true).rectTransform;

        _toggleGroup = GetComponentInChildren<ToggleGroup>().gameObject;

        _leftMenu = _mainBackground.gameObject;
    }

    /// <summary>
    /// Opens left menu according to bool value of toggle
    /// </summary>
    /// <param name="isOpened">Is menu opened or not</param>
    public void OpenLeftMenu(bool isOpened)
    {
        Vector3 toggleRotation = new Vector3(0, 0, isOpened ? 0 : 180);
        _openToggle.eulerAngles = toggleRotation;

        _leftMenu.SetActive(!isOpened);
        _sizeToggle.gameObject.SetActive(!isOpened);

        Toggle[] toggles = _toggleGroup.GetComponentsInChildren<Toggle>();

        foreach (Toggle item in toggles) // while menu is not opened tabs toggles are not interactable
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
    public void ChangeLeftMenuSize(bool isHalfSize) // changing of menu size for small collapse toggle 
    {
        int toggleHalfSizePosition = 426;
        int toggleFullSizePosition = 29;

        Vector2 halfSizeViewport = new Vector2(0, 0);
        Vector2 fullSizeViewport = new Vector2(0, -401);

        Vector3 toggleRotation = new Vector3(0, 0, isHalfSize ? 0 : 180);
        Vector3 togglePosition = new Vector3(0, isHalfSize ? toggleHalfSizePosition : toggleFullSizePosition, 0);

        Vector2 backgroundPosition = new Vector2(215, isHalfSize ? 198.5f : 0);
        Vector2 backgroundSize = new Vector2(430, isHalfSize ? 491 : 888);

        _mainBackground.anchoredPosition = backgroundPosition;
        _mainBackground.sizeDelta = backgroundSize;

        _sizeToggle.eulerAngles = toggleRotation;
        _sizeToggle.anchoredPosition = togglePosition;

        _modelsViewport.offsetMin = isHalfSize ? halfSizeViewport : fullSizeViewport;
        _camerasViewport.offsetMin = isHalfSize ? halfSizeViewport : fullSizeViewport;
        _labelsViewport.offsetMin = isHalfSize ? halfSizeViewport : fullSizeViewport;
    }
}
