using UnityEngine;

public class ApplicationUI : MonoBehaviour
{
    [SerializeField]
    private GameObject Toolbox;

    [SerializeField]
    private Transform uiStateToggle;

    private Transform currentSelection;

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
        currentSelection = selectable.transform;
    }

    private void OnObjectDeselected()
    {
        Toolbox.SetActive(false);
        currentSelection = null;
    }

    public void CopySelectedObject()
    {
        if (currentSelection != null)
        {
            Vector3 copyPosition = new Vector3(0, currentSelection.transform.position.y, 0);

            var copy = Instantiate(currentSelection.transform,
                        copyPosition,
                        currentSelection.transform.rotation,
                        currentSelection.transform.parent);

            copy.gameObject.name = currentSelection.name;
        }
    }

    public void DeleteSelectedObject()
    {
        if (currentSelection != null)
        {
            Destroy(currentSelection.gameObject);
            OnObjectDeselected();
        }
    }

    public void SetUIActive(bool isActive)
    {
        gameObject.SetActive(isActive);

        uiStateToggle.eulerAngles = new Vector3(0, 0, isActive ? 0 : 180);
    }

    public void QuitApplication() => Application.Quit();
}