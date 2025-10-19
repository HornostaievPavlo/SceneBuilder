using DG.Tweening;
using Enums;
using Gameplay;
using RuntimeHandle;
using Services.SceneObjectCopying;
using Services.SceneObjectSelection;
using Services.SceneObjectsRegistry;
using UI.Widgets.Painting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Widgets
{
	public class ToolboxWidget : MonoBehaviour
	{
		[Header("Holders")]
		[SerializeField] private CanvasGroup buttonsCanvasGroup;
		[SerializeField] private GameObject buttonsHolder;
		[SerializeField] private GameObject modelOnlyButtonsHolder;
		
		[Header("Transform")]
		[SerializeField] private TransformHandleWrapper transformHandleWrapper;
		[SerializeField] private Button moveButton;
		[SerializeField] private Button rotateButton;
		[SerializeField] private Button scaleButton;
		
		[Header("Painting")]
		[SerializeField] private Button paintButton;
		[SerializeField] private PaintingWidget paintingWidget;
		
		[Header("Loading")]
		[SerializeField] private Button copyButton;
		[SerializeField] private Button deleteButton;
		
		[Header("Camera")]
		[SerializeField] private Button focusButton;
		[SerializeField] private OrbitCamera orbitCamera;
		
		private Vector3 _initialButtonsHolderPosition;
		
		private ISceneObjectSelectionService _sceneObjectSelectionService;
		private ISceneObjectsRegistry _sceneObjectsRegistry;
		private ISceneObjectCopyService _sceneObjectCopyService;

		[Inject]
		private void Construct(
			ISceneObjectSelectionService sceneObjectSelectionService,
			ISceneObjectsRegistry sceneObjectsRegistry,
			ISceneObjectCopyService sceneObjectCopyService)
		{
			_sceneObjectSelectionService = sceneObjectSelectionService;
			_sceneObjectsRegistry = sceneObjectsRegistry;
			_sceneObjectCopyService = sceneObjectCopyService;
		}

		private void Awake()
		{
			_initialButtonsHolderPosition = buttonsHolder.transform.localPosition;
		}

		private void OnEnable()
		{
			_sceneObjectSelectionService.OnObjectSelected += HandleObjectSelected;
			_sceneObjectSelectionService.OnObjectDeselected += HandleObjectDeselected;
			
			moveButton.onClick.AddListener(HandleMoveButtonClicked);
			rotateButton.onClick.AddListener(HandleRotateButtonClicked);
			scaleButton.onClick.AddListener(HandleScaleButtonClicked);
			paintButton.onClick.AddListener(HandlePaintButtonClicked);
			copyButton.onClick.AddListener(HandleCopyButtonClicked);
			deleteButton.onClick.AddListener(HandleDeleteButtonClicked);
			focusButton.onClick.AddListener(HandleFocusButtonClicked);
		}
		
		private void OnDisable()
		{
			_sceneObjectSelectionService.OnObjectSelected -= HandleObjectSelected;
			_sceneObjectSelectionService.OnObjectDeselected -= HandleObjectDeselected;
			
			moveButton.onClick.RemoveListener(HandleMoveButtonClicked);
			rotateButton.onClick.RemoveListener(HandleRotateButtonClicked);
			scaleButton.onClick.RemoveListener(HandleScaleButtonClicked);
			paintButton.onClick.RemoveListener(HandlePaintButtonClicked);
			copyButton.onClick.RemoveListener(HandleCopyButtonClicked);
			deleteButton.onClick.RemoveListener(HandleDeleteButtonClicked);
			focusButton.onClick.RemoveListener(HandleFocusButtonClicked);
		}

		private void HandleObjectSelected(SceneObject sceneObject)
		{
			buttonsHolder.SetActive(true);
			modelOnlyButtonsHolder.SetActive(sceneObject.TypeId == SceneObjectTypeId.Model);
			
			AnimateButtonsAppear();
		}

		private void HandleObjectDeselected()
		{
			AnimateButtonsDisappear();
		}

		private void HandleMoveButtonClicked()
		{
			AnimateButtonClick(moveButton.transform);
			transformHandleWrapper.SetType(HandleType.POSITION);
		}

		private void HandleRotateButtonClicked()
		{
			AnimateButtonClick(rotateButton.transform);
			transformHandleWrapper.SetType(HandleType.ROTATION);
		}

		private void HandleScaleButtonClicked()
		{
			AnimateButtonClick(scaleButton.transform);
			transformHandleWrapper.SetType(HandleType.SCALE);
		}

		private void HandlePaintButtonClicked()
		{
			AnimateButtonClick(paintButton.transform);
			
			paintingWidget.gameObject.SetActive(true);
			buttonsHolder.SetActive(false);
		}

		private void HandleCopyButtonClicked()
		{
			AnimateButtonClick(copyButton.transform);
			
			SceneObject selectedObject = _sceneObjectSelectionService.SelectedObject;
			_sceneObjectCopyService.CreateCopy(selectedObject);
		}

		private void HandleDeleteButtonClicked()
		{
			AnimateButtonClick(deleteButton.transform);
			
			_sceneObjectsRegistry.DeleteObject(_sceneObjectSelectionService.SelectedObject);
			buttonsHolder.SetActive(false);
		}

		private void HandleFocusButtonClicked()
		{
			AnimateButtonClick(focusButton.transform);
			orbitCamera.FocusOnSelectedObject();
		}

		private void AnimateButtonsAppear()
		{
			float appearDuration = 0.25f;
			buttonsHolder.transform.localPosition = GetOffscreenPosition();
			
			buttonsHolder.transform.DOKill(true);
			buttonsHolder.transform.DOLocalMoveX(_initialButtonsHolderPosition.x, appearDuration).SetEase(Ease.OutBack);

			buttonsCanvasGroup.alpha = 0f;
			buttonsCanvasGroup.DOKill(true);
			DOTween.To(() => buttonsCanvasGroup.alpha, x => buttonsCanvasGroup.alpha = x, 1f, appearDuration);
		}

		private void AnimateButtonsDisappear()
		{
			float disappearDuration = 0.15f;

			buttonsHolder.transform.DOKill(true);
			buttonsHolder.transform.DOLocalMoveX(GetOffscreenPosition().x, disappearDuration)
				.SetEase(Ease.InBack)
				.OnComplete(() =>
				{
					buttonsHolder.SetActive(false);
					buttonsHolder.transform.localPosition = _initialButtonsHolderPosition;
				});
		}
		
		private Vector3 GetOffscreenPosition()
		{
			return new Vector3(
				_initialButtonsHolderPosition.x + 200f,
				_initialButtonsHolderPosition.y,
				_initialButtonsHolderPosition.z);
		}
		
		private void AnimateButtonClick(Transform buttonTransform)
		{
			buttonTransform.localScale = Vector3.one;
			buttonTransform.DOKill(true);
			buttonTransform.DOPunchScale(Vector3.one * 0.25f, 0.15f);
		}
	}
}