using Gameplay;

namespace Services.SceneObjectsRegistry
{
	public interface ISceneObjectsRegistry
	{
		void Register(SceneObject sceneObject);
		void Unregister(SceneObject sceneObject);
	}
}