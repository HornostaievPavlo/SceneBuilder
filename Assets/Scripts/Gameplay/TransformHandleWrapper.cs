using System.Collections;
using DG.Tweening;
using RuntimeHandle;
using Services.SceneObjectSelection;
using Services.SceneObjectsRegistry;
using UnityEngine;
using Zenject;

namespace Gameplay
{
	public class TransformHandleWrapper : MonoBehaviour
	{
		[SerializeField] private RuntimeTransformHandle handle;
		[SerializeField] LayerMask gizmoLayerMask;

		private int _gizmoLayerIndex;

		private ISceneObjectSelectionService _sceneObjectSelectionService;
		private ISceneObjectsRegistry _sceneObjectsRegistry;

		[Inject]
		private void Construct(
			ISceneObjectSelectionService sceneObjectSelectionService, 
			ISceneObjectsRegistry sceneObjectsRegistry)
		{
			_sceneObjectSelectionService = sceneObjectSelectionService;
			_sceneObjectsRegistry = sceneObjectsRegistry;
		}

		private void Awake()
		{
			_gizmoLayerIndex = GetLayerIndex();
		}

		private void OnEnable()
		{
			_sceneObjectSelectionService.OnObjectSelected += HandleObjectSelected;
			_sceneObjectSelectionService.OnObjectDeselected += HandleObjectDeselected;
			
			_sceneObjectsRegistry.OnObjectUnregistered += HandleObjectUnregistered;
		}

		private void OnDisable()
		{
			_sceneObjectSelectionService.OnObjectSelected -= HandleObjectSelected;
			_sceneObjectSelectionService.OnObjectDeselected -= HandleObjectDeselected;
			
			_sceneObjectsRegistry.OnObjectUnregistered -= HandleObjectUnregistered;
		}

		public void SetType(HandleType type)
		{
			handle.type = type;
			StartCoroutine(RefreshGizmoLayer());
		}

		private void HandleObjectSelected(SceneObject sceneObject)
		{
			handle.gameObject.SetActive(true);
			handle.target = sceneObject.transform;

			AnimateGizmoAppear();
			StartCoroutine(RefreshGizmoLayer());
		}

		private void HandleObjectDeselected()
		{
			handle.gameObject.SetActive(false);
			handle.target = null;
		}

		private void HandleObjectUnregistered(SceneObject sceneObject)
		{
			HandleObjectDeselected();
		}

		private IEnumerator RefreshGizmoLayer()
		{
			yield return null;

			Transform[] children = handle.GetComponentsInChildren<Transform>();

			foreach (Transform child in children)
			{
				child.gameObject.layer = _gizmoLayerIndex;
			}
		}

		private int GetLayerIndex()
		{
			int value = gizmoLayerMask.value;

			for (int i = 0; i < 32; i++)
			{
				if ((value & 1 << i) != 0)
					return i;
			}

			return 0;
		}

		private void AnimateGizmoAppear()
		{
			transform.DOKill(true);
			transform.DOPunchScale(handle.transform.localScale * 0.9f, 0.2f);
		}
	}
}