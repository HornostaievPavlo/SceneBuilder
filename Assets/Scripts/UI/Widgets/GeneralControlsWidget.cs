using UnityEngine;
using UnityEngine.UI;

public class GeneralControlsWidget : MonoBehaviour
{
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button infoButton;

    private void OnEnable()
    {
        saveButton.onClick.AddListener(OnSaveButtonClicked);
        loadButton.onClick.AddListener(OnLoadButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
        infoButton.onClick.AddListener(OnInfoButtonClicked);
    }

    private void OnDisable()
    {
        saveButton.onClick.RemoveListener(OnSaveButtonClicked);
        loadButton.onClick.RemoveListener(OnLoadButtonClicked);
        quitButton.onClick.RemoveListener(OnQuitButtonClicked);
        infoButton.onClick.RemoveListener(OnInfoButtonClicked);
    }

    private void OnSaveButtonClicked()
    {
        
    }

    private void OnLoadButtonClicked()
    {
        
    }

    private void OnQuitButtonClicked()
    {

    }

    private void OnInfoButtonClicked()
    {

    }
}