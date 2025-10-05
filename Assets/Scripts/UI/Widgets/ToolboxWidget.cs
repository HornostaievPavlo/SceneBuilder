using Gameplay;
using RuntimeHandle;
using Services.Loading;
using Services.SceneObjectSelection;
using Services.SceneObjectsRegistry;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Widgets
{
	public class ToolboxWidget : MonoBehaviour
	{
		[Header("Holder")]
		[SerializeField] private GameObject buttonsHolder;
		
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
		
		private ISceneObjectSelectionService _sceneObjectSelectionService;
		private ISceneObjectsRegistry _sceneObjectsRegistry;

		[Inject]
		private void Construct(
			ISceneObjectSelectionService sceneObjectSelectionService,
			ISceneObjectsRegistry sceneObjectsRegistry)
		{
			_sceneObjectSelectionService = sceneObjectSelectionService;
			_sceneObjectsRegistry = sceneObjectsRegistry;
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
		}

		private void HandleObjectSelected(SceneObject sceneObject)
		{
			buttonsHolder.SetActive(true);
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
			
			var copyInstantiator = selectedObject.GetComponent<SceneObjectCopyInstantiator>();
			SceneObject copy = copyInstantiator.CreateCopy(selectedObject);
			copy.GenerateGuid();
			
			_sceneObjectsRegistry.Register(copy);
		}
	}
}