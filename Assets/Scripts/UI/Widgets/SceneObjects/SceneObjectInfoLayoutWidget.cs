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

		private readonly List<SceneObjectInfoWidget> _infoWidgets = new();

		private ISceneObjectsRegistry _sceneObjectsRegistry;
		private IInstantiateService _instantiateService;

		[Inject]
		private void Construct(
			ISceneObjectsRegistry sceneObjectsRegistry,
			IInstantiateService instantiateService)
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
		}

		private void HandleObjectUnregistered(SceneObject sceneObject)
		{
			SceneObjectInfoWidget infoWidget = _infoWidgets.FirstOrDefault(widget => widget.GetSceneObjectId() == sceneObject.Id);
			DeleteInfoWidget(infoWidget);
			RefreshWidgetsNumbers();
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
			
			int widgetNumber = _infoWidgets.IndexOf(infoWidget) + 1;
			infoWidget.Setup(sceneObject, widgetNumber);

			return infoWidget;
		}

		private void DeleteInfoWidget(SceneObjectInfoWidget infoWidget)
		{
			Destroy(infoWidget.gameObject);
		}
		
		private void RefreshWidgetsNumbers()
		{
			for (int i = 0; i < _infoWidgets.Count; i++)
			{
				_infoWidgets[i].SetNumber(i + 1);
			}
		}
	}
}