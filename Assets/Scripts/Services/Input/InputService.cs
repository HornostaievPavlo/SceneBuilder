using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Services.Input
{
	public class InputService : IInputService, IInitializable, ITickable
	{
		private bool _hadFocus;
		private bool _hasTouch;

		private Vector3 _previousMousePosition;
		private Camera _mainCamera;

		private const float MouseRotationRate = 100f;
		private const float MousePanRate = 1f;
		private const float MouseZoomRate = 0.05f;

		public event Action OnTouchBegin;
		public event Action OnTouchEnd;
		public event Action<Vector2> OnPrimaryDrag;
		public event Action<Vector2> OnSecondaryDrag;
		public event Action<float> OnZoom;
		public event Action<RaycastHit> OnRayHit;
		public event Action OnRayMiss;

		public void Initialize()
		{
			_mainCamera = Camera.main;
		}

		public void Tick()
		{
			PollMouse();

			_previousMousePosition = UnityEngine.Input.mousePosition;
			_hadFocus = Application.isFocused;
		}

		private void PollMouse()
		{
			Vector2 delta = Vector2.zero;

			if (_hadFocus)
			{
				delta = NormalizeScreenVector(UnityEngine.Input.mousePosition - _previousMousePosition);
			}

			if (UnityEngine.Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
			{
				Raycast(UnityEngine.Input.mousePosition);
			}

			if (UnityEngine.Input.GetMouseButton(0) == false)
			{
				EndTouch();
			}

			if (UnityEngine.Input.GetMouseButton(1))
			{
				BeginTouch();
				OnPrimaryDrag?.Invoke(delta * MouseRotationRate);
			}
			else if (UnityEngine.Input.GetMouseButton(2))
			{
				OnSecondaryDrag?.Invoke(delta * MousePanRate);
			}
			else if (UnityEngine.Input.mouseScrollDelta.y != 0f)
			{
				OnZoom?.Invoke(UnityEngine.Input.mouseScrollDelta.y * MouseZoomRate);
			}
		}

		private Vector2 NormalizeScreenVector(Vector2 vector)
		{
			return vector / Screen.currentResolution.height;
		}

		private void Raycast(Vector2 screenPosition)
		{
			Ray ray = _mainCamera.ScreenPointToRay(screenPosition);

			if (Physics.Raycast(ray, out RaycastHit hit, _mainCamera.farClipPlane))
			{
				OnRayHit?.Invoke(hit);
			}
			else
			{
				OnRayMiss?.Invoke();
			}
		}

		private void EndTouch()
		{
			if (_hasTouch == false)
				return;

			_hasTouch = false;
			OnTouchEnd?.Invoke();
		}

		private void BeginTouch()
		{
			if (_hasTouch)
				return;

			_hasTouch = true;
			OnTouchBegin?.Invoke();
		}
	}
}