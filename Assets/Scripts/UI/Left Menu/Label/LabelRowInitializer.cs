using UnityEngine;
using UnityEngine.UI;

public class LabelRowInitializer : MonoBehaviour
{
    private LabelEditor labelEditor;

    private void Awake()
    {
        labelEditor = GetComponentInParent<LabelEditor>();

        Toggle editModeToggle = gameObject.GetComponentInChildren<Toggle>(true);
        editModeToggle.onValueChanged.AddListener(delegate { labelEditor.SetLabelEditMode(editModeToggle.isOn); });
    }
}