using Services.Painting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Widgets.Painting
{
	public class ColorPaintingWidget : MonoBehaviour
	{
		[SerializeField] private GameObject colorsButtonsParent;
		[SerializeField] private Slider colorTintSlider;
		[SerializeField] private Button resetChangesButton;

		private Color[] _colors;
		private Button[] _colorsButtons;
		
		private IModelPaintingService _modelPaintingService;

		[Inject]
		private void Construct(IModelPaintingService modelPaintingService)
		{
			_modelPaintingService = modelPaintingService;
		}
		
		private void Awake()
		{
			_colors = Constants.PaintingColors;
			_colorsButtons = colorsButtonsParent.GetComponentsInChildren<Button>();
		}
		
		private void OnEnable()
		{
			colorTintSlider.onValueChanged.AddListener(HandleColorTintSliderValueChanged);
			resetChangesButton.onClick.AddListener(HandleResetChangesButtonClicked);
		}

		private void OnDisable()
		{
			colorTintSlider.onValueChanged.RemoveListener(HandleColorTintSliderValueChanged);
			resetChangesButton.onClick.RemoveListener(HandleResetChangesButtonClicked);
		}

		private void HandleColorTintSliderValueChanged(float value)
		{
			_modelPaintingService.SetColorTint(value);
		}

		private void HandleResetChangesButtonClicked()
		{
			_modelPaintingService.RestoreOriginalMaterial();
			colorTintSlider.value = 0;
		}
	}
}