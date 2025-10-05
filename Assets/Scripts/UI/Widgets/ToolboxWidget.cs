using Gameplay;
using RuntimeHandle;
using Services.SceneObjectSelection;
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
		
		private ISceneObjectSelectionService _sceneObjectSelectionService;

		[Inject]
		private void Construct(ISceneObjectSelectionService sceneObjectSelectionService)
		{
			_sceneObjectSelectionService = sceneObjectSelectionService;
		}
		
		private void OnEnable()
		{
			_sceneObjectSelectionService.OnObjectSelected += HandleObjectSelected;
			_sceneObjectSelectionService.OnObjectDeselected += HandleObjectDeselected;
			
			moveButton.onClick.AddListener(HandleMoveButtonClicked);
			rotateButton.onClick.AddListener(HandleRotateButtonClicked);
			scaleButton.onClick.AddListener(HandleScaleButtonClicked);
		}
		
		private void OnDisable()
		{
			_sceneObjectSelectionService.OnObjectSelected -= HandleObjectSelected;
			_sceneObjectSelectionService.OnObjectDeselected -= HandleObjectDeselected;
			
			moveButton.onClick.RemoveListener(HandleMoveButtonClicked);
			rotateButton.onClick.RemoveListener(HandleRotateButtonClicked);
			scaleButton.onClick.RemoveListener(HandleScaleButtonClicked);
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
	}
}