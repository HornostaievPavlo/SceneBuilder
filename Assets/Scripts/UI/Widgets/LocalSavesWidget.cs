using System.Collections.Generic;
using LocalSaves;
using Services.Instantiation;
using Services.LocalSavesRepository;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Widgets
{
    public class LocalSavesWidget : MonoBehaviour
    {
        [SerializeField] private GameObject localSaveWidgetPrefab;
        [SerializeField] private Transform content;
        [SerializeField] private Button closeButton;
        
        private readonly List<LocalSaveWidget> _localSavesWidgets = new();

        private ILocalSavesRepository _localSavesRepository;
        private IInstantiateService _instantiateService;

        [Inject]
        private void Construct(ILocalSavesRepository localSavesRepository, IInstantiateService instantiateService)
        {
            _localSavesRepository = localSavesRepository;
            _instantiateService = instantiateService;
        }

        private void OnEnable()
        {
            closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        private void OnDisable()
        {
            closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        }

        public void Setup()
        {
            Cleanup();
            SetupWidgets();
            gameObject.SetActive(true);
        }

        private void Cleanup()
        {
            foreach (LocalSaveWidget widget in _localSavesWidgets)
            {
                Destroy(widget.gameObject);
            }
            
            _localSavesWidgets.Clear();
        }

        private void SetupWidgets()
        {
            foreach (LocalSave localSave in _localSavesRepository.GetLocalSaves())
            {
                LocalSaveWidget widget = _instantiateService.Instantiate<LocalSaveWidget>(localSaveWidgetPrefab, content);
                widget.Setup(localSave);
            
                _localSavesWidgets.Add(widget);
            }
        }

        private void OnCloseButtonClicked()
        {
            gameObject.SetActive(false);
        }
    }
}