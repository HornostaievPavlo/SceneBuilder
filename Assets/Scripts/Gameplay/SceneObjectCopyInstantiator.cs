using Services.SceneObjectsRegistry;
using Services.Instantiation;
using UnityEngine;
using Zenject;

namespace Gameplay
{
	public class SceneObjectCopyInstantiator : MonoBehaviour
	{
		private ReadableTextureCopyInstantiator _textureCopyInstantiator;
		
		private IInstantiateService _instantiateService;
		private ISceneObjectsRegistry _sceneObjectsRegistry;

		[Inject]
		private void Construct(IInstantiateService instantiateService, ISceneObjectsRegistry sceneObjectsRegistry)
		{
			_instantiateService = instantiateService;
			_sceneObjectsRegistry = sceneObjectsRegistry;
		}
		
		private void Awake()
		{
			_textureCopyInstantiator = new ReadableTextureCopyInstantiator();
		}

		public void CreateCopy(SceneObject originalObject)
		{
			var meshCopy = new Mesh();
			meshCopy.Clear();
			meshCopy = originalObject.transform.GetComponentInChildren<MeshFilter>().mesh;
			meshCopy.name = "mesh";
			
			Material materialCopy = originalObject.transform.GetComponentInChildren<MeshRenderer>().material;
			Texture2D textureCopy = _textureCopyInstantiator.CreateReadableTexture(materialCopy.mainTexture);
			
			GameObject copyGameObject = Instantiate(originalObject.gameObject, Vector3.zero, Quaternion.identity, _sceneObjectsRegistry.SceneObjectsHolder);
			
			if (copyGameObject.TryGetComponent(out SceneObject existingSceneObject))
			{
				DestroyImmediate(existingSceneObject);
			}
			
			copyGameObject.name = originalObject.gameObject.name;
			copyGameObject.GetComponentInChildren<MeshFilter>().mesh = meshCopy;
			copyGameObject.GetComponentInChildren<MeshRenderer>().material.mainTexture = textureCopy;
			copyGameObject.GetComponentInChildren<MeshCollider>().sharedMesh = meshCopy;
			
			_instantiateService.AddSceneObjectComponent(copyGameObject, originalObject.TypeId);
		}
	}
}