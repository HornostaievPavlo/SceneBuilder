using UnityEngine;
using UnityEngine.UI;

public class LabelRowInitializer : MonoBehaviour
{
    private LabelEditor labelEditor;

    private void Awake()
    {
        labelEditor = GetComponentInParent<LabelEditor>();

        Toggle toggle = gameObject.GetComponentInChildren<Toggle>(true);
        toggle.onValueChanged.AddListener(delegate { labelEditor.SetLabelEditMode(toggle.isOn); });
    }
}