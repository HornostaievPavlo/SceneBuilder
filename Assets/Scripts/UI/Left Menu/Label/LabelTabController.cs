using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LabelTabController : MonoBehaviour
{
    [SerializeField] private GameObject labelDataMenu;
    [SerializeField] private Button updateButton;

    [SerializeField] private GameObject onScreenLabel;

    [SerializeField] private Sprite editToggle;
    [SerializeField] private Sprite editTogglePressed;

    private TMP_InputField _titleInput;
    private TMP_InputField _descriptionInput;

    private TMP_Text _onScreenLabelTitle;
    private TMP_Text _onScreenLabelDescription;

    [HideInInspector] public string title;

    [HideInInspector] public string description;

    [HideInInspector] public Toggle editLabelToggle;
    [HideInInspector] public TMP_Text currentTitle;
    [HideInInspector] public TMP_Text currentDescription;

    private void Awake()
    {
        TMP_InputField[] inputFields = GetComponentsInChildren<TMP_InputField>(true);

        _titleInput = inputFields[0];
        _descriptionInput = inputFields[1];

        TMP_Text[] onScreenTexts = onScreenLabel.GetComponentsInChildren<TMP_Text>(true);

        _onScreenLabelTitle = onScreenTexts[0];
        _onScreenLabelDescription = onScreenTexts[1];
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
    public void GetTitleText()
    {
        title = _titleInput.text.ToString();
    }

    /// <summary>
    /// Reads description text from input field
    /// </summary>
    public void GetDescriptionText()
    {
        description = _descriptionInput.text.ToString();
    }

    /// <summary>
    /// Turns on UI allowing to edit existing label 
    /// </summary>
    /// <param name="isEditMode">True if edit toggle is pressed</param>
    public void EditLabel(bool isEditMode)
    {
        editLabelToggle.image.sprite = isEditMode ? editTogglePressed : editToggle;

        labelDataMenu.SetActive(isEditMode);

        updateButton.gameObject.SetActive(isEditMode);

        _titleInput.text = currentTitle.text;

        _descriptionInput.text = currentDescription.text;

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

        _onScreenLabelTitle.text = currentTitle.text;

        _onScreenLabelDescription.text = currentDescription.text;
    }

    /// <summary>
    /// Turns off input menu
    /// </summary>
    public void HideDataMenu()
    {
        _titleInput.text = string.Empty;
        title = string.Empty;

        _descriptionInput.text = string.Empty;
        description = string.Empty;

        labelDataMenu.SetActive(false);

        if (editLabelToggle != null && editLabelToggle.image.sprite == editTogglePressed) editLabelToggle.image.sprite = editToggle;
    }
}