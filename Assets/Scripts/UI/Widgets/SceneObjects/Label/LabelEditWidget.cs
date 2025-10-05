using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets.SceneObjects.Label
{
	public class LabelEditWidget : MonoBehaviour
	{
		[SerializeField] private Button closeButton;
		[SerializeField] private Button applyButton;

		[SerializeField] private TMP_InputField titleInputField;
		[SerializeField] private TMP_InputField descriptionInputField;
		
		private Gameplay.Label _label;
		
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

		public void Setup(Gameplay.Label label)
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
			if (titleInputField.text != string.Empty && titleInputField.text != _label.Title)
			{
				_label.SetTitle(titleInputField.text);
			}
			
			if (descriptionInputField.text != string.Empty && descriptionInputField.text != _label.Description)
			{
				_label.SetDescription(descriptionInputField.text);
			}

			OnClosed?.Invoke();
			gameObject.SetActive(false);
		}
	}
}