using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LabelTabController : MonoBehaviour
{
    [SerializeField] private GameObject labelDataMenu;
    [SerializeField] private Button updateButton;

    [SerializeField] private GameObject onScreenLabel;

    [SerializeField] private Sprite editToggle;
    [SerializeField] private Sprite editTogglePressed;

    private Menu Menu;

    private TMP_InputField titleInput;
    private TMP_InputField descriptionInput;

    private TMP_Text onScreenLabelTitle;
    private TMP_Text onScreenLabelDescription;

    [HideInInspector] public string title;

    [HideInInspector] public string description;

    [HideInInspector] public Toggle editLabelToggle;
    [HideInInspector] public TMP_Text currentTitle;
    [HideInInspector] public TMP_Text currentDescription;

    private void Awake()
    {
        Menu = GetComponentInParent<Menu>();

        TMP_InputField[] inputFields = GetComponentsInChildren<TMP_InputField>(true);

        titleInput = inputFields[0];
        descriptionInput = inputFields[1];

        TMP_Text[] onScreenTexts = onScreenLabel.GetComponentsInChildren<TMP_Text>(true);

        onScreenLabelTitle = onScreenTexts[0];
        onScreenLabelDescription = onScreenTexts[1];
    }

    public void AddNewLabel()
    {
        labelDataMenu.SetActive(true);
        updateButton.gameObject.SetActive(false);
    }

    public void GetTitleText()
    {
        title = titleInput.text.ToString();
    }

    public void GetDescriptionText()
    {
        description = descriptionInput.text.ToString();
    }

    public void EditLabel(bool isEditMode)
    {
        editLabelToggle.image.sprite = isEditMode ? editTogglePressed : editToggle;



        labelDataMenu.SetActive(isEditMode);

        updateButton.gameObject.SetActive(isEditMode);

        titleInput.text = currentTitle.text;

        descriptionInput.text = currentDescription.text;

        ShowOnScreenLabel(false);
    }

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

    public void ShowOnScreenLabel(bool isLabelSelected)
    {
        onScreenLabel.SetActive(isLabelSelected);

        onScreenLabelTitle.text = currentTitle.text;

        onScreenLabelDescription.text = currentDescription.text;
    }

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