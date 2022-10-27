using UnityEngine;
using UnityEngine.UI;

public class Row : MonoBehaviour
{
    private ApplicationUI _applicationUI;

    private void Awake()
    {
        _applicationUI = GetComponentInParent<ApplicationUI>();

        AssignButtonsEvents();
    }

    /// <summary>
    /// Adds events for buttons on row creation
    /// </summary>
    public void AssignButtonsEvents() // adding button event to each new row
    {
        Button[] buttons = GetComponentsInChildren<Button>();

        buttons[0].onClick.AddListener(_applicationUI.RemoveObject);

        if (buttons.Length > 1)
            buttons[1].onClick.AddListener(_applicationUI.CopySelectedObject);
    }
}