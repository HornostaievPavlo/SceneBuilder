using System.Collections;
using Plain;
using Services.Saving;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Widgets.LocalSaves
{
    public class CreateLocalSaveWidget : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private GameObject successInfoObject;

        private ScreenshotMaker _screenshotMaker;
    
        private ISaveService _saveService;

        [Inject]
        private void Construct(ISaveService saveService)
        {
            _saveService = saveService;
        }

        private void Awake()
        {
            _screenshotMaker = new ScreenshotMaker();
        }

        private void OnEnable()
        {
            button.onClick.AddListener(HandleClick);
        }    

        private void OnDisable()
        {
            button.onClick.RemoveListener(HandleClick);
        }

        private async void HandleClick()
        {
            Texture2D previewTexture = _screenshotMaker.CreatePreview();
            await _saveService.CreateLocalSave(previewTexture);
            
            StartCoroutine(ShowSuccessInfo());
        }

        private IEnumerator ShowSuccessInfo()
        {
            successInfoObject.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            successInfoObject.gameObject.SetActive(false);
        }
    }
}