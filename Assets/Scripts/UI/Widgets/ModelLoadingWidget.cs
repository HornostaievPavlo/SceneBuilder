using Services.Loading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Widgets
{
	public class ModelLoadingWidget : MonoBehaviour
	{
		[SerializeField] private TMP_InputField inputField;
		[SerializeField] private Button loadButton;
		[SerializeField] private GameObject processingPopup;
		
		private ILoadService _loadService;

		[Inject]
		private void Construct(ILoadService loadService)
		{
			_loadService = loadService;
		}
		
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
			await _loadService.LoadModel(input);
			processingPopup.SetActive(false);
			
			inputField.text = string.Empty;
		}
	}
}