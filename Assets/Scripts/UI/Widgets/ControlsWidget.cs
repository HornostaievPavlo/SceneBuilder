using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
	public class ControlsWidget : MonoBehaviour
	{
		[SerializeField] private Button closeButton;

		private void OnEnable()
		{
			closeButton.onClick.AddListener(OnCloseButtonClicked);
		}

		private void OnDisable()
		{
			closeButton.onClick.RemoveListener(OnCloseButtonClicked);
		}

		private void OnCloseButtonClicked()
		{
			gameObject.SetActive(false);
		}
	}
}