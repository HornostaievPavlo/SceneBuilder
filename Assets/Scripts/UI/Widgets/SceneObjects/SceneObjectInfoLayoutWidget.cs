using System.Collections.Generic;
using System.Linq;
using Enums;
using Gameplay;
using Services.Instantiation;
using Services.SceneObjectsRegistry;
using UnityEngine;
using Zenject;

namespace UI.Widgets.SceneObjects
{
	public class SceneObjectInfoLayoutWidget : MonoBehaviour
	{
		[SerializeField] private SceneObjectTypeId sceneObjectTypeId;

		[SerializeField] private GameObject infoWidgetPrefab;
		[SerializeField] private Transform infoWidgetsParent;

		[SerializeField] private GameObject emptyLayoutInfo;

		private readonly List<SceneObjectInfoWidget> _infoWidgets = new();

		private ISceneObjectsRegistry _sceneObjectsRegistry;
		private IInstantiateService _instantiateService;

		[Inject]
		private void Construct(ISceneObjectsRegistry sceneObjectsRegistry, IInstantiateService instantiateService)
		{
			_sceneObjectsRegistry = sceneObjectsRegistry;
			_instantiateService = instantiateService;
		}

		private void OnEnable()
		{
			_sceneObjectsRegistry.OnObjectRegistered += HandleObjectRegistered;
			_sceneObjectsRegistry.OnObjectUnregistered += HandleObjectUnregistered;

			Setup();
		}

		private void OnDisable()
		{
			_sceneObjectsRegistry.OnObjectRegistered -= HandleObjectRegistered;
			_sceneObjectsRegistry.OnObjectUnregistered -= HandleObjectUnregistered;
		}

		private void HandleObjectRegistered(SceneObject sceneObject)
		{
			CreateInfoWidget(sceneObject);
			RefreshEmptyLayoutInfoObject();
		}

		private void HandleObjectUnregistered(SceneObject sceneObject)
		{
			SceneObjectInfoWidget infoWidget = _infoWidgets.FirstOrDefault(widget => widget.GetSceneObjectId() == sceneObject.Id);
			DeleteInfoWidget(infoWidget);
			RefreshWidgetsNumbers();
			RefreshEmptyLayoutInfoObject();
		}

		private void Setup()
		{
			Cleanup();
			SetupWidgets();
			RefreshWidgetsNumbers();
			RefreshEmptyLayoutInfoObject();
		}

		private void Cleanup()
		{
			for (var i = _infoWidgets.Count - 1; i >= 0; i--)
			{
				SceneObjectInfoWidget infoWidget = _infoWidgets[i];
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
			
			int widgetNumber = _infoWidgets.IndexOf(infoWidget) + 1;
			infoWidget.Setup(sceneObject, widgetNumber);

			return infoWidget;
		}

		private void DeleteInfoWidget(SceneObjectInfoWidget infoWidget)
		{
			if (infoWidget == null)
				return;
			
			_infoWidgets.Remove(infoWidget);
			Destroy(infoWidget.gameObject);
		}

		private void RefreshWidgetsNumbers()
		{
			for (int i = 0; i < _infoWidgets.Count; i++)
			{
				_infoWidgets[i].SetNumber(i + 1);
			}
		}

		private void RefreshEmptyLayoutInfoObject()
		{
			emptyLayoutInfo.SetActive(_infoWidgets.Count == 0);
		}
	}
}