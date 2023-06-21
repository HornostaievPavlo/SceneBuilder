using RuntimeHandle;
using UnityEngine;
using UnityEngine.UI;

public class ApplicationUI : MonoBehaviour
{
    [Header("Classes")]
    [Space]

    public SelectionSystem selectionSystem;

    [SerializeField] private RuntimeTransformHandle runtimeTransformHandle;

    [SerializeField] private SurfacePainter colorManipulator;

    [SerializeField] private OrbitCameraController orbitCameraController;

    [Header("Objects")]
    [Space]

    [SerializeField] private Slider colorTintSlider;

    [SerializeField] private GameObject colorsButtons;

    [SerializeField] private Transform hideUIToggle;

    private void Start()
    {
        AssignColorsButtons();
    }

    #region MainUI
    /// <summary>
    /// Turns all UI elements on/off 
    /// </summary>
    /// <param name="isActive">Toggle value</param>
    public void SetUIState(bool isActive)
    {
        gameObject.SetActive(isActive);

        hideUIToggle.eulerAngles = new Vector3(0, 0, isActive ? 0 : 180);
    }

    public void CenterCameraFocalPoint()
    {
        orbitCameraController.CenterCameraFocalPoint();
    }

    /// <summary>
    /// Makes a copy of selected object
    /// </summary>
    public void CopySelectedObject()
    {
        if (selectionSystem.selectedObject != null)
        {
            Vector3 copyPosition = new Vector3(0, selectionSystem.selectedObject.transform.position.y, 0);

            Instantiate(selectionSystem.selectedObject.transform,
                        copyPosition,
                        selectionSystem.selectedObject.transform.rotation,
                        selectionSystem.selectedObject.transform.parent);
        }
    }

    /// <summary>
    /// Deletes selected object and its ui row from the scene
    /// </summary>
    public void RemoveObject()
    {
        if (selectionSystem.selectedObject != null)
        {
            Destroy(selectionSystem.selectedObject);

            selectionSystem.ItemSelection(false, selectionSystem.selectedObject.transform);
        }
    }
    #endregion

    #region TransformHandle
    /// <summary>
    /// Changes mode of the TransformHandle to translation 
    /// </summary>
    public void SetPositionType()
    {
        runtimeTransformHandle.type = HandleType.POSITION;
    }

    /// <summary>
    /// Changes mode of the TransformHandle to rotation 
    /// </summary>
    public void SetRotationType()
    {
        runtimeTransformHandle.type = HandleType.ROTATION;
    }

    /// <summary>
    /// Changes mode of the TransformHandle to scaling 
    /// </summary>
    public void SetScaleType()
    {
        runtimeTransformHandle.type = HandleType.SCALE;
    }
    #endregion

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

    public void QuitApplication()
    {
        Application.Quit();
    }
}