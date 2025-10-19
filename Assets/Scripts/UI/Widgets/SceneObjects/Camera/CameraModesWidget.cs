using Enums;
using Gameplay;
using Services.SceneObjectSelection;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

namespace UI.Widgets.SceneObjects.Camera
{
	public class CameraModesWidget : MonoBehaviour
	{
		[Header("Buttons")] 
		[SerializeField] private Button mainViewButton;
		[SerializeField] private Button splitScreenViewButton;
		[SerializeField] private Button cameraViewButton;

		[Header("Images")] 
		[SerializeField] private Image mainViewImage;
		[SerializeField] private Image splitScreenViewImage;
		[SerializeField] private Image cameraViewImage;

		[Header("Sprites")] [Space]
		[SerializeField] private Sprite mainViewImageSelected;
		[SerializeField] private Sprite mainViewImageDeselected;
		[Space] 
		[SerializeField] private Sprite splitScreenImageSelected;
		[SerializeField] private Sprite splitScreenImageDeselected;
		[Space] 
		[SerializeField] private Sprite cameraViewImageSelected;
		[SerializeField] private Sprite cameraViewImageDeselected;

		private UnityEngine.Camera _mainCamera;
		private UnityEngine.Camera _sceneObjectCamera;

		private CameraModeTypeId _currentModeTypeId;

		private ISceneObjectSelectionService _sceneObjectSelectionService;

		private const float RectTweenDuration = 0.1f;
		
		private Tweener _mainCameraRectTween;
		private Tweener _sceneObjectCameraRectTween;

		[Inject]
		private void Construct(ISceneObjectSelectionService sceneObjectSelectionService)
		{
			_sceneObjectSelectionService = sceneObjectSelectionService;
		}

		private void Awake()
		{
			_mainCamera = UnityEngine.Camera.main;
			_currentModeTypeId = CameraModeTypeId.MainView;
			ToggleButtonsActiveState(false);
		}

		private void OnEnable()
		{
			_sceneObjectSelectionService.OnObjectSelected += HandleObjectSelected;
			_sceneObjectSelectionService.OnObjectDeselected += HandleObjectDeselected;

			mainViewButton.onClick.AddListener(HandleMainViewButtonClicked);
			splitScreenViewButton.onClick.AddListener(HandleSplitScreenViewButtonClicked);
			cameraViewButton.onClick.AddListener(HandleCameraViewButtonClicked);
		}

		private void OnDisable()
		{
			_sceneObjectSelectionService.OnObjectSelected -= HandleObjectSelected;
			_sceneObjectSelectionService.OnObjectDeselected -= HandleObjectDeselected;

			mainViewButton.onClick.RemoveListener(HandleMainViewButtonClicked);
			splitScreenViewButton.onClick.RemoveListener(HandleSplitScreenViewButtonClicked);
			cameraViewButton.onClick.RemoveListener(HandleCameraViewButtonClicked);
		}

		private void HandleObjectSelected(SceneObject sceneObject)
		{
			if (sceneObject.TypeId != SceneObjectTypeId.Camera)
				return;

			ToggleButtonsActiveState(true);

			_sceneObjectCamera = sceneObject.GetComponentInChildren<UnityEngine.Camera>();
			SetCameraMode(_currentModeTypeId);
		}

		private void HandleObjectDeselected()
		{
			ToggleButtonsActiveState(false);

			if (_currentModeTypeId == CameraModeTypeId.CameraView)
			{
				SetCameraMode(CameraModeTypeId.MainView);
			}

			_sceneObjectCamera = null;
		}

		private void ToggleButtonsActiveState(bool value)
		{
			mainViewButton.gameObject.SetActive(value);
			splitScreenViewButton.gameObject.SetActive(value);
			cameraViewButton.gameObject.SetActive(value);
		}

		private void HandleMainViewButtonClicked()
		{
			SetCameraMode(CameraModeTypeId.MainView);
		}

		private void HandleSplitScreenViewButtonClicked()
		{
			SetCameraMode(CameraModeTypeId.SplitScreenView);
		}

		private void HandleCameraViewButtonClicked()
		{
			SetCameraMode(CameraModeTypeId.CameraView);
		}

		private void SetCameraMode(CameraModeTypeId modeTypeId)
		{
			_currentModeTypeId = modeTypeId;

			switch (modeTypeId)
			{
				case CameraModeTypeId.MainView:
					SetMainViewMode();
					break;

				case CameraModeTypeId.SplitScreenView:
					SetSplitScreenMode();
					break;

				case CameraModeTypeId.CameraView:
					SetCameraViewMode();
					break;
			}
		}

		private void SetCameraViewMode()
		{
			AnimateCameraRect(_sceneObjectCamera, new Rect(0, 0, 1, 1));
			_sceneObjectCamera.depth = Constants.MiddleCameraDepth;

			_mainCamera.depth = Constants.ZeroCameraDepth;

			cameraViewImage.sprite = cameraViewImageSelected;
			mainViewImage.sprite = mainViewImageDeselected;
			splitScreenViewImage.sprite = splitScreenImageDeselected;
		}

		private void SetSplitScreenMode()
		{
			AnimateCameraRect(_mainCamera, new Rect(0, 0, 0.5f, 1));
			AnimateCameraRect(_sceneObjectCamera, new Rect(0.5f, 0, 0.5f, 1));

			_sceneObjectCamera.depth = Constants.LowestCameraDepth;

			splitScreenViewImage.sprite = splitScreenImageSelected;
			mainViewImage.sprite = mainViewImageDeselected;
			cameraViewImage.sprite = cameraViewImageDeselected;
		}

		private void SetMainViewMode()
		{
			AnimateCameraRect(_mainCamera, new Rect(0, 0, 1, 1));

			_sceneObjectCamera.depth = Constants.ZeroCameraDepth;
			_mainCamera.depth = Constants.HighestCameraDepth;

			mainViewImage.sprite = mainViewImageSelected;
			splitScreenViewImage.sprite = splitScreenImageDeselected;
			cameraViewImage.sprite = cameraViewImageDeselected;
		}

		private void AnimateCameraRect(UnityEngine.Camera camera, Rect targetRect)
		{
			if (camera == _mainCamera)
			{
				_mainCameraRectTween?.Kill();
				_mainCameraRectTween = camera.DORect(targetRect, RectTweenDuration)
					.SetEase(Ease.Linear)
					.SetUpdate(UpdateType.Late);
			}
			else if (camera == _sceneObjectCamera)
			{
				_sceneObjectCameraRectTween?.Kill();
				_sceneObjectCameraRectTween = camera.DORect(targetRect, RectTweenDuration)
					.SetEase(Ease.Linear)
					.SetUpdate(UpdateType.Late);
			}
		}
	}
}