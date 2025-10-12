using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets.Painting
{
	public class PaintingWidget : MonoBehaviour
	{
		[SerializeField] private Button colorPaintingButton;
		[SerializeField] private Button texturePaintingButton;
		
		[SerializeField] private ColorPaintingWidget colorPaintingWidget;
		[SerializeField] private TexturePaintingWidget texturePaintingWidget;

		private void OnEnable()
		{
			colorPaintingButton.onClick.AddListener(HandleColorPaintingButtonClicked);
			texturePaintingButton.onClick.AddListener(HandleTexturePaintingButtonClicked);
		}
		
		private void OnDisable()
		{
			colorPaintingButton.onClick.RemoveListener(HandleColorPaintingButtonClicked);
			texturePaintingButton.onClick.RemoveListener(HandleTexturePaintingButtonClicked);
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
	}
}