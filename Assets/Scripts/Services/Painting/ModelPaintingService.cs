using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Services.Painting
{
	public class ModelPaintingService : IModelPaintingService
	{
		private readonly List<Material> _copiedMaterials = new();

		private List<Renderer> _targetRenderers = new();

		private readonly List<Material> _originalMaterials = new();

		private const string COLOR_PROPERTY_NAME = "baseColorFactor";
		private const string TEXTURE_PATH = "baseColorTexture";

		public void CacheModelMaterials(GameObject target, bool isSelected)
		{
			if (isSelected)
			{
				_targetRenderers = target.GetComponentsInChildren<Renderer>().ToList();

				foreach (Renderer renderer in _targetRenderers)
				{
					_originalMaterials.Add(renderer.material);
				}

				CopyOriginalMaterials();
			}
			else
			{
				_targetRenderers.Clear();
				_originalMaterials.Clear();
				_copiedMaterials.Clear();
			}
		}

		private void CopyOriginalMaterials()
		{
			foreach (Material mat in _originalMaterials)
			{
				Material copy = new Material(mat);
				_copiedMaterials.Add(copy);
			}

			for (int i = 0; i < _copiedMaterials.Count; i++)
			{
				_copiedMaterials[i].name = $"Material{i + 1}";
			}
		}

		public void SetColor(Color color)
		{
			foreach (var material in _copiedMaterials)
			{
				foreach (Renderer renderer in _targetRenderers)
				{
					renderer.material = material;
				}

				material.SetColor(COLOR_PROPERTY_NAME, color);
			}
		}

		public void SetColorTint(float value)
		{
			foreach (var material in _copiedMaterials)
			{
				foreach (Renderer renderer in _targetRenderers)
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

			for (int i = 0; i < _copiedMaterials.Count; i++)
			{
				_targetRenderers[i].material = _originalMaterials[i];
				_targetRenderers[i].material.color = Color.white;
			}
		}

		public void SetTexture(Texture texture)
		{
			DeleteOriginalTextures();

			foreach (var material in _copiedMaterials)
			{
				foreach (Renderer renderer in _targetRenderers)
				{
					renderer.material = material;
				}

				material.SetTexture(TEXTURE_PATH, texture);
			}
		}

		private void DeleteOriginalTextures()
		{
			foreach (var material in _copiedMaterials)
			{
				material.SetTexture(TEXTURE_PATH, null);
			}
		}
	}
}