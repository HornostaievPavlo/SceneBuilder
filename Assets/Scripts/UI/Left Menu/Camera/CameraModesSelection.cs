using UnityEngine;
using UnityEngine.UI;

public class CameraModesSelection : MonoBehaviour
{
    [SerializeField] private RaycastItemSelection RaycastItemSelection;

    [SerializeField] private Image mainViewImage;
    [SerializeField] private Sprite mainViewImageSelected;
    [SerializeField] private Sprite mainViewImageDeselected;

    [SerializeField] private Image splitScreenViewImage;
    [SerializeField] private Sprite splitScreenImageSelected;
    [SerializeField] private Sprite splitScreenImageDeselected;

    [SerializeField] private Image cameraViewImage;
    [SerializeField] private Sprite cameraViewImageSelected;
    [SerializeField] private Sprite cameraViewImageDeselected;

    private Camera mainCamera;

    public Camera ballCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        MainView(true);
    }

    private void Update()
    {
        CheckForBallCamera();
    }

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

    private void ShowToggles(bool isCameraNotNull)
    {
        splitScreenViewImage.gameObject.SetActive(isCameraNotNull);
        cameraViewImage.gameObject.SetActive(isCameraNotNull);
    }

    public void MainView(bool isMainView)
    {
        if (!isMainView) mainViewImage.sprite = mainViewImageDeselected;
        else
        {
            mainViewImage.sprite = mainViewImageSelected;

            mainCamera.rect = new Rect(0, 0, 1, 1); // fullscreen
            mainCamera.depth = 3; // always on top

            if (ballCamera != null)
            {
                ballCamera.depth = 0; // hide ball camera
            }
        }
    }

    public void SplitScreenView(bool isSplitScreenView)
    {
        if (ballCamera != null)
        {
            if (!isSplitScreenView) splitScreenViewImage.sprite = splitScreenImageDeselected;
            else
            {
                splitScreenViewImage.sprite = splitScreenImageSelected;

                ballCamera.rect = new Rect(0.5f, 0, 0.5f, 1); // ball half
                ballCamera.depth = 1; // ball on top

                mainCamera.rect = new Rect(0, 0, 0.5f, 1); // main half
            }
        }
    }

    public void CameraView(bool isCameraView)
    {
        if (ballCamera != null)
        {
            if (!isCameraView) cameraViewImage.sprite = cameraViewImageDeselected;
            else
            {
                cameraViewImage.sprite = cameraViewImageSelected;

                ballCamera.rect = new Rect(0, 0, 1, 1); // fullscreen
                ballCamera.depth = 2; // on top

                mainCamera.depth = 0; // hide main camera
            }
        }
    }
}