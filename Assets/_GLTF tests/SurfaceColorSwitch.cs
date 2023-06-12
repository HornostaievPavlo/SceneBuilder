using UnityEngine;
using UnityEngine.UI;

public class SurfaceColorSwitch : MonoBehaviour
{
    [SerializeField] private GameObject target;

    [SerializeField] private MeshRenderer targetRenderer;

    [SerializeField] private Material targetMaterial;

    [SerializeField] private Color defaultColor;

    [SerializeField] private Color targetColor;

    [SerializeField] private Texture defaultTexture;

    private const string BASE_COLOR = "baseColorFactor";
    private const string TEXTURE = "baseColorTexture";

    public Slider tintSlider;

    private void Awake()
    {
        targetRenderer = target.GetComponent<MeshRenderer>();

        targetMaterial = targetRenderer.material;

        defaultColor = targetMaterial.color;

        defaultTexture = targetMaterial.GetTexture(TEXTURE);


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            targetMaterial.SetColor(BASE_COLOR, targetColor); //glTF

            //targetMaterial.SetColor("_Color", targetColor); //standard
        }
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z))
        {
            targetMaterial.SetColor(BASE_COLOR, defaultColor);
        }
        else if (Input.GetKeyDown(KeyCode.T)) // adding tint to texture
        {
            //Color tintedColor = new Color(defaultColor.r * tintTest,
            //                              defaultColor.g * tintTest,
            //                              defaultColor.b * tintTest);


            float tintTest2 = tintSlider.value;

            Debug.Log("na base " + tintTest2);

            Color tintedColor = new Color(defaultColor.r * tintTest2,
                                          defaultColor.g * tintTest2,
                                          defaultColor.b * tintTest2);

            targetMaterial.SetColor(BASE_COLOR, tintedColor);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            targetMaterial.SetTexture(TEXTURE, null);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            targetMaterial.SetTexture(TEXTURE, defaultTexture);
        }
    }
}








/*
1) color picker на все цвета 
поставить палитру 
и смотреть по ray cast в UV coordinate, потом в цвет пикселя

2) оттенки множители в Color - 48
 */
