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
		[SerializeField] private Button editButton;
		
		private LabelEditWidget _labelEditWidget;
		
		private Camera _mainCamera;
		private Label _label;
		
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
			editButton.onClick.AddListener(HandleEditButtonClicked);
		}
		
		private void OnDisable()
		{
			alignToCameraButton.onClick.RemoveListener(HandleAlignButtonClicked);
			editButton.onClick.RemoveListener(HandleEditButtonClicked);
		}

		public override void Setup(SceneObject sceneObject)
		{
			base.Setup(sceneObject);
			_label = CacheLabel();
		}

		public void SetEditWidget(LabelEditWidget editWidget)
		{
			_labelEditWidget = editWidget;
		}

		private void HandleAlignButtonClicked()
		{
			AlignToCamera();
		}

		private void HandleEditButtonClicked()
		{
			_labelEditWidget.Setup(_label);
		}

		private void AlignToCamera()
		{
			_label.transform.forward = _mainCamera.transform.forward;
		}

		private Label CacheLabel()
		{
			List<SceneObject> labels = _sceneObjectsRegistry.GetSceneObjects(SceneObjectTypeId.Label);
			SceneObject labelSceneObject = labels.FirstOrDefault(label => label.Id == SceneObject.Id);
			
			return labelSceneObject as Label;
		}
	}
}