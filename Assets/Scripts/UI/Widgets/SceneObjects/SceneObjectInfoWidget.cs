using Gameplay;
using UnityEngine;

namespace UI.Widgets.SceneObjects
{
	public class SceneObjectInfoWidget : MonoBehaviour
	{
		private SceneObject _sceneObject;
		
		public void Setup(SceneObject sceneObject)
		{
			_sceneObject = sceneObject;
			gameObject.name = $"{sceneObject.gameObject.name}{nameof(SceneObjectInfoWidget)}";
		}
		
		public string GetSceneObjectId()
		{
			return _sceneObject.Id;
		}
	}
}