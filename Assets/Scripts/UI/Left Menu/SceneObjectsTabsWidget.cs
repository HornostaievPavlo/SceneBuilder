using UnityEngine;
using UnityEngine.UI;

namespace UI.Left_Menu
{
    public class SceneObjectsTabsWidget : MonoBehaviour
    {
        [SerializeField] private Toggle modelsTabToggle;
        [SerializeField] private Toggle camerasTabToggle;
        [SerializeField] private Toggle labelsTabToggle;

        [SerializeField] private GameObject modelsTabParent;
        [SerializeField] private GameObject camerasTabParent;
        [SerializeField] private GameObject labelsTabParent;

        private void OnEnable()
        {
            modelsTabToggle.onValueChanged.AddListener(HandleModelsTabToggleValueChanged);
            camerasTabToggle.onValueChanged.AddListener(HandleCamerasTabToggleValueChanged);
            labelsTabToggle.onValueChanged.AddListener(HandleLabelsTabToggleValueChanged);
            
            modelsTabParent.gameObject.SetActive(false);
            camerasTabParent.gameObject.SetActive(false);
            labelsTabParent.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            modelsTabToggle.onValueChanged.RemoveListener(HandleModelsTabToggleValueChanged);
            camerasTabToggle.onValueChanged.RemoveListener(HandleCamerasTabToggleValueChanged);
            labelsTabToggle.onValueChanged.RemoveListener(HandleLabelsTabToggleValueChanged);
        }

        private void HandleModelsTabToggleValueChanged(bool value)
        {
            modelsTabParent.SetActive(value);
        }

        private void HandleCamerasTabToggleValueChanged(bool value)
        {
            camerasTabParent.SetActive(value);
        }

        private void HandleLabelsTabToggleValueChanged(bool value)
        {
            labelsTabParent.SetActive(value);
        }
    }
}
