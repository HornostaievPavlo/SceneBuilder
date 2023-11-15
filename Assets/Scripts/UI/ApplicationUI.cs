using UnityEngine;

public class ApplicationUI : MonoBehaviour
{
    [SerializeField]
    private GameObject Toolbox;

    [SerializeField]
    private GameObject rightMenu;

    [SerializeField]
    private Transform uiStateToggle;

    [SerializeField]
    private GameObject deleteButtonHover;

    private SelectableObject currentSelection;

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
        Toolbox.SetActive(true);
        currentSelection = selectable;
    }

    private void OnObjectDeselected()
    {
        Toolbox.SetActive(false);
        rightMenu.SetActive(false);

        currentSelection = null;
    }

    public void CopySelectedObject()
    {
        SelectableObjectUtility.CopySelectableObject(currentSelection);
    }

    public void DeleteSelectedObject()
    {
        SelectableObjectUtility.DeleteSelectableObject(currentSelection);
        deleteButtonHover.SetActive(false);
        this.OnObjectDeselected();
    }

    public void SetUIActive(bool isActive)
    {
        gameObject.SetActive(isActive);

        uiStateToggle.eulerAngles = new Vector3(0, 0, isActive ? 0 : 180);
    }
}