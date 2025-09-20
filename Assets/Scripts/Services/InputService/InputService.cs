using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Services.InputService
{
	public class InputService : IInputService, IInitializable, ITickable
	{
		private const float AxisRotationRate = 100f;
		private const float AxisPanRate = 1f;
		
		private const float MouseRotationRate = 100f;
		private const float MousePanRate = 1f;
		private const float MouseZoomRate = 0.05f;

		public event Action TouchBeginAction = delegate { };
		public event Action TouchReleaseAction = delegate { };

		public event Action<Vector2> SecondaryDragAction = delegate { };
		public event Action<Vector2> PrimaryDragAction = delegate { };
		public event Action<float> ZoomAction = delegate { };

		public event Action<RaycastHit> RayHit;
		public event Action RayMiss;

		public Vector2 InputScreenSpacePosition { get; private set; }

		private bool hadFocus;
		private bool hasTouch;
		private Vector3 previousMousePosition;
		private Camera mainCamera;
		
		public void Initialize()
		{
			mainCamera = Camera.main;
		}

		public void Tick()
		{
			PollMouse();

			previousMousePosition = Input.mousePosition;
			hadFocus = Application.isFocused;
		}

		/// <summary>
		/// Process mouse input.
		/// </summary>
		private void PollMouse()
		{
			InputScreenSpacePosition = Input.mousePosition;
			Vector2 delta = Vector2.zero;

			// If window was just refocused, previous mouse position will be invalid
			// so keep delta 0 unless we had focus for at least 1 frame
			if (hadFocus)
				delta = NormalizeScreenVector(Input.mousePosition - previousMousePosition);

			if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
				Raycast(Input.mousePosition);

			if (!Input.GetMouseButton(0))
				EndTouch();

			if (Input.GetMouseButton(1))
			{
				BeginTouch();
				PrimaryDragAction(delta * MouseRotationRate);
			}

			else if (Input.GetMouseButton(2))
				SecondaryDragAction(delta * MousePanRate);
			else if (Input.mouseScrollDelta.y != 0f)
				ZoomAction(Input.mouseScrollDelta.y * MouseZoomRate);
		}

		private void BeginTouch()
		{
			if (hasTouch) return;

			hasTouch = true;
			TouchBeginAction();
		}

		private void EndTouch()
		{
			if (!hasTouch) return;

			hasTouch = false;
			TouchReleaseAction();
		}

		/// <summary>
		/// Normalize a screen space vector by dividing it by native screen height.
		/// </summary>
		private Vector2 NormalizeScreenVector(Vector2 v)
		{
			int height = Screen.currentResolution.height;
			return v / height;
		}

		/// <summary>
		/// Applies panning from axis values.
		/// </summary>
		private void AxisPan(float horizontal, float vertical)
		{
			var delta = new Vector2(horizontal * Time.deltaTime, vertical * Time.deltaTime);
			delta *= AxisPanRate;

			SecondaryDragAction(delta);
		}

		/// <summary>
		/// Applies rotation from axis values.
		/// </summary>
		private void AxisRotate(float horizontal, float vertical)
		{
			var delta = new Vector2(horizontal * Time.deltaTime, vertical * Time.deltaTime);
			delta *= AxisRotationRate;

			PrimaryDragAction(delta);
		}

		/// <summary>
		/// Performs a raycast from screenPosition. Fires RayHit
		/// </summary>
		private void Raycast(Vector2 screenPosition)
		{
			if (RayHit == null) 
				return;

			Ray ray = mainCamera.ScreenPointToRay(screenPosition);

			if (Physics.Raycast(ray, out RaycastHit hit, mainCamera.farClipPlane))
				RayHit(hit);
			else
				RayMiss();
		}
	}
}