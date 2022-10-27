using UnityEngine;
using UnityEngine.UI;

public class CameraModesSelection : MonoBehaviour
{
    [SerializeField] private RaycastItemSelection _raycastItemSelection;

    [SerializeField] private Image mainViewImage;
    [SerializeField] private Sprite mainViewImageSelected;
    [SerializeField] private Sprite mainViewImageDeselected;

    [SerializeField] private Image splitScreenViewImage;
    [SerializeField] private Sprite splitScreenImageSelected;
    [SerializeField] private Sprite splitScreenImageDeselected;

    [SerializeField] private Image cameraViewImage;
    [SerializeField] private Sprite cameraViewImageSelected;
    [SerializeField] private Sprite cameraViewImageDeselected;

    [HideInInspector] public Camera ballCamera;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;

        MainView(true);
    }

    private void Update()
    {
        CheckForBallCamera();
    }

    /// <summary>
    /// Sets modes toggles active 
    /// </summary>
    /// <param name="isCameraNotNull">True if camera selected and false if not</param>
    private void ShowToggles(bool isCameraNotNull)
    {
        splitScreenViewImage.gameObject.SetActive(isCameraNotNull);
        cameraViewImage.gameObject.SetActive(isCameraNotNull);
    }

    /// <summary>
    /// Showing modes toggles if camera object is selected
    /// </summary>
    private void CheckForBallCamera()
    {
        if (ballCamera != null)
        {
            ShowToggles(true);
        }
        else
        {
            ShowToggles(false);
        }
    }

    /// <summary>
    /// Enables fullscreen main camera view,
    /// hides ball camera view 
    /// </summary>
    /// <param name="isMainView">Changes sprite of toggle</param>
    public void MainView(bool isMainView)
    {
        if (!isMainView)
            mainViewImage.sprite = mainViewImageDeselected;
        else
        {
            mainViewImage.sprite = mainViewImageSelected;

            _mainCamera.rect = new Rect(0, 0, 1, 1);
            _mainCamera.depth = 3;

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
    public void SplitScreenView(bool isSplitScreenView)
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

                _mainCamera.rect = new Rect(0, 0, 0.5f, 1);
            }
        }
    }

    /// <summary>
    /// Enables fullscreen ball camera view,
    /// hides main camera view
    /// </summary>
    /// <param name="isCameraView">Changes sprite of toggle</param>
    public void CameraView(bool isCameraView)
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

                _mainCamera.depth = 0;
            }
        }
    }
}