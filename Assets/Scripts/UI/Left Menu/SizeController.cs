using UnityEngine;
using UnityEngine.UI;

public class SizeController : MonoBehaviour
{
    [SerializeField] private RectTransform modelsViewport;

    [SerializeField] private RectTransform camerasViewport;

    [SerializeField] private RectTransform labelsViewport;

    [SerializeField] private RectTransform openToggle;

    [SerializeField] private RectTransform sizeToggle;

    private RectTransform mainBackground;

    private GameObject toggleGroup;

    private GameObject leftMenu;

    private void Awake()
    {
        mainBackground = GetComponentInChildren<Image>(true).rectTransform;

        toggleGroup = GetComponentInChildren<ToggleGroup>().gameObject;

        leftMenu = mainBackground.gameObject;
    }

    public void OpenLeftMenu(bool isOpened)
    {
        Vector3 toggleRotation = new Vector3(0, 0, isOpened ? 0 : 180);
        openToggle.eulerAngles = toggleRotation;

        leftMenu.SetActive(!isOpened);
        sizeToggle.gameObject.SetActive(!isOpened);

        Toggle[] toggles = toggleGroup.GetComponentsInChildren<Toggle>();

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

    public void ChangeLeftMenuSize(bool isHalfSize) // changing of menu size for small collapse toggle 
    {
        const int toggleHalfSizePosition = 426;
        const int toggleFullSizePosition = 29;

        Vector2 halfSizeViewport = new Vector2(0, 0);
        Vector2 fullSizeViewport = new Vector2(0, -401);

        Vector3 toggleRotation = new Vector3(0, 0, isHalfSize ? 0 : 180);
        Vector3 togglePosition = new Vector3(0, isHalfSize ? toggleHalfSizePosition : toggleFullSizePosition, 0);

        Vector2 backgroundPosition = new Vector2(215, isHalfSize ? 198.5f : 0);
        Vector2 backgroundSize = new Vector2(430, isHalfSize ? 491 : 888);

        mainBackground.anchoredPosition = backgroundPosition;
        mainBackground.sizeDelta = backgroundSize;

        sizeToggle.eulerAngles = toggleRotation;
        sizeToggle.anchoredPosition = togglePosition;

        modelsViewport.offsetMin = isHalfSize ? halfSizeViewport : fullSizeViewport;
        camerasViewport.offsetMin = isHalfSize ? halfSizeViewport : fullSizeViewport;
        labelsViewport.offsetMin = isHalfSize ? halfSizeViewport : fullSizeViewport;
    }
}
