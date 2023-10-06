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

    private Camera mainCamera;
    private Camera selectableCamera;

    private Button[] modesButtons;

    private const int zeroDepth = 0;
    private const int lowestDepth = 1;
    private const int middleDepth = 2;
    private const int highestDepth = 3;

    private enum CameraMode
    {
        mainView,
        splitScreenView,
        cameraView
    }

    private CameraMode currentMode;

    private void Start()
    {
        mainCamera = Camera.main;

        modesButtons = this.GetComponentsInChildren<Button>(true);

        currentMode = CameraMode.mainView;
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
        if (selectable.type != AssetType.Camera)
            return;

        ShowButtons(true);

        selectableCamera = selectable.GetComponentInChildren<Camera>();

        UpdateModesButtonsEvents(selectableCamera);

        SetCameraMode(currentMode, selectableCamera);
    }

    private void OnObjectDeselected()
    {
        ShowButtons(false);

        if (currentMode == CameraMode.cameraView)
        {
            SetCameraMode(CameraMode.mainView, selectableCamera);
        }
    }

    private void ShowButtons(bool isCameraSelected)
    {
        foreach (var toggle in modesButtons)
        {
            toggle.gameObject.SetActive(isCameraSelected);
        }
    }

    /// <summary>
    /// Reassigns modes according to camera
    /// </summary>
    /// <param name="selectableCamera">Currently selected Camera</param>
    private void UpdateModesButtonsEvents(Camera selectableCamera)
    {
        modesButtons[0].onClick.AddListener(() => SetCameraMode(CameraMode.mainView, selectableCamera));
        modesButtons[1].onClick.AddListener(() => SetCameraMode(CameraMode.splitScreenView, selectableCamera));
        modesButtons[2].onClick.AddListener(() => SetCameraMode(CameraMode.cameraView, selectableCamera));
    }

    /// <summary>
    /// Changes cameras properties according to mode
    /// </summary>
    /// <param name="mode">Defines changes to be made</param>
    /// <param name="selectableCamera">Camera to operate with</param>
    private void SetCameraMode(CameraMode mode, Camera selectableCamera)
    {
        currentMode = mode;

        switch (mode)
        {
            case CameraMode.mainView:

                mainViewImage.sprite = mainViewImageSelected;

                selectableCamera.depth = zeroDepth;

                var full = new Rect(0, 0, 1, 1);
                mainCamera.rect = full;
                mainCamera.depth = highestDepth;

                splitScreenViewImage.sprite = splitScreenImageDeselected;
                cameraViewImage.sprite = cameraViewImageDeselected;
                break;

            case CameraMode.splitScreenView:

                splitScreenViewImage.sprite = splitScreenImageSelected;

                var mainHalf = new Rect(0, 0, 0.5f, 1);
                mainCamera.rect = mainHalf;

                var splitHalf = new Rect(0.5f, 0, 0.5f, 1);
                selectableCamera.rect = splitHalf;
                selectableCamera.depth = lowestDepth;

                mainViewImage.sprite = mainViewImageDeselected;
                cameraViewImage.sprite = cameraViewImageDeselected;
                break;

            case CameraMode.cameraView:

                cameraViewImage.sprite = cameraViewImageSelected;

                var selectableFull = new Rect(0, 0, 1, 1);
                selectableCamera.rect = selectableFull;
                selectableCamera.depth = middleDepth;

                mainCamera.depth = zeroDepth;

                mainViewImage.sprite = mainViewImageDeselected;
                splitScreenViewImage.sprite = splitScreenImageDeselected;
                break;

            default: break;
        }
    }
}