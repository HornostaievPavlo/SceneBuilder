using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LabelEditor : MonoBehaviour
{
    [SerializeField] private GameObject labelDataMenu;

    [SerializeField] private Sprite editNormal;
    [SerializeField] private Sprite editSelected;

    private Transform currentSelectable;

    private Toggle editLabelToggle;

    private TMP_InputField titleInput;
    private TMP_InputField descriptionInput;

    private TMP_Text currentTitle;
    private TMP_Text currentDescription;

    private string InputTitle => titleInput.text.ToString();
    private string InputDescription => descriptionInput.text.ToString();

    private void Awake()
    {
        var inputFields = labelDataMenu.GetComponentsInChildren<TMP_InputField>(true);

        titleInput = inputFields[0];
        descriptionInput = inputFields[1];
    }

    private void OnEnable()
    {
        SelectionSystem.OnObjectSelected += OnObjectSelected;
        SelectionSystem.OnObjectDeselected += OnObjectDeselected;
    }

    private void OnDisable()
    {
        SelectionSystem.OnObjectSelected -= OnObjectSelected;
        SelectionSystem.OnObjectDeselected -= OnObjectDeselected;
    }

    private void OnObjectSelected(SelectableObject selectable)
    {
        if (selectable.TypeId != AssetTypeId.Label)
            return;

        currentSelectable = selectable.transform;

        editLabelToggle = selectable.MenuRow.gameObject.GetComponentInChildren<Toggle>(true);
        editLabelToggle.gameObject.SetActive(true);

        TMP_Text[] labelTexts = selectable.MenuRow.gameObject.GetComponentsInChildren<TMP_Text>();

        currentTitle = labelTexts[1];
        currentDescription = labelTexts[2];
    }

    private void OnObjectDeselected()
    {
        if (labelDataMenu.activeSelf) HideDataMenu();

        editLabelToggle.gameObject.SetActive(false);
    }

    public void SetLabelEditMode(bool isEditModeOn)
    {
        editLabelToggle.image.sprite = isEditModeOn ? editSelected : editNormal;

        labelDataMenu.SetActive(isEditModeOn);

        titleInput.text = currentTitle.text;
        descriptionInput.text = currentDescription.text;
    }

    public void UpdateLabel()
    {
        var rowTexts = editLabelToggle.transform.parent.gameObject.GetComponentsInChildren<TMP_Text>();

        rowTexts[1].text = InputTitle;
        rowTexts[2].text = InputDescription;

        var labelText = currentSelectable.GetComponentInChildren<TMP_Text>();
        labelText.text = InputTitle + "\n" + InputDescription;

        HideDataMenu();
    }

    public void HideDataMenu()
    {
        labelDataMenu.SetActive(false);

        titleInput.text = string.Empty;
        descriptionInput.text = string.Empty;

        if (editLabelToggle.image.sprite == editSelected)
            editLabelToggle.image.sprite = editNormal;
    }
}