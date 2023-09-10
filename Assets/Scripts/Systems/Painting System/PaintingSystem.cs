using UnityEngine;
using UnityEngine.UI;

public class PaintingSystem : MonoBehaviour
{
    private SurfacePainter surfacePainter;

    [SerializeField] private Slider colorTintSlider;

    [SerializeField] private GameObject colorsButtonsParent;

    private void Start()
    {
        surfacePainter = GetComponent<SurfacePainter>();

        //AssignColorsButtons();
    }

    private void AssignColorsButtons()
    {
        Button[] buttons = colorsButtonsParent.GetComponentsInChildren<Button>(true);

        buttons[0].onClick.AddListener(() => surfacePainter.SetColor(Color.red));
        buttons[1].onClick.AddListener(() => surfacePainter.SetColor(new Color(1, 0, 0.5f, 1))); //pink
        buttons[2].onClick.AddListener(() => surfacePainter.SetColor(new Color(1, 0.5f, 0, 1))); //orange
        buttons[3].onClick.AddListener(() => surfacePainter.SetColor(new Color(1, 1, 0, 1))); //yellow
        buttons[4].onClick.AddListener(() => surfacePainter.SetColor(Color.green));
        buttons[5].onClick.AddListener(() => surfacePainter.SetColor(new Color(0.75f, 1, 0.75f, 1))); //lgreen
        buttons[6].onClick.AddListener(() => surfacePainter.SetColor(Color.blue)); //blue
        buttons[7].onClick.AddListener(() => surfacePainter.SetColor(new Color(0.5f, 0.7f, 1, 1))); //lblue
        buttons[8].onClick.AddListener(() => surfacePainter.SetColor(new Color(0, 0, 0.5f, 1))); //violet 
        buttons[9].onClick.AddListener(() => surfacePainter.SetColor(new Color(0.5f, 0.5f, 0.75f, 1))); //purple
        buttons[10].onClick.AddListener(() => surfacePainter.SetColor(Color.black));
        buttons[11].onClick.AddListener(() => surfacePainter.SetColor(Color.white));
    }

    public void SetColorTint()
    {
        surfacePainter.SetColorTint(colorTintSlider);
    }

    public void ResetColor()
    {
        surfacePainter.ResetColor();
    }
}
