using Services.Painting;
using UnityEngine;
using Zenject;

namespace UI.Widgets.Painting
{
	public class TexturePaintingWidget : MonoBehaviour
	{
		private IModelPaintingService _modelPaintingService;

		[Inject]
		private void Construct(IModelPaintingService modelPaintingService)
		{
			_modelPaintingService = modelPaintingService;
		}
	}
}