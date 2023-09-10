using UnityEngine;
using UnityEngine.UI;

public class ApplicationUI : MonoBehaviour
{
    [Header("Classes")]
    [Space]

    //public SelectionSystem selectionSystem;


    [SerializeField] private SurfacePainter colorManipulator;

    //[SerializeField] private OrbitCameraController orbitCameraController;

    [Header("Objects")]
    [Space]

    [SerializeField] private Slider colorTintSlider;

    [SerializeField] private GameObject colorsButtons;

    [SerializeField] private Transform hideUIToggle;

    /// <summary>
    /// Implementing selection event
    /// </summary>
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

    }

    private void OnObjectDeselected()
    {

    }

    private void Start()
    {
        //AssignColorsButtons();
    }

    //public void CenterCameraFocalPoint()
    //{
    //    orbitCameraController.CenterCameraFocalPoint();
    //}


    //public void CopySelectedObject()
    //{
    //    if (selectionSystem.selectedObject != null)
    //    {
    //        Vector3 copyPosition = new Vector3(0, selectionSystem.selectedObject.transform.position.y, 0);

    //        var copy = Instantiate(selectionSystem.selectedObject.transform,
    //                    copyPosition,
    //                    selectionSystem.selectedObject.transform.rotation,
    //                    selectionSystem.selectedObject.transform.parent);

    //        copy.gameObject.name = selectionSystem.selectedObject.name;
    //    }
    //}


    //public void RemoveObject()
    //{
    //    if (selectionSystem.selectedObject != null)
    //    {
    //        Destroy(selectionSystem.selectedObject);

    //        selectionSystem.ItemSelection(false, selectionSystem.selectedObject.transform);
    //    }
    //}

    #region PaintingSystem

    private void AssignColorsButtons()
    {
        Button[] buttons = colorsButtons.GetComponentsInChildren<Button>(true);

        buttons[0].onClick.AddListener(() => colorManipulator.SetColor(Color.red));
        buttons[1].onClick.AddListener(() => colorManipulator.SetColor(new Color(1, 0, 0.5f, 1))); //pink
        buttons[2].onClick.AddListener(() => colorManipulator.SetColor(new Color(1, 0.5f, 0, 1))); //orange
        buttons[3].onClick.AddListener(() => colorManipulator.SetColor(new Color(1, 1, 0, 1))); //yellow
        buttons[4].onClick.AddListener(() => colorManipulator.SetColor(Color.green));
        buttons[5].onClick.AddListener(() => colorManipulator.SetColor(new Color(0.75f, 1, 0.75f, 1))); //lgreen
        buttons[6].onClick.AddListener(() => colorManipulator.SetColor(Color.blue)); //blue
        buttons[7].onClick.AddListener(() => colorManipulator.SetColor(new Color(0.5f, 0.7f, 1, 1))); //lblue
        buttons[8].onClick.AddListener(() => colorManipulator.SetColor(new Color(0, 0, 0.5f, 1))); //violet 
        buttons[9].onClick.AddListener(() => colorManipulator.SetColor(new Color(0.5f, 0.5f, 0.75f, 1))); //purple
        buttons[10].onClick.AddListener(() => colorManipulator.SetColor(Color.black));
        buttons[11].onClick.AddListener(() => colorManipulator.SetColor(Color.white));
    }

    public void SetColorTint()
    {
        colorManipulator.SetColorTint(colorTintSlider);
    }

    public void ResetColor()
    {
        colorManipulator.ResetColor();
    }
    #endregion

    public void SetUIActive(bool isActive)
    {
        gameObject.SetActive(isActive);

        hideUIToggle.eulerAngles = new Vector3(0, 0, isActive ? 0 : 180);
    }

    public void QuitApplication() => Application.Quit();
}