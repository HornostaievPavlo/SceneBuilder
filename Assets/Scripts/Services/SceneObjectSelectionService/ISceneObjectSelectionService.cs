using System;
using Gameplay;

namespace Services.SceneObjectSelectionService
{
	public interface ISceneObjectSelectionService
	{
		SceneObject SelectedObject { get; }
		event Action<SceneObject> OnObjectSelected;
		event Action OnObjectDeselected;
	}
}