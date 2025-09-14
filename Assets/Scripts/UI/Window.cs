using Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour
{
    [SerializeField] private GameObject Toolbox;
    [SerializeField] private GameObject rightMenu;
    [SerializeField] private GameObject deleteButtonHover;
    [SerializeField] private Toggle contentToggle;
    [SerializeField] private GameObject content;

    private SceneObject currentSelection;

    private void OnEnable()
    {
        SelectionSystem.OnObjectSelected += OnObjectSelected;
        SelectionSystem.OnObjectDeselected += OnObjectDeselected;

        contentToggle.onValueChanged.AddListener(ToggleContent);
    }

    private void OnDisable()
    {
        SelectionSystem.OnObjectSelected -= OnObjectSelected;
        SelectionSystem.OnObjectDeselected -= OnObjectDeselected;

        contentToggle.onValueChanged.RemoveListener(ToggleContent);
    }

    private void OnObjectSelected(SceneObject scene)
    {
        Toolbox.SetActive(true);
        currentSelection = scene;
    }

    private void OnObjectDeselected()
    {
        Toolbox.SetActive(false);
        rightMenu.SetActive(false);

        currentSelection = null;
    }

    // public void CopySelectedObject()
    // {
    //     SelectableObjectUtility.CopySelectableObject(currentSelection);
    // }

    // public void DeleteSelectedObject()
    // {
    //     SelectableObjectUtility.DeleteSelectableObject(currentSelection);
    //     deleteButtonHover.gameObject.SetActive(false);
    //     this.OnObjectDeselected();
    // }

    public void ToggleContent(bool value)
    {
        content.SetActive(value);
        contentToggle.transform.eulerAngles = new Vector3(0, 0, value ? 0 : 180);
    }
}