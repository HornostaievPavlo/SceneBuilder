using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets.SceneObjects
{
    public class SceneObjectsTabsWidget : MonoBehaviour
    {
        [SerializeField] private Toggle modelsTabToggle;
        [SerializeField] private Toggle camerasTabToggle;
        [SerializeField] private Toggle labelsTabToggle;

        [SerializeField] private SceneObjectInfoLayoutWidget modelsLayoutWidget;
        [SerializeField] private SceneObjectInfoLayoutWidget camerasLayoutWidget;
        [SerializeField] private SceneObjectInfoLayoutWidget labelsLayoutWidget;
        
        private SceneObjectInfoLayoutWidget _lastActiveWidget;

        private void OnEnable()
        {
            modelsTabToggle.onValueChanged.AddListener(HandleModelsTabToggleValueChanged);
            camerasTabToggle.onValueChanged.AddListener(HandleCamerasTabToggleValueChanged);
            labelsTabToggle.onValueChanged.AddListener(HandleLabelsTabToggleValueChanged);
            
            modelsLayoutWidget.gameObject.SetActive(false);
            camerasLayoutWidget.gameObject.SetActive(false);
            labelsLayoutWidget.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            modelsTabToggle.onValueChanged.RemoveListener(HandleModelsTabToggleValueChanged);
            camerasTabToggle.onValueChanged.RemoveListener(HandleCamerasTabToggleValueChanged);
            labelsTabToggle.onValueChanged.RemoveListener(HandleLabelsTabToggleValueChanged);
        }

        public void Setup(bool isEnabled)
        {
            if (isEnabled == false)
            {
                _lastActiveWidget.gameObject.SetActive(false);
                ChangeLastActiveToggleState(false);
                return;
            }
            
            if (_lastActiveWidget == null)
            {
                modelsLayoutWidget.gameObject.SetActive(true);
                modelsTabToggle.isOn = true;
                _lastActiveWidget = modelsLayoutWidget;
            }
            else
            {
                _lastActiveWidget.gameObject.SetActive(true);
                ChangeLastActiveToggleState(true);
            }
        }

        private void HandleModelsTabToggleValueChanged(bool value)
        {
            modelsLayoutWidget.gameObject.SetActive(value);
            _lastActiveWidget = modelsLayoutWidget;
        }

        private void HandleCamerasTabToggleValueChanged(bool value)
        {
            camerasLayoutWidget.gameObject.SetActive(value);
            _lastActiveWidget = camerasLayoutWidget;
        }

        private void HandleLabelsTabToggleValueChanged(bool value)
        {
            labelsLayoutWidget.gameObject.SetActive(value);
            _lastActiveWidget = labelsLayoutWidget;
        }

        private void ChangeLastActiveToggleState(bool value)
        {
            if (_lastActiveWidget == modelsLayoutWidget)
                modelsTabToggle.isOn = value;
            else if (_lastActiveWidget == camerasLayoutWidget)
                camerasTabToggle.isOn = value;
            else if (_lastActiveWidget == labelsLayoutWidget)
                labelsTabToggle.isOn = value;
        }
    }
}
