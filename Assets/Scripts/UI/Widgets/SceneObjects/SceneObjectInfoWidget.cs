using Gameplay;
using Services.SceneObjectSelection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Widgets.SceneObjects
{
	public class SceneObjectInfoWidget : MonoBehaviour
	{
		[SerializeField] private TMP_Text numberText;
		[SerializeField] private Image selectionImage;
		
		protected SceneObject SceneObject;
		
		private ISceneObjectSelectionService _sceneObjectSelectionService;

		[Inject]
		private void Construct(ISceneObjectSelectionService sceneObjectSelectionService)
		{
			_sceneObjectSelectionService = sceneObjectSelectionService;
		}
		
		protected virtual void OnEnable()
		{
			_sceneObjectSelectionService.OnObjectSelected += HandleObjectSelected;
			_sceneObjectSelectionService.OnObjectDeselected += HandleObjectDeselected;
			
			RefreshSelectionImage();
		}
		
		protected virtual void OnDisable()
		{
			_sceneObjectSelectionService.OnObjectSelected -= HandleObjectSelected;
			_sceneObjectSelectionService.OnObjectDeselected -= HandleObjectDeselected;
		}
		
		public virtual void Setup(SceneObject sceneObject, int number)
		{
			SceneObject = sceneObject;
			
			gameObject.name = string.Empty;
			gameObject.name = $"{sceneObject.gameObject.name}{nameof(SceneObjectInfoWidget)}";
			
			numberText.text = number.ToString();
			
			RefreshSelectionImage();
		}

		public void SetNumber(int number)
		{
			numberText.text = number.ToString();
		}
		
		protected virtual void HandleObjectSelected(SceneObject sceneObject)
		{
			selectionImage.color = sceneObject == SceneObject
				? Constants.InfoWidgetSelectedColor 
				: Constants.InfoWidgetUnselectedColor;
		}

		protected virtual void HandleObjectDeselected()
		{
			selectionImage.color = Constants.InfoWidgetUnselectedColor;
		}
		
		public string GetSceneObjectId()
		{
			return SceneObject.Id;
		}

		private void RefreshSelectionImage()
		{
			selectionImage.color = _sceneObjectSelectionService.SelectedObject == SceneObject
				? Constants.InfoWidgetSelectedColor
				: Constants.InfoWidgetUnselectedColor;
		}
	}
}