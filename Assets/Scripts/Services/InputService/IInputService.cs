using System;
using UnityEngine;

namespace Services.InputService
{
	public interface IInputService
	{
		event Action TouchBeginAction;
		event Action TouchReleaseAction;
		event Action<Vector2> SecondaryDragAction;
		event Action<Vector2> PrimaryDragAction;
		event Action<float> ZoomAction;
		event Action<RaycastHit> RayHit;
		event Action RayMiss;
	}
}