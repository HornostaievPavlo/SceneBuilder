using System;
using UnityEngine;

namespace Services.InputService
{
	public interface IInputService
	{
		event Action OnTouchBegin;
		event Action OnTouchEnd;
		event Action<Vector2> OnPrimaryDrag;
		event Action<Vector2> OnSecondaryDrag;
		event Action<float> OnZoom;
		event Action<RaycastHit> OnRayHit;
		event Action OnRayMiss;
	}
}