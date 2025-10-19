using DG.Tweening;
using Services.Painting;
using Services.SceneObjectSelection;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Widgets.Painting
{
	public class PaintingWidget : MonoBehaviour
	{
		[SerializeField] private Button colorPaintingButton;
		[SerializeField] private Button texturePaintingButton;
		[SerializeField] private Button resetChangesButton;
		[SerializeField] private Slider colorTintSlider;
		
		[SerializeField] private ColorPaintingWidget colorPaintingWidget;
		[SerializeField] private TexturePaintingWidget texturePaintingWidget;

		[SerializeField] private CanvasGroup canvasGroup;
 		
		private IModelPaintingService _modelPaintingService;
		private ISceneObjectSelectionService _sceneObjectSelectionService;

		[Inject]
		private void Construct(IModelPaintingService modelPaintingService, ISceneObjectSelectionService sceneObjectSelectionService)
		{
			_modelPaintingService = modelPaintingService;
			_sceneObjectSelectionService = sceneObjectSelectionService;
		}

		private void OnEnable()
		{
			_sceneObjectSelectionService.OnObjectDeselected += HandleObjectDeselected;
			
			colorPaintingButton.onClick.AddListener(HandleColorPaintingButtonClicked);
			texturePaintingButton.onClick.AddListener(HandleTexturePaintingButtonClicked);
			resetChangesButton.onClick.AddListener(HandleResetChangesButtonClicked);
			
			AnimateAppear(isActive: true);
		}
		
		private void OnDisable()
		{
			_sceneObjectSelectionService.OnObjectDeselected -= HandleObjectDeselected;
			
			colorPaintingButton.onClick.RemoveListener(HandleColorPaintingButtonClicked);
			texturePaintingButton.onClick.RemoveListener(HandleTexturePaintingButtonClicked);
			resetChangesButton.onClick.RemoveListener(HandleResetChangesButtonClicked);
			
			AnimateAppear(isActive: false);
		}

		private void HandleObjectDeselected()
		{
			gameObject.SetActive(false);
		}

		private void HandleColorPaintingButtonClicked()
		{
			colorPaintingWidget.gameObject.SetActive(true);
			texturePaintingWidget.gameObject.SetActive(false);
		}

		private void HandleTexturePaintingButtonClicked()
		{
			texturePaintingWidget.gameObject.SetActive(true);
			colorPaintingWidget.gameObject.SetActive(false);
		}

		private void HandleResetChangesButtonClicked()
		{
			_modelPaintingService.RestoreOriginalMaterial();
			colorTintSlider.value = 0;
		}

		private void AnimateAppear(bool isActive)
		{
			canvasGroup.alpha = isActive ? 0f : 1f;
			float endValue = isActive ? 1f : 0f;
			
			canvasGroup.DOKill(true);
			DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, endValue, 0.25f);
		}
	}
}