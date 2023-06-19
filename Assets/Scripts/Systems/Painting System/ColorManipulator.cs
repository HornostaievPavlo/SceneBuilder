using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ColorManipulator : MonoBehaviour
{
    [SerializeField]
    private List<Renderer> targetRenderers = new List<Renderer>();

    [SerializeField]
    private List<Material> originalMaterials = new List<Material>();

    [SerializeField]
    private List<Material> targetMaterials;

    private const string COLOR_PROPERTY_NAME = "baseColorFactor";

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

            ///
            material.SetTexture("baseColorTexture", null);
            ///

            material.SetColor(COLOR_PROPERTY_NAME, color);
        }
    }

    public void SetColorTint(Slider slider)
    {
        float tintValue = slider.value;

        foreach (var newMaterial in targetMaterials)
        {
            Color tintedColor = new Color(newMaterial.color.r * tintValue,
                                          newMaterial.color.g * tintValue,
                                          newMaterial.color.b * tintValue);

            newMaterial.SetColor(COLOR_PROPERTY_NAME, tintedColor);

            foreach (Renderer renderer in targetRenderers)
            {
                renderer.material = newMaterial;
            }
        }
    }

    public void ResetColor()
    {
        for (int i = 0; i < targetMaterials.Count; i++)
        {
            targetRenderers[i].material = originalMaterials[i];
        }
    }
}
