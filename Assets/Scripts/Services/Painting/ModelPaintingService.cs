using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay;
using Services.SceneObjectSelection;
using UnityEngine;
using UnityEngine.UI;

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
			
			sceneObjectSelectionService.OnObjectSelected += HandleObjectSelected;
			sceneObjectSelectionService.OnObjectDeselected += HandleObjectDeselected;
		}
		
		public void Dispose()
		{
			_sceneObjectSelectionService.OnObjectSelected -= HandleObjectSelected;
			_sceneObjectSelectionService.OnObjectDeselected -= HandleObjectDeselected;
		}

		public void SetColor(Color color)
		{
			foreach (var material in _copiedMaterials)
			{
				foreach (Renderer renderer in _renderers)
				{
					renderer.material = material;
				}

				material.SetColor(Constants.ColorProperty, color);
			}
		}

		public void SetColorTint(float value)
		{
			value = 1 - value;
			
			foreach (var material in _copiedMaterials)
			{
				foreach (Renderer renderer in _renderers)
				{
					renderer.material = material;
				}

				Color tintedColor = new Color(
					Color.white.r * value,
					Color.white.r * value,
					Color.white.r * value);

				material.SetColor(Constants.ColorProperty, tintedColor);
			}
		}

		public void RestoreOriginalMaterial()
		{
			for (int i = 0; i < _copiedMaterials.Count; i++)
			{
				_renderers[i].material = _originalMaterials[i];
				_renderers[i].material.color = Color.white;
			}
		}

		public void SetTexture(Texture texture)
		{
			DeleteOriginalTextures();

			foreach (var material in _copiedMaterials)
			{
				foreach (Renderer renderer in _renderers)
				{
					renderer.material = material;
				}

				material.SetTexture(Constants.TextureProperty, texture);
			}
		}
		
		private void HandleObjectSelected(SceneObject sceneObject)
		{
			_renderers = sceneObject.GetComponentsInChildren<Renderer>().ToList();

			foreach (Renderer renderer in _renderers)
			{
				_originalMaterials.Add(renderer.material);
			}

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

		private void HandleObjectDeselected()
		{
			_renderers.Clear();
			_originalMaterials.Clear();
			_copiedMaterials.Clear();
		}

		private void DeleteOriginalTextures()
		{
			foreach (var material in _copiedMaterials)
			{
				material.SetTexture(Constants.TextureProperty, null);
			}
		}
	}
}