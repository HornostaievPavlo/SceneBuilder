using LocalSaves;
using Services.Loading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Widgets
{
    public class LocalSaveWidget : MonoBehaviour
    {
        [SerializeField] private RawImage preview;
        [SerializeField] private Button loadButton;
        [SerializeField] private Button deleteButton;

        private LocalSave _localSave;
        
        private ILoadService _loadService;

        [Inject]
        private void Construct(ILoadService loadService)
        {
            _loadService = loadService;
        }

        private void OnEnable()
        {
            loadButton.onClick.AddListener(HandleLoadButtonClicked);
            deleteButton.onClick.AddListener(HandleDeleteButtonClicked);
        }
        
        private void OnDisable()
        {
            loadButton.onClick.RemoveListener(HandleLoadButtonClicked);
            deleteButton.onClick.RemoveListener(HandleDeleteButtonClicked);
        }

        public void Setup(LocalSave localSave)
        {
            _localSave = localSave;
            preview.texture = localSave.Preview;
        }

        private void HandleLoadButtonClicked()
        {
            _loadService.LoadLocalSave(_localSave);
        }

        private void HandleDeleteButtonClicked()
        {
            
        }
    }
}
