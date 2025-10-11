using UI.Widgets;
using UnityEngine;
using UnityEngine.UI;

public class GeneralControlsWidget : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button loadButton;
    [SerializeField] private Button infoButton;
    [SerializeField] private Button quitButton;

    [Header("Widgets")]
    [SerializeField] private LocalSavesWidget localSavesWidget;
    [SerializeField] private ControlsWidget controlsWidget;

    private void OnEnable()
    {
        loadButton.onClick.AddListener(OnLoadButtonClicked);
        infoButton.onClick.AddListener(OnInfoButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnDisable()
    {
        loadButton.onClick.RemoveListener(OnLoadButtonClicked);
        infoButton.onClick.RemoveListener(OnInfoButtonClicked);
        quitButton.onClick.RemoveListener(OnQuitButtonClicked);
    }

    private void OnLoadButtonClicked()
    {
        localSavesWidget.gameObject.SetActive(true);
    }    

    private void OnInfoButtonClicked()
    {
        controlsWidget.gameObject.SetActive(true);
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}