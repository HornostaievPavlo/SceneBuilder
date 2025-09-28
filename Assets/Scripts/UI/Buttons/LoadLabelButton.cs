using Services.Loading;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace UI.Buttons
{
	public class LoadLabelButton : Button
	{
		private ILoadService _loadService;

		[Inject]
		private void Construct(ILoadService loadService)
		{
			_loadService = loadService;
		}
		
		public override void OnPointerClick(PointerEventData eventData)
		{
			base.OnPointerClick(eventData);
			_loadService.LoadLabel();
		}
	}
}