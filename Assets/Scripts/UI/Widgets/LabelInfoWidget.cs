using System.Collections.Generic;
using System.Linq;
using Enums;
using Gameplay;
using Services.SceneObjectsRegistry;
using UI.Widgets.SceneObjects;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Widgets
{
	public class LabelInfoWidget : SceneObjectInfoWidget
	{
		[SerializeField] private Button alignToCameraButton;
		
		private Camera _mainCamera;
		
		private ISceneObjectsRegistry _sceneObjectsRegistry;

		[Inject]
		private void Construct(ISceneObjectsRegistry sceneObjectsRegistry)
		{
			_sceneObjectsRegistry = sceneObjectsRegistry;
		}

		private void Awake()
		{
			_mainCamera = Camera.main;
		}

		private void OnEnable()
		{
			alignToCameraButton.onClick.AddListener(HandleAlignButtonClicked);
		}
		
		private void OnDisable()
		{
			alignToCameraButton.onClick.RemoveListener(HandleAlignButtonClicked);
		}

		private void HandleAlignButtonClicked()
		{
			AlignToCamera();
		}

		private void AlignToCamera()
		{
			SceneObject targetLabel = GetTargetLabel();
			targetLabel.transform.forward = _mainCamera.transform.forward;
		}

		private SceneObject GetTargetLabel()
		{
			List<SceneObject> labels = _sceneObjectsRegistry.GetSceneObjects(SceneObjectTypeId.Label);
			return labels.FirstOrDefault(label => label.Id == SceneObject.Id);
		}
	}
}