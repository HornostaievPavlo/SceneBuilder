using System.Collections;
using System.Collections.Generic;
using Plain;
using Services.Instantiation;
using Services.Loading;
using Services.LocalSavesRepository;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Widgets.LocalSaves
{
    public class LocalSavesWidget : MonoBehaviour
    {
        [SerializeField] private GameObject localSaveWidgetPrefab;
        [SerializeField] private Transform content;
        [SerializeField] private Button closeButton;
        
        private readonly List<LocalSaveWidget> _localSavesWidgets = new();

        private ILocalSavesRepository _localSavesRepository;
        private IInstantiateService _instantiateService;
        private ILoadService _loadService;

        [Inject]
        private void Construct(
            ILocalSavesRepository localSavesRepository, 
            IInstantiateService instantiateService,
            ILoadService loadService)
        {
            _localSavesRepository = localSavesRepository;
            _instantiateService = instantiateService;
            _loadService = loadService;
        }

        private void OnEnable()
        {
            closeButton.onClick.AddListener(HandleCloseButtonClicked);
            
            _localSavesRepository.OnLocalSaveCreated += HandleLocalSaveCreated;
            _localSavesRepository.OnLocalSaveDeleted += HandleLocalSaveDeleted;

            _loadService.OnLocalSaveLoaded += HandleLocalSaveLoaded;
        }

        private void OnDisable()
        {
            closeButton.onClick.RemoveListener(HandleCloseButtonClicked);
            
            _localSavesRepository.OnLocalSaveCreated -= HandleLocalSaveCreated;
            _localSavesRepository.OnLocalSaveDeleted -= HandleLocalSaveDeleted;
            
            _loadService.OnLocalSaveLoaded -= HandleLocalSaveLoaded;
        }

        public void Setup()
        {
            Cleanup();
            gameObject.SetActive(true);

            StartCoroutine(WidgetsSetupSequence());
        }

        private IEnumerator WidgetsSetupSequence()
        {
            List<LocalSave> localSaves = _localSavesRepository.GetLocalSaves();
            
            for (var i = 0; i < localSaves.Count; i++)
            {
                LocalSave localSave = localSaves[i];
                
                if ( i > 0)
                {
                    yield return new WaitForSeconds(0.05f);
                }

                CreateWidget(localSave);
            }
        }

        private void Cleanup()
        {
            foreach (LocalSaveWidget widget in _localSavesWidgets)
            {
                Destroy(widget.gameObject);
            }
            
            _localSavesWidgets.Clear();
        }

        private void CreateWidget(LocalSave localSave)
        {
            LocalSaveWidget widget = _instantiateService.Instantiate<LocalSaveWidget>(localSaveWidgetPrefab, content);
            widget.Setup(localSave);
            
            _localSavesWidgets.Add(widget);
        }

        private void HandleLocalSaveCreated(LocalSave localSave)
        {
            CreateWidget(localSave);
        }

        private void HandleLocalSaveDeleted(LocalSave localSave)
        {
            foreach (LocalSaveWidget widget in _localSavesWidgets)
            {
                if (widget.LocalSave != localSave) 
                    continue;
                
                _localSavesWidgets.Remove(widget);
                Destroy(widget.gameObject);
                break;
            }
            
            List<LocalSave> localSaves = _localSavesRepository.GetLocalSaves();
            
            for (int i = 0; i < _localSavesWidgets.Count; i++)
            {
                if (i >= localSaves.Count) 
                    break;
                
                _localSavesWidgets[i].Setup(localSaves[i]);
            }
        }

        private void HandleCloseButtonClicked()
        {
            Close();
        }
        
        private void HandleLocalSaveLoaded()
        {
            Close();
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }
    }
}