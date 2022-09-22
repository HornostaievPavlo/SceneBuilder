using UnityEngine;
using UnityEngine.UI;

public class Row : MonoBehaviour
{
    private ApplicationUI ApplicationUI;

    private void Awake()
    {
        ApplicationUI = GetComponentInParent<ApplicationUI>();

        AssignButtonsEvents();
    }

    public void AssignButtonsEvents() // adding button event to each new row
    {
        Button[] buttons = GetComponentsInChildren<Button>();

        buttons[0].onClick.AddListener(ApplicationUI.RemoveObject);

        if (buttons.Length > 1) buttons[1].onClick.AddListener(ApplicationUI.CopySelectedObject);
    }
}