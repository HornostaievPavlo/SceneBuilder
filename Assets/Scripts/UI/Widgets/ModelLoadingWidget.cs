using System.Threading.Tasks;
using DG.Tweening;
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
		[SerializeField] private ProcessingWidget processingWidget;
		
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
			loadButton.transform.DOKill(true);
			loadButton.transform.DOPunchScale(Vector3.one * 0.5f, 0.2f);
			
			LoadModel();
		}

		private async void LoadModel()
		{
			string input = inputField.text;
			
			processingWidget.Show();
			await _loadService.LoadModel(input);
			processingWidget.Hide();
			
			inputField.text = string.Empty;
		}
	}
}