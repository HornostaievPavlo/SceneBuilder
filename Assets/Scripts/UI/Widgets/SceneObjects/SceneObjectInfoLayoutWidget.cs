using System.Collections.Generic;
using System.Linq;
using Enums;
using Gameplay;
using Services.Instantiation;
using Services.SceneObjectSelection;
using Services.SceneObjectsRegistry;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Widgets.SceneObjects
{
	public class SceneObjectInfoLayoutWidget : MonoBehaviour
	{
		[SerializeField] private SceneObjectTypeId sceneObjectTypeId;

		[SerializeField] private GameObject infoWidgetPrefab;
		[SerializeField] private Transform infoWidgetsParent;

		private readonly List<SceneObjectInfoWidget> _infoWidgets = new();

		private Color32 selectedRowColor = new Color32(63, 106, 204, 255);
		private Color32 unselectedRowColor = new Color32(221, 223, 229, 255);

		private ISceneObjectsRegistry _sceneObjectsRegistry;
		private ISceneObjectSelectionService _sceneObjectSelectionService;
		private IInstantiateService _instantiateService;

		[Inject]
		private void Construct(
			ISceneObjectsRegistry sceneObjectsRegistry,
			ISceneObjectSelectionService sceneObjectSelectionService,
			IInstantiateService instantiateService)
		{
			_sceneObjectsRegistry = sceneObjectsRegistry;
			_sceneObjectSelectionService = sceneObjectSelectionService;
			_instantiateService = instantiateService;
		}

		private void OnEnable()
		{
			_sceneObjectsRegistry.OnObjectRegistered += HandleObjectRegistered;
			_sceneObjectsRegistry.OnObjectUnregistered += HandleObjectUnregistered;
			_sceneObjectSelectionService.OnObjectSelected += HandleObjectSelected;
			_sceneObjectSelectionService.OnObjectDeselected += HandleObjectDeselected;

			Setup();
		}

		private void OnDisable()
		{
			_sceneObjectsRegistry.OnObjectRegistered -= HandleObjectRegistered;
			_sceneObjectsRegistry.OnObjectUnregistered -= HandleObjectUnregistered;
			_sceneObjectSelectionService.OnObjectSelected -= HandleObjectSelected;
			_sceneObjectSelectionService.OnObjectDeselected -= HandleObjectDeselected;
		}

		private void HandleObjectRegistered(SceneObject sceneObject)
		{
			CreateInfoWidget(sceneObject);
		}

		private void HandleObjectUnregistered(SceneObject sceneObject)
		{
			SceneObjectInfoWidget infoWidget = _infoWidgets.FirstOrDefault(widget => widget.GetSceneObjectId() == sceneObject.Id);
			DeleteInfoWidget(infoWidget);
		}

		private void HandleObjectSelected(SceneObject scene)
		{
			// HighlightRow(scene.MenuRow, true);
		}

		private void HandleObjectDeselected()
		{
			HighlightRow(null, false);
		}

		private void Setup()
		{
			Cleanup();
			SetupWidgets();
		}

		private void Cleanup()
		{
			foreach (SceneObjectInfoWidget infoWidget in _infoWidgets)
			{
				DeleteInfoWidget(infoWidget);
			}

			_infoWidgets.Clear();
		}

		private void SetupWidgets()
		{
			List<SceneObject> sceneObjects = _sceneObjectsRegistry.GetSceneObjects(sceneObjectTypeId);

			foreach (SceneObject sceneObject in sceneObjects)
			{
				CreateInfoWidget(sceneObject);
			}
		}

		protected virtual SceneObjectInfoWidget CreateInfoWidget(SceneObject sceneObject)
		{
			var infoWidget = _instantiateService.Instantiate<SceneObjectInfoWidget>(infoWidgetPrefab, infoWidgetsParent);
			
			_infoWidgets.Add(infoWidget);
			infoWidget.Setup(sceneObject);

			// AssignRowsNumbers(rows);

			return infoWidget;
		}

		private void DeleteInfoWidget(SceneObjectInfoWidget infoWidget)
		{
			Destroy(infoWidget.gameObject);

			// AssignRowsNumbers(rows);
		}

		private void AssignRowsNumbers(List<SceneObjectInfoWidget> rows)
		{
			foreach (var row in rows)
			{
				TMP_Text text = row.GetComponentInChildren<TMP_Text>();
				text.text = (rows.IndexOf(row) + 1).ToString();
			}
		}

		private void HighlightRow(SceneObjectInfoWidget sceneObjectInfoWidget, bool isSelected)
		{
			if (sceneObjectInfoWidget != null)
			{
				DeselectAllRows();
				GetHighlightDots(sceneObjectInfoWidget).color = isSelected ? selectedRowColor : unselectedRowColor;
			}
			else
			{
				DeselectAllRows();
			}
		}

		private void DeselectAllRows()
		{
			foreach (SceneObjectInfoWidget infoWidget in _infoWidgets)
			{
				GetHighlightDots(infoWidget).color = unselectedRowColor;
			}
		}

		private Image GetHighlightDots(SceneObjectInfoWidget sceneObjectInfoWidget)
		{
			Image[] images = sceneObjectInfoWidget.GetComponentsInChildren<Image>();
			Image highlightningDots = images[1];

			return highlightningDots;
		}
	}
}