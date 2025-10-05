using Services.SceneObjectsRegistry;
using UnityEngine;
using Zenject;

namespace Gameplay
{
	public class SceneObjectsHolder : MonoBehaviour
	{
		private ISceneObjectsRegistry _sceneObjectsRegistry;

		[Inject]
		private void Construct(ISceneObjectsRegistry sceneObjectsRegistry)
		{
			_sceneObjectsRegistry = sceneObjectsRegistry;
		}
		
		private void Awake()
		{
			_sceneObjectsRegistry.RegisterSceneObjectsHolder(transform);
		}
	}
}