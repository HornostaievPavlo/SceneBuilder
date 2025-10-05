using System;
using Gameplay;
using Services.Input;
using UnityEngine;
using Zenject;

namespace Services.SceneObjectSelection
{
	public class SceneObjectSelectionService : ISceneObjectSelectionService, IInitializable, IDisposable
	{
		private SceneObject _selectedObject;
		
		private IInputService _inputService;

		public event Action<SceneObject> OnObjectSelected;
		public event Action OnObjectDeselected;
		
		public SceneObject SelectedObject => _selectedObject;

		[Inject]
		private void Construct(IInputService inputService)
		{
			_inputService = inputService;
		}
		
		public void Initialize()
		{
			_inputService.OnRayHit += HandleRayHit;
			_inputService.OnRayMiss += HandleRayMiss;
		}

		public void Dispose()
		{
			_inputService.OnRayHit -= HandleRayHit;
			_inputService.OnRayMiss -= HandleRayMiss;
		}

		private void HandleRayHit(RaycastHit hit)
		{
			var sceneObject = hit.transform.gameObject.GetComponentInParent<SceneObject>();

			if (sceneObject == null || sceneObject == _selectedObject)
				return;
			
			_selectedObject = sceneObject;
			OnObjectSelected?.Invoke(sceneObject);
		}

		private void HandleRayMiss()
		{
			_selectedObject = null;
			OnObjectDeselected?.Invoke();
		}
	}
}