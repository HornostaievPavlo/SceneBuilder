using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Window : MonoBehaviour
    {
        [SerializeField] private Toggle contentToggle;
        [SerializeField] private GameObject content;

        private void OnEnable()
        {
            contentToggle.onValueChanged.AddListener(ToggleContent);
        }

        private void OnDisable()
        {
            contentToggle.onValueChanged.RemoveListener(ToggleContent);
        }

        private void ToggleContent(bool value)
        {
            content.SetActive(value);
            contentToggle.transform.eulerAngles = new Vector3(0, 0, value ? 0 : 180);
        }
    }
}