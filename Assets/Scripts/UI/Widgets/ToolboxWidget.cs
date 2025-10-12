using Enums;
using Gameplay;
using RuntimeHandle;
using Services.SceneObjectCopying;
using Services.SceneObjectSelection;
using Services.SceneObjectsRegistry;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Widgets
{
	public class ToolboxWidget : MonoBehaviour
	{
		[Header("Holders")]
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
		}

		private void HandleObjectDeselected()
		{
			buttonsHolder.SetActive(false);
		}

		private void HandleMoveButtonClicked()
		{
			transformHandleWrapper.SetType(HandleType.POSITION);
		}

		private void HandleRotateButtonClicked()
		{
			transformHandleWrapper.SetType(HandleType.ROTATION);
		}

		private void HandleScaleButtonClicked()
		{
			transformHandleWrapper.SetType(HandleType.SCALE);
		}

		private void HandlePaintButtonClicked()
		{
			paintingWidget.gameObject.SetActive(true);
			buttonsHolder.SetActive(false);
		}

		private void HandleCopyButtonClicked()
		{
			SceneObject selectedObject = _sceneObjectSelectionService.SelectedObject;
			_sceneObjectCopyService.CreateCopy(selectedObject);
		}

		private void HandleDeleteButtonClicked()
		{
			_sceneObjectsRegistry.DeleteObject(_sceneObjectSelectionService.SelectedObject);
			buttonsHolder.SetActive(false);
		}

		private void HandleFocusButtonClicked()
		{
			orbitCamera.FocusOnSelectedObject();
		}
	}
}