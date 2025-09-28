using System;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
	public class LabelEditWidget : MonoBehaviour
	{
		[SerializeField] private Button closeButton;
		[SerializeField] private Button applyButton;
		
		private Label _label;
		
		public event Action OnClosed;
		
		private void OnEnable()
		{
			closeButton.onClick.AddListener(HandleCloseButtonClicked);
			applyButton.onClick.AddListener(HandleApplyButtonClicked);
		}
		
		private void OnDisable()
		{
			closeButton.onClick.RemoveListener(HandleCloseButtonClicked);
			applyButton.onClick.RemoveListener(HandleApplyButtonClicked);
		}

		public void Setup(Label label)
		{
			gameObject.SetActive(true);
			_label = label;
		}

		private void HandleCloseButtonClicked()
		{
			OnClosed?.Invoke();
			gameObject.SetActive(false);
		}

		private void HandleApplyButtonClicked()
		{
			OnClosed?.Invoke();
			gameObject.SetActive(false);
		}
	}
}