using System.Collections.Generic;
using LocalSaves;
using Services.Instantiation;
using Services.LocalSaves;
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

        private ILocalSavesService _localSavesService;
        private IInstantiateService _instantiateService;

        [Inject]
        private void Construct(ILocalSavesService localSavesService, IInstantiateService instantiateService)
        {
            _localSavesService = localSavesService;
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
            foreach (LocalSave localSave in _localSavesService.GetLocalSaves())
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