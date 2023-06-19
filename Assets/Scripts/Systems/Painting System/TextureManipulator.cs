using UnityEngine;

public class TextureManipulator : MonoBehaviour
{
    [SerializeField] private SelectionSystem _raycastItemSelection;

    [SerializeField] private MeshRenderer targetRenderer;

    [SerializeField] private Material targetMaterial;

    [SerializeField] private Texture originalTexture;

    private const string TEXTURE_PATH = "baseColorTexture";

    public GameObject selectedObject;

    private void Awake()
    {
        targetRenderer = selectedObject.GetComponent<MeshRenderer>();

        targetMaterial = targetRenderer.material;

        originalTexture = targetMaterial.GetTexture(TEXTURE_PATH);
    }

    public void SetTexture(Texture texture)
    {
        targetMaterial.SetTexture(TEXTURE_PATH, texture);
    }

    public void ResetTexture()
    {
        targetMaterial.SetTexture(TEXTURE_PATH, originalTexture);
    }
}
