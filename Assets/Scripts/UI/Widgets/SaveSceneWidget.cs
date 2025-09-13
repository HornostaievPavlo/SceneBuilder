using UnityEngine;
using UnityEngine.UI;

public class SaveSceneWidget : MonoBehaviour
{
    [SerializeField] private SavingSystem savingSystem;
    [SerializeField] private Button button;
    [SerializeField] private GameObject progressPopup;

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
        await savingSystem.SaveCurrentScene();
        progressPopup.SetActive(false);
    }
}