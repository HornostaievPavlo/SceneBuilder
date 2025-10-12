using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay;
using Services.SceneObjectSelection;
using UnityEngine;

namespace Services.Painting
{
	public class ModelPaintingService : IModelPaintingService, IDisposable
	{
		private readonly List<Material> _originalMaterials = new();
		private readonly List<Material> _copiedMaterials = new();
		private List<Renderer> _renderers = new();

		private readonly ISceneObjectSelectionService _sceneObjectSelectionService;

		public ModelPaintingService(ISceneObjectSelectionService sceneObjectSelectionService)
		{
			_sceneObjectSelectionService = sceneObjectSelectionService;
			
			_sceneObjectSelectionService.OnObjectSelected += HandleObjectSelected;
			_sceneObjectSelectionService.OnObjectDeselected += HandleObjectDeselected;
		}
		
		public void Dispose()
		{
			_sceneObjectSelectionService.OnObjectSelected -= HandleObjectSelected;
			_sceneObjectSelectionService.OnObjectDeselected -= HandleObjectDeselected;
		}

		public void SetColor(Color color)
		{
			ApplyMaterialsToRenderers();
			
			foreach (Material material in _copiedMaterials)
			{
				material.SetColor(Constants.ColorProperty, color);
			}
		}

		public void SetColorTint(float tintValue)
		{
			float normalizedTintValue = 1f - tintValue;
			
			ApplyMaterialsToRenderers();
			
			Color tintedColor = CreateTintedColor(normalizedTintValue);
			
			foreach (Material material in _copiedMaterials)
			{
				material.SetColor(Constants.ColorProperty, tintedColor);
			}
		}

		public void SetTexture(Texture texture)
		{
			ClearTexturesFromMaterials();
			ApplyMaterialsToRenderers();
			
			foreach (Material material in _copiedMaterials)
			{
				material.SetTexture(Constants.TextureProperty, texture);
			}
		}

		public void RestoreOriginalMaterial()
		{
			RestoreOriginalMaterialsToRenderers();
			RecreateCleanCopiedMaterials();
		}
		
		private void HandleObjectSelected(SceneObject sceneObject)
		{
			CollectRenderersFromSceneObject(sceneObject);
			StoreOriginalMaterials();
			CreateCopiedMaterials();
		}

		private void HandleObjectDeselected()
		{
			ClearAllMaterialCollections();
		}

		private void CollectRenderersFromSceneObject(SceneObject sceneObject)
		{
			_renderers = sceneObject.GetComponentsInChildren<Renderer>().ToList();
		}

		private void StoreOriginalMaterials()
		{
			foreach (Renderer renderer in _renderers)
			{
				_originalMaterials.Add(renderer.material);
			}
		}

		private void CreateCopiedMaterials()
		{
			foreach (Material originalMaterial in _originalMaterials)
			{
				Material materialCopy = new Material(originalMaterial);
				_copiedMaterials.Add(materialCopy);
			}

			AssignNamesToMaterials(_copiedMaterials);
		}

		private void RecreateCleanCopiedMaterials()
		{
			_copiedMaterials.Clear();
			
			foreach (Material originalMaterial in _originalMaterials)
			{
				Material materialCopy = new Material(originalMaterial);
				_copiedMaterials.Add(materialCopy);
			}

			AssignNamesToMaterials(_copiedMaterials);
		}

		private void AssignNamesToMaterials(List<Material> materials)
		{
			for (int i = 0; i < materials.Count; i++)
			{
				materials[i].name = $"Material{i + 1}";
			}
		}

		private void ApplyMaterialsToRenderers()
		{
			foreach (Material material in _copiedMaterials)
			{
				foreach (Renderer renderer in _renderers)
				{
					renderer.material = material;
				}
			}
		}

		private void RestoreOriginalMaterialsToRenderers()
		{
			for (int i = 0; i < _renderers.Count && i < _originalMaterials.Count; i++)
			{
				_renderers[i].material = _originalMaterials[i];
			}
		}

		private void ClearTexturesFromMaterials()
		{
			foreach (Material material in _copiedMaterials)
			{
				material.SetTexture(Constants.TextureProperty, null);
			}
		}

		private void ClearAllMaterialCollections()
		{
			_renderers.Clear();
			_originalMaterials.Clear();
			_copiedMaterials.Clear();
		}

		private Color CreateTintedColor(float tintValue)
		{
			return new Color(
				Color.white.r * tintValue,
				Color.white.g * tintValue,
				Color.white.b * tintValue);
		}
	}
}