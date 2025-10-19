using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

namespace UI.Widgets
{
	public class ControlsInfoWidget : MonoBehaviour
	{
		[SerializeField] private Button closeButton;
		[SerializeField] private Button openSavesFolderButton;
		[SerializeField] private TMP_Text localSavesPathText;
		
		private void Awake()
		{
			localSavesPathText.text = $"Local saves are stored in:\n{Application.persistentDataPath}";
		}

		private void OnEnable()
		{
			closeButton.onClick.AddListener(OnCloseButtonClicked);
			openSavesFolderButton.onClick.AddListener(OnOpenSavesFolderClicked);
		}

		private void OnDisable()
		{
			closeButton.onClick.RemoveListener(OnCloseButtonClicked);
			openSavesFolderButton.onClick.RemoveListener(OnOpenSavesFolderClicked);
		}

		private void OnCloseButtonClicked()
		{
			gameObject.SetActive(false);
		}

		private void OnOpenSavesFolderClicked()
		{
			string path = Application.persistentDataPath;
			
			if (System.IO.Directory.Exists(path))
			{
				Process.Start(path);
			}
		}
	}
}