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
			
			
		}
		
		private void OnDisable()
		{
			_sceneObjectSelectionService.OnObjectSelected -= HandleObjectSelected;
			_sceneObjectSelectionService.OnObjectDeselected -= HandleObjectDeselected;
			
			
		}

		private void HandleObjectSelected(SceneObject sceneObject)
		{
			buttonsHolder.SetActive(true);
		}

		private void HandleObjectDeselected()
		{
			buttonsHolder.SetActive(false);
		}

		
	}
}