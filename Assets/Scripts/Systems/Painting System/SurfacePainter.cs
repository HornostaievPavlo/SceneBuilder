using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SurfacePainter : MonoBehaviour
{
    [HideInInspector] public List<Material> copiedMaterials;

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
            }
            CopyOriginalMaterials();
        }
        else
        {
            targetRenderers.Clear();
            originalMaterials.Clear();
            copiedMaterials.Clear();
        }
    }

    private void CopyOriginalMaterials()
    {
        foreach (Material mat in originalMaterials)
        {
            Material copy = new Material(mat);
            copiedMaterials.Add(copy);
        }

        for (int i = 0; i < copiedMaterials.Count; i++)
        {
            copiedMaterials[i].name = $"Material{i + 1}";
        }
    }

    public void SetColor(Color color)
    {
        foreach (var material in copiedMaterials)
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
        foreach (var material in copiedMaterials)
        {
            foreach (Renderer renderer in targetRenderers)
            {
                renderer.material = material;
            }

            Color tintedColor = new Color(Color.white.r * value,
                                          Color.white.r * value,
                                          Color.white.r * value);

            material.SetColor(COLOR_PROPERTY_NAME, tintedColor);
        }
    }

    public void SetOriginalMaterial(Slider slider)
    {
        slider.value = 1;

        for (int i = 0; i < copiedMaterials.Count; i++)
        {
            targetRenderers[i].material = originalMaterials[i];
            targetRenderers[i].material.color = Color.white;
        }
    }

    public void SetTexture(Texture texture)
    {
        DeleteOriginalTextures();

        foreach (var material in copiedMaterials)
        {
            foreach (Renderer renderer in targetRenderers)
            {
                renderer.material = material;
            }

            material.SetTexture(TEXTURE_PATH, texture);
        }
    }

    private void DeleteOriginalTextures()
    {
        foreach (var material in copiedMaterials)
        {
            material.SetTexture(TEXTURE_PATH, null);
        }
    }
}
