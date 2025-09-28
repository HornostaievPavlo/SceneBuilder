using Enums;
using Gameplay;
using Services.SceneObjectSelection;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CameraModesWidget : MonoBehaviour
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

    private CameraModeTypeId currentMode;
    
    private ISceneObjectSelectionService _sceneObjectSelectionService;

    [Inject]
    private void Construct(ISceneObjectSelectionService sceneObjectSelectionService)
    {
        _sceneObjectSelectionService = sceneObjectSelectionService;
    }

    private void Start()
    {
        mainCamera = Camera.main;

        modesButtons = this.GetComponentsInChildren<Button>(true);

        currentMode = CameraModeTypeId.MainView;
    }

    private void OnEnable()
    {
        _sceneObjectSelectionService.OnObjectSelected += OnObjectSelected;
        _sceneObjectSelectionService.OnObjectDeselected += OnObjectDeselected;
    }

    private void OnDisable()
    {
        _sceneObjectSelectionService.OnObjectSelected -= OnObjectSelected;
        _sceneObjectSelectionService.OnObjectDeselected -= OnObjectDeselected;
    }

    private void OnObjectSelected(SceneObject scene)
    {
        if (scene.TypeId != SceneObjectTypeId.Camera)
            return;

        ShowButtons(true);

        selectableCamera = scene.GetComponentInChildren<Camera>();

        UpdateModesButtonsEvents(selectableCamera);

        SetCameraMode(currentMode, selectableCamera);
    }

    private void OnObjectDeselected()
    {
        ShowButtons(false);

        if (currentMode == CameraModeTypeId.CameraView)
        {
            SetCameraMode(CameraModeTypeId.MainView, selectableCamera);
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
        modesButtons[0].onClick.AddListener(() => SetCameraMode(CameraModeTypeId.MainView, selectableCamera));
        modesButtons[1].onClick.AddListener(() => SetCameraMode(CameraModeTypeId.SplitScreenView, selectableCamera));
        modesButtons[2].onClick.AddListener(() => SetCameraMode(CameraModeTypeId.CameraView, selectableCamera));
    }

    /// <summary>
    /// Changes cameras properties according to mode
    /// </summary>
    /// <param name="mode">Defines changes to be made</param>
    /// <param name="selectableCamera">Camera to operate with</param>
    private void SetCameraMode(CameraModeTypeId mode, Camera selectableCamera)
    {
        currentMode = mode;

        switch (mode)
        {
            case CameraModeTypeId.MainView:

                mainViewImage.sprite = mainViewImageSelected;

                selectableCamera.depth = zeroDepth;

                var full = new Rect(0, 0, 1, 1);
                mainCamera.rect = full;
                mainCamera.depth = highestDepth;

                splitScreenViewImage.sprite = splitScreenImageDeselected;
                cameraViewImage.sprite = cameraViewImageDeselected;
                break;

            case CameraModeTypeId.SplitScreenView:

                splitScreenViewImage.sprite = splitScreenImageSelected;

                var mainHalf = new Rect(0, 0, 0.5f, 1);
                mainCamera.rect = mainHalf;

                var splitHalf = new Rect(0.5f, 0, 0.5f, 1);
                selectableCamera.rect = splitHalf;
                selectableCamera.depth = lowestDepth;

                mainViewImage.sprite = mainViewImageDeselected;
                cameraViewImage.sprite = cameraViewImageDeselected;
                break;

            case CameraModeTypeId.CameraView:

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