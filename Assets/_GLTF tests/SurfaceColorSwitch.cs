using UnityEngine;

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
        else if (Input.GetKeyDown(KeyCode.A)) // adding tint to texture
        {
            Color multiplier = new Color(defaultColor.r * 0.5f,
                                         defaultColor.g * 0.5f,
                                         defaultColor.b * 0.5f);

            targetMaterial.SetColor(BASE_COLOR, multiplier);
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
