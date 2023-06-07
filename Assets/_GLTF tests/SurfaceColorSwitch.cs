using UnityEngine;

public class SurfaceColorSwitch : MonoBehaviour
{
    [SerializeField] private GameObject target;

    [SerializeField] private MeshRenderer targetRenderer;

    [SerializeField] private Material targetMaterial;

    [SerializeField] private Color defaultColor;

    [SerializeField] private Color targetColor;

    private void Awake()
    {
        targetRenderer = target.GetComponent<MeshRenderer>();

        targetMaterial = targetRenderer.material;

        defaultColor = targetMaterial.color;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            targetMaterial.SetColor("baseColorFactor", targetColor); //glTF

            //targetMaterial.SetColor("_Color", targetColor); //standard
        }
    }
}
