using UnityEngine;
using UnityEngine.UI;

public class CameraModesSwitcher : MonoBehaviour
{
    [SerializeField] private Image mainViewImage;
    [SerializeField] private Sprite mainViewImageSelected;
    [SerializeField] private Sprite mainViewImageDeselected;
    [Space]
    [SerializeField] private Image splitScreenViewImage;
    [SerializeField] private Sprite splitScreenImageSelected;
    [SerializeField] private Sprite splitScreenImageDeselected;
    [Space]
    [SerializeField] private Image cameraViewImage;
    [SerializeField] private Sprite cameraViewImageSelected;
    [SerializeField] private Sprite cameraViewImageDeselected;

    private Camera ballCamera;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        SetMainView(true);
    }

    private void OnEnable()
    {
        SelectionSystem.OnObjectSelected += OnObjectSelected;
        SelectionSystem.OnObjectDeselected += OnObjectDeselected;
    }

    private void OnDisable()
    {
        SelectionSystem.OnObjectSelected -= OnObjectSelected;
        SelectionSystem.OnObjectDeselected -= OnObjectDeselected;
    }

    private void OnObjectSelected(SelectableObject selectable)
    {
        if (selectable.type == AssetType.Camera)
        {
            ballCamera = selectable.GetComponentInChildren<Camera>();
            ShowToggles(true);
        }
        else ShowToggles(false);
    }

    private void OnObjectDeselected()
    {
        ShowToggles(false);
    }

    private void ShowToggles(bool isCameraSelected)
    {
        mainViewImage.gameObject.SetActive(isCameraSelected);
        splitScreenViewImage.gameObject.SetActive(isCameraSelected);
        cameraViewImage.gameObject.SetActive(isCameraSelected);
    }

    /// <summary>
    /// Enables fullscreen main camera view,
    /// hides ball camera view 
    /// </summary>
    /// <param name="isMainView">Changes sprite of toggle</param>
    public void SetMainView(bool isMainView)
    {
        if (!isMainView)
            mainViewImage.sprite = mainViewImageDeselected;
        else
        {
            mainViewImage.sprite = mainViewImageSelected;

            mainCamera.rect = new Rect(0, 0, 1, 1);
            mainCamera.depth = 3;

            if (ballCamera != null)
            {
                ballCamera.depth = 0;
            }
        }
    }

    /// <summary>
    /// Sets both main and ball cameras to render on half of the screen
    /// </summary>
    /// <param name="isSplitScreenView">Changes sprite of toggle</param>
    public void SetSplitScreenView(bool isSplitScreenView)
    {
        if (ballCamera != null)
        {
            if (!isSplitScreenView)
                splitScreenViewImage.sprite = splitScreenImageDeselected;
            else
            {
                splitScreenViewImage.sprite = splitScreenImageSelected;

                ballCamera.rect = new Rect(0.5f, 0, 0.5f, 1);
                ballCamera.depth = 1;

                mainCamera.rect = new Rect(0, 0, 0.5f, 1);
            }
        }
    }

    /// <summary>
    /// Enables fullscreen ball camera view,
    /// hides main camera view
    /// </summary>
    /// <param name="isCameraView">Changes sprite of toggle</param>
    public void SetCameraView(bool isCameraView)
    {
        if (ballCamera != null)
        {
            if (!isCameraView)
                cameraViewImage.sprite = cameraViewImageDeselected;
            else
            {
                cameraViewImage.sprite = cameraViewImageSelected;

                ballCamera.rect = new Rect(0, 0, 1, 1);
                ballCamera.depth = 2;

                mainCamera.depth = 0;
            }
        }
    }
}