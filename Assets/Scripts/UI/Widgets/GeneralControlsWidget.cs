using DG.Tweening;
using UI.Widgets.LocalSaves;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
    public class GeneralControlsWidget : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button loadButton;
        [SerializeField] private Button infoButton;
        [SerializeField] private Button quitButton;

        [Header("Widgets")]
        [SerializeField] private LocalSavesWidget localSavesWidget;
        [SerializeField] private ControlsInfoWidget controlsInfoWidget;

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
            AnimateButtonClick(loadButton.transform);
            localSavesWidget.Setup();
        }    

        private void OnInfoButtonClicked()
        {
            AnimateButtonClick(infoButton.transform);
            controlsInfoWidget.gameObject.SetActive(true);
        }

        private void OnQuitButtonClicked()
        {
            AnimateButtonClick(quitButton.transform);
            Application.Quit();
        }
        
        private void AnimateButtonClick(Transform buttonTransform)
        {
            buttonTransform.DOKill(true);
            buttonTransform.DOPunchScale(Vector3.one * 0.25f, 0.15f);
        }
    }
}