using System.Collections.Generic;
using Services.Painting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Widgets.Painting
{
	public class TexturePaintingWidget : MonoBehaviour
	{
		[SerializeField] private GameObject texturesButtonsParent;
		
		private readonly List<Texture> _textures = new();
		private Button[] _textureButtons;
		
		private IModelPaintingService _modelPaintingService;

		[Inject]
		private void Construct(IModelPaintingService modelPaintingService)
		{
			_modelPaintingService = modelPaintingService;
		}

		private void Awake()
		{
			_textureButtons = texturesButtonsParent.GetComponentsInChildren<Button>();

			foreach (var button in _textureButtons)
			{
				Image buttonImage = button.GetComponent<Image>();
				Texture texture = buttonImage.mainTexture;
				
				_textures.Add(texture);
			}
		}

		private void OnEnable()
		{
			AddClickHandlers();
		}

		private void OnDisable()
		{
			RemoveClickHandlers();
		}

		private void AddClickHandlers()
		{
			for (int i = 0; i < _textureButtons.Length && i < _textures.Count; i++)
			{
				int textureIndex = i;
				_textureButtons[i].onClick.AddListener(() => HandleTextureButtonClicked(textureIndex));
			}
		}

		private void RemoveClickHandlers()
		{
			foreach (Button button in _textureButtons)
			{
				button.onClick.RemoveAllListeners();
			}
		}

		private void HandleTextureButtonClicked(int textureIndex)
		{
			_modelPaintingService.SetTexture(_textures[textureIndex]);
		}
	}
}