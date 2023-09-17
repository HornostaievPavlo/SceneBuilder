using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LabelEditor : MonoBehaviour
{
    [SerializeField] private GameObject labelDataMenu;
    [SerializeField] private Button updateButton;

    [SerializeField] private GameObject onScreenLabel;

    [SerializeField] private Sprite editToggle;
    [SerializeField] private Sprite editTogglePressed;

    private TMP_InputField titleInput;
    private TMP_InputField descriptionInput;

    private TMP_Text onScreenLabelTitle;
    private TMP_Text onScreenLabelDescription;

    [HideInInspector] public string title;

    [HideInInspector] public string description;

    private Toggle editLabelToggle;

    private TMP_Text currentTitle;
    private TMP_Text currentDescription;

    private void Awake()
    {
        TMP_InputField[] inputFields = GetComponentsInChildren<TMP_InputField>(true);

        titleInput = inputFields[0];
        descriptionInput = inputFields[1];

        TMP_Text[] onScreenTexts = onScreenLabel.GetComponentsInChildren<TMP_Text>(true);

        onScreenLabelTitle = onScreenTexts[0];
        onScreenLabelDescription = onScreenTexts[1];
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
        if (selectable.type == AssetType.Label)
        {
            editLabelToggle = selectable.MenuRow.gameObject.GetComponentInChildren<Toggle>(true);
            editLabelToggle.gameObject.SetActive(true);

            TMP_Text[] labelTexts = selectable.MenuRow.gameObject.GetComponentsInChildren<TMP_Text>();

            currentTitle = labelTexts[1];
            currentDescription = labelTexts[2];

            ShowOnScreenLabel(true);
        }
    }

    private void OnObjectDeselected()
    {
        ShowOnScreenLabel(false);
        editLabelToggle.gameObject.SetActive(false);
    }

    /// <summary>
    /// Turns on adding label window
    /// </summary>
    public void AddNewLabel()
    {
        labelDataMenu.SetActive(true);
        updateButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Reads title text from input field
    /// </summary>
    public void GetTitleText() => title = titleInput.text.ToString();

    /// <summary>
    /// Reads description text from input field
    /// </summary>
    public void GetDescriptionText() => description = descriptionInput.text.ToString();

    /// <summary>
    /// Turns on UI allowing to edit existing label 
    /// </summary>
    /// <param name="isEditMode">True if edit toggle is pressed</param>
    public void EditLabel(bool isEditMode)
    {
        editLabelToggle.image.sprite = isEditMode ? editTogglePressed : editToggle;

        labelDataMenu.SetActive(isEditMode);
        updateButton.gameObject.SetActive(isEditMode);

        titleInput.text = currentTitle.text;
        descriptionInput.text = currentDescription.text;

        ShowOnScreenLabel(false);
    }

    /// <summary>
    /// Takes input and changes text values to new ones
    /// </summary>
    public void UpdateLabel()
    {
        GetTitleText();
        GetDescriptionText();

        TMP_Text[] texts = editLabelToggle.transform.parent.gameObject.GetComponentsInChildren<TMP_Text>();

        texts[1].text = title;
        texts[2].text = description;

        HideDataMenu();

        editLabelToggle.image.sprite = editToggle;
    }

    /// <summary>
    /// Turns on/off on screen label depending on selection
    /// </summary>
    /// <param name="isLabelSelected">True if label object is selected, false if not</param>
    public void ShowOnScreenLabel(bool isLabelSelected)
    {
        onScreenLabel.SetActive(isLabelSelected);

        onScreenLabelTitle.text = currentTitle.text;
        onScreenLabelDescription.text = currentDescription.text;
    }

    /// <summary>
    /// Turns off input menu
    /// </summary>
    public void HideDataMenu()
    {
        titleInput.text = string.Empty;
        title = string.Empty;

        descriptionInput.text = string.Empty;
        description = string.Empty;

        labelDataMenu.SetActive(false);

        if (editLabelToggle != null && editLabelToggle.image.sprite == editTogglePressed) editLabelToggle.image.sprite = editToggle;
    }
}