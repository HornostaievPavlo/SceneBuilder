using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LabelEditor : MonoBehaviour
{
    [SerializeField] private GameObject labelDataMenu;

    [SerializeField] private Sprite editNormal;
    [SerializeField] private Sprite editSelected;

    private GameObject currentSelectable;

    private Toggle editLabelToggle;

    private TMP_InputField titleInput;
    private TMP_InputField descriptionInput;

    private string title;
    private string description;
    private TMP_Text currentTitle;
    private TMP_Text currentDescription;

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
        if (selectable.type != AssetType.Label)
            return;

        currentSelectable = selectable.gameObject;

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

    public void GetTitleText() => title = titleInput.text.ToString();

    public void GetDescriptionText() => description = descriptionInput.text.ToString();

    public void SetLabelEditMode(bool isEditModeOn)
    {
        editLabelToggle.image.sprite = isEditModeOn ? editSelected : editNormal;

        labelDataMenu.SetActive(isEditModeOn);

        titleInput.text = currentTitle.text;
        descriptionInput.text = currentDescription.text;
    }

    /// <summary>
    /// Takes input and changes text values to new ones
    /// </summary>
    public void UpdateLabel()
    {
        GetTitleText();
        GetDescriptionText();

        var rowTexts = editLabelToggle.transform.parent.gameObject.GetComponentsInChildren<TMP_Text>();

        rowTexts[1].text = title;
        rowTexts[2].text = description;

        var labelText = currentSelectable.GetComponentInChildren<TMP_Text>();
        labelText.text = title + "\n" + description;

        HideDataMenu();
    }

    public void HideDataMenu()
    {
        labelDataMenu.SetActive(false);

        ResetTemporaryData();
    }

    private void ResetTemporaryData()
    {
        titleInput.text = string.Empty;
        title = string.Empty;

        descriptionInput.text = string.Empty;
        description = string.Empty;

        if (editLabelToggle.image.sprite == editSelected)
            editLabelToggle.image.sprite = editNormal;
    }
}