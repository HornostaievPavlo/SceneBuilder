using Enums;
using Gameplay;
using Services.SceneObjectSelection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

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
    
    private ISceneObjectSelectionService _sceneObjectSelectionService;

    private string InputTitle => titleInput.text.ToString();
    private string InputDescription => descriptionInput.text.ToString();

    [Inject]
    private void Construct(ISceneObjectSelectionService sceneObjectSelectionService)
    {
        _sceneObjectSelectionService = sceneObjectSelectionService;
    }

    private void Awake()
    {
        var inputFields = labelDataMenu.GetComponentsInChildren<TMP_InputField>(true);

        titleInput = inputFields[0];
        descriptionInput = inputFields[1];
    }

    private void OnEnable()
    {
        _sceneObjectSelectionService.OnObjectSelected += OnObjectSelected;
        _sceneObjectSelectionService.OnObjectDeselected += OnObjectDeselected;
    }

    private void OnDisable()
    {
        _sceneObjectSelectionService.OnObjectSelected -= OnObjectSelected;
        _sceneObjectSelectionService.OnObjectDeselected -= OnObjectDeselected;
    }

    private void OnObjectSelected(SceneObject scene)
    {
        if (scene.TypeId != SceneObjectTypeId.Label)
            return;

        // currentSelectable = scene.transform;
        //
        // editLabelToggle = scene.MenuRow.gameObject.GetComponentInChildren<Toggle>(true);
        // editLabelToggle.gameObject.SetActive(true);
        //
        // TMP_Text[] labelTexts = scene.MenuRow.gameObject.GetComponentsInChildren<TMP_Text>();
        //
        // currentTitle = labelTexts[1];
        // currentDescription = labelTexts[2];
    }

    private void OnObjectDeselected()
    {
        // if (labelDataMenu.activeSelf) HideDataMenu();
        //
        // editLabelToggle.gameObject.SetActive(false);
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