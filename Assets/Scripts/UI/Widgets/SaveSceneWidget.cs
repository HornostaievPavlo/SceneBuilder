using Services.Saving;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SaveSceneWidget : MonoBehaviour
{
    [SerializeField] private SavingSystem savingSystem;
    [SerializeField] private Button button;
    [SerializeField] private GameObject progressPopup;
    
    private ISaveService _saveService;

    [Inject]
    private void Construct(ISaveService saveService)
    {
        _saveService = saveService;
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
        await _saveService.SaveCurrentScene();
        progressPopup.SetActive(false);
    }
}