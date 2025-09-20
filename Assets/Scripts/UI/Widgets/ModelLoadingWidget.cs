using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
	public class ModelLoadingWidget : MonoBehaviour
	{
		[SerializeField] private TMP_InputField inputField;
		[SerializeField] private Button loadButton;
		[SerializeField] private GameObject processingPopup;
		[SerializeField] private LoadingSystem loadingSystem;

		private void OnEnable()
		{
			loadButton.onClick.AddListener(HandleLoadButtonClicked);
		}
		
		private void OnDisable()
		{
			loadButton.onClick.RemoveListener(HandleLoadButtonClicked);
		}
		
		private void HandleLoadButtonClicked()
		{
			LoadModel();
		}

		private async void LoadModel()
		{
			string input = inputField.text;
			
			processingPopup.SetActive(true);
			await loadingSystem.LoadModel(input);
			processingPopup.SetActive(false);
			
			inputField.text = string.Empty;
		}
	}
}