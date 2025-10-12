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
		private Button[] _colorButtons;
		
		private IModelPaintingService _modelPaintingService;

		[Inject]
		private void Construct(IModelPaintingService modelPaintingService)
		{
			_modelPaintingService = modelPaintingService;
		}
		
		private void Awake()
		{
			_colors = Constants.PaintingColors;
			SetupColorButtons();
		}
		
		private void OnEnable()
		{
			colorTintSlider.onValueChanged.AddListener(HandleColorTintSliderValueChanged);
			resetChangesButton.onClick.AddListener(HandleResetChangesButtonClicked);
			AddColorButtonsHandlers();
		}

		private void OnDisable()
		{
			colorTintSlider.onValueChanged.RemoveListener(HandleColorTintSliderValueChanged);
			resetChangesButton.onClick.RemoveListener(HandleResetChangesButtonClicked);
			RemoveColorButtonsHandlers();
		}

		private void SetupColorButtons()
		{
			_colorButtons = colorsButtonsParent.GetComponentsInChildren<Button>();
			
			for (int i = 0; i < _colors.Length; i++)
			{
				Image buttonImage = _colorButtons[i].GetComponent<Image>();
				buttonImage.color = _colors[i];
			}
		}

		private void AddColorButtonsHandlers()
		{
			for (int i = 0; i < _colors.Length; i++)
			{
				int colorIndex = i;
				_colorButtons[i].onClick.AddListener(() => HandleColorButtonClicked(colorIndex));
			}
		}

		private void RemoveColorButtonsHandlers()
		{
			for (int i = 0; i < _colorButtons.Length; i++)
			{
				_colorButtons[i].onClick.RemoveAllListeners();
			}
		}

		private void HandleColorButtonClicked(int colorIndex)
		{
			_modelPaintingService.SetColor(_colors[colorIndex]);
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