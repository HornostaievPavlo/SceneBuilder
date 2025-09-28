using Gameplay;
using UnityEngine;

namespace UI.Widgets.SceneObjects
{
	public class SceneObjectInfoWidget : MonoBehaviour
	{
		protected SceneObject SceneObject;
		
		public virtual void Setup(SceneObject sceneObject)
		{
			SceneObject = sceneObject;
			
			gameObject.name = string.Empty;
			gameObject.name = $"{sceneObject.gameObject.name}{nameof(SceneObjectInfoWidget)}";
		}
		
		public string GetSceneObjectId()
		{
			return SceneObject.Id;
		}
	}
}