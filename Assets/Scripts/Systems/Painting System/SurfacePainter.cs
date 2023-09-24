using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SurfacePainter : MonoBehaviour
{
    [SerializeField]
    private List<Material> targetMaterials;

    private List<Renderer> targetRenderers = new List<Renderer>();

    private List<Material> originalMaterials = new List<Material>();

    private const string COLOR_PROPERTY_NAME = "baseColorFactor";

    private const string TEXTURE_PATH = "baseColorTexture";

    public void GetAllModelMaterials(GameObject target, bool isSelected)
    {
        if (isSelected)
        {
            targetRenderers = target.GetComponentsInChildren<Renderer>().ToList();

            foreach (Renderer renderer in targetRenderers)
            {
                originalMaterials.Add(renderer.material);

                CopyOriginalMaterials();
            }
        }
        else
        {
            targetRenderers.Clear();
            originalMaterials.Clear();
            targetMaterials.Clear();
        }
    }

    private void CopyOriginalMaterials()
    {
        foreach (Material mat in originalMaterials)
        {
            Material copy = new Material(mat);

            targetMaterials.Add(copy);
        }

        for (int i = 0; i < targetMaterials.Count; i++)
        {
            targetMaterials[i].name = $"Material {i}";
        }
    }

    public void SetColor(Color color)
    {
        foreach (var material in targetMaterials)
        {
            foreach (Renderer renderer in targetRenderers)
            {
                renderer.material = material;
            }

            material.SetColor(COLOR_PROPERTY_NAME, color);
        }
    }

    public void SetColorTint(float value)
    {
        Color originalColorCopy = originalMaterials[0].color;

        foreach (var material in targetMaterials)
        {
            foreach (Renderer renderer in targetRenderers)
            {
                renderer.material = material;
            }

            Color tintedColor = new Color(originalColorCopy.r * value,
                                          originalColorCopy.g * value,
                                          originalColorCopy.b * value);

            material.SetColor(COLOR_PROPERTY_NAME, tintedColor);
        }
    }

    public void ResetColor()
    {
        for (int i = 0; i < targetMaterials.Count; i++)
        {
            targetRenderers[i].material = originalMaterials[i];

            targetRenderers[i].material.color = Color.white;
        }
    }

    public void SetTexture(Texture texture)
    {
        DeleteOriginalTextures();

        foreach (var material in targetMaterials)
        {
            foreach (Renderer renderer in targetRenderers)
            {
                renderer.material = material;
            }

            material.SetTexture(TEXTURE_PATH, texture);
        }
    }

    public void DeleteOriginalTextures()
    {
        foreach (var material in targetMaterials)
        {
            material.SetTexture(TEXTURE_PATH, null);
        }
    }
}
