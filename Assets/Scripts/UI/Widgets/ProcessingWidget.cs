using DG.Tweening;
using UnityEngine;

namespace UI.Widgets
{
	public class ProcessingWidget : MonoBehaviour
	{
		[SerializeField] private CanvasGroup canvasGroup;

		public void Show()
		{
			gameObject.SetActive(true);
			AnimateProcessingPopup(isEnabled: true);
		}

		public void Hide()
		{
			AnimateProcessingPopup(isEnabled: false);
		}

		private void AnimateProcessingPopup(bool isEnabled)
		{
			canvasGroup.alpha = isEnabled ? 0f : 1f;
			float endValue = isEnabled ? 1f : 0f;
			
			canvasGroup.DOKill(true);
			DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, endValue, 0.2f)
				.OnComplete(() =>
				{
					if (isEnabled == false)
					{
						gameObject.SetActive(false);
					}
				});
		}
	}
}