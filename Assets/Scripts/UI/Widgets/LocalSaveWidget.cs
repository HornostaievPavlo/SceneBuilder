using LocalSaves;
using Services.Saving;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Widgets
{
    public class LocalSaveWidget : MonoBehaviour
    {
        [SerializeField] private RawImage preview;
        
        private ISaveService _saveService;

        // public RawImage preview => GetComponentInChildren<RawImage>();

        public Button loadButton => GetComponentsInChildren<Button>()[0];
        public Button deleteButton => GetComponentsInChildren<Button>()[1];

        [Inject]
        private void Construct(ISaveService saveService)
        {
            _saveService = saveService;
        }

        public void Setup(LocalSave localSave)
        {
            preview.texture = localSave.Preview;
        }
    }
}
