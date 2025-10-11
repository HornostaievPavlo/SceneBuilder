using System;
using Gameplay;
using Services.Saving;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SaveSceneWidget : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject progressPopup;

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
        progressPopup.SetActive(true);

        Texture2D previewTexture = _screenshotMaker.CreatePreview();
        await _saveService.CreateLocalSave(previewTexture);
        
        progressPopup.SetActive(false);
    }
}