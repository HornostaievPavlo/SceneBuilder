using DG.Tweening;
using Plain;
using Services.Loading;
using Services.Saving;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Widgets.LocalSaves
{
    public class LocalSaveWidget : MonoBehaviour
    {
        [SerializeField] private RawImage preview;
        [SerializeField] private Button loadButton;
        [SerializeField] private Button deleteButton;
        [SerializeField] private CanvasGroup canvasGroup;

        private LocalSave _localSave;
        
        private ILoadService _loadService;
        private ISaveService _saveService;
        
        public LocalSave LocalSave => _localSave;

        [Inject]
        private void Construct(ILoadService loadService, ISaveService saveService)
        {
            _loadService = loadService;
            _saveService = saveService;
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
            
            AnimateAppear();
        }

        private void HandleLoadButtonClicked()
        {
            _loadService.LoadLocalSave(_localSave);
        }

        private void HandleDeleteButtonClicked()
        {
            _saveService.DeleteLocalSave(_localSave);
        }

        private void AnimateAppear()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.DOKill(true);
            DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1f, 0.15f);
        }
    }
}
