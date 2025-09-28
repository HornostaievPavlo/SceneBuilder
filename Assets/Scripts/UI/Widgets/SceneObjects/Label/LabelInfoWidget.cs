using System.Collections.Generic;
using System.Linq;
using Enums;
using Gameplay;
using Services.SceneObjectsRegistry;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Widgets.SceneObjects.Label
{
	public class LabelInfoWidget : SceneObjectInfoWidget
	{
		[SerializeField] private TMP_Text titleText;
		[SerializeField] private TMP_Text descriptionText;
		
		[SerializeField] private Button alignToCameraButton;
		
		[Header("Edit Button")]
		[SerializeField] private Button editButton;
		[SerializeField] private Image editButtonImage;
		[SerializeField] private Sprite editButtonRegularSprite;
		[SerializeField] private Sprite editButtonActiveSprite;
		
		private LabelEditWidget _labelEditWidget;
		
		private Camera _mainCamera;
		private Gameplay.Label _label;
		
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
			
			if (_labelEditWidget != null)
				_labelEditWidget.OnClosed -= HandleEditWidgetClosed;
		}

		public override void Setup(SceneObject sceneObject)
		{
			base.Setup(sceneObject);
			_label = CacheLabel();
		}

		public void SetEditWidget(LabelEditWidget editWidget)
		{
			_labelEditWidget = editWidget;
			_labelEditWidget.OnClosed += HandleEditWidgetClosed;
		}

		private void HandleAlignButtonClicked()
		{
			AlignToCamera();
		}

		private void HandleEditWidgetClosed()
		{
			editButtonImage.sprite = editButtonRegularSprite;
			RefreshText();
		}

		private void HandleEditButtonClicked()
		{
			_labelEditWidget.Setup(_label);
			editButtonImage.sprite = editButtonActiveSprite;
		}

		private void RefreshText()
		{
			titleText.text = _label.Title;
			descriptionText.text = _label.Description;
		}

		private void AlignToCamera()
		{
			_label.transform.forward = _mainCamera.transform.forward;
		}

		private Gameplay.Label CacheLabel()
		{
			List<SceneObject> labels = _sceneObjectsRegistry.GetSceneObjects(SceneObjectTypeId.Label);
			SceneObject labelSceneObject = labels.FirstOrDefault(label => label.Id == SceneObject.Id);
			
			return labelSceneObject as Gameplay.Label;
		}
	}
}