using Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class PaintingSystem : MonoBehaviour
{
    [SerializeField]
    private Slider colorTintSlider;

    [SerializeField]
    private GameObject colorsButtonsParent;

    private SurfacePainter surfacePainter;

    private void Start()
    {
        surfacePainter = GetComponent<SurfacePainter>();

        colorTintSlider.onValueChanged.AddListener(surfacePainter.SetColorTint);
        AssignColorsButtons();
    }

    private void OnEnable()
    {
        // SelectionSystem.OnObjectSelected += OnObjectSelected;
        // SelectionSystem.OnObjectDeselected += OnObjectDeselected;
    }

    private void OnDisable()
    {
        // SelectionSystem.OnObjectSelected -= OnObjectSelected;
        // SelectionSystem.OnObjectDeselected -= OnObjectDeselected;
    }

    private void OnObjectSelected(SceneObject scene)
    {
        surfacePainter.GetAllModelMaterials(scene.transform.gameObject, true);
    }

    private void OnObjectDeselected()
    {
        surfacePainter.GetAllModelMaterials(null, false);
    }

    private void AssignColorsButtons()
    {
        Button[] buttons = colorsButtonsParent.GetComponentsInChildren<Button>(true);

        buttons[0].onClick.AddListener(() => surfacePainter.SetColor(Color.red));
        buttons[1].onClick.AddListener(() => surfacePainter.SetColor(new Color(1, 0.7019f, 0.7764f, 1))); //pink
        buttons[2].onClick.AddListener(() => surfacePainter.SetColor(new Color(1, 0.5f, 0, 1))); //orange
        buttons[3].onClick.AddListener(() => surfacePainter.SetColor(new Color(1, 1, 0, 1))); //yellow
        buttons[4].onClick.AddListener(() => surfacePainter.SetColor(Color.green));
        buttons[5].onClick.AddListener(() => surfacePainter.SetColor(new Color(0.75f, 1, 0.75f, 1))); //lgreen
        buttons[6].onClick.AddListener(() => surfacePainter.SetColor(Color.blue)); //blue
        buttons[7].onClick.AddListener(() => surfacePainter.SetColor(new Color(0.5f, 0.7f, 1, 1))); //lblue
        buttons[8].onClick.AddListener(() => surfacePainter.SetColor(new Color(0.6f, 0.3458f, 0.1647f, 1))); //brown 
        buttons[9].onClick.AddListener(() => surfacePainter.SetColor(new Color(0.7843f, 0.7137f, 1, 1))); //purple
        buttons[10].onClick.AddListener(() => surfacePainter.SetColor(Color.black));
        buttons[11].onClick.AddListener(() => surfacePainter.SetColor(Color.white));
    }
}
