using Services.SceneObjectsRegistry;
using UnityEngine;
using Zenject;

namespace Gameplay
{
	public class SceneObjectCopyInstantiator : MonoBehaviour
	{
		private ReadableTextureCopyInstantiator _textureCopyInstantiator;
		
		private ISceneObjectsRegistry _sceneObjectsRegistry;

		[Inject]
		private void Construct(ISceneObjectsRegistry sceneObjectsRegistry)
		{
			_sceneObjectsRegistry = sceneObjectsRegistry;
		}

		private void Awake()
		{
			_textureCopyInstantiator = new ReadableTextureCopyInstantiator();
		}

		public void CreateCopy(SceneObject originalObject)
		{
			Transform selectableTr = originalObject.transform;
			
			var meshCopy = new Mesh();
			meshCopy.Clear();
			meshCopy = selectableTr.GetComponentInChildren<MeshFilter>().mesh;
			meshCopy.name = "mesh";
			
			Material materialCopy = selectableTr.GetComponentInChildren<MeshRenderer>().material;
			Texture2D textureCopy = _textureCopyInstantiator.CreateReadableTexture(materialCopy.mainTexture);
			
			SceneObject copyObject = Instantiate(originalObject, Vector3.zero, Quaternion.identity, selectableTr.parent);
			
			copyObject.gameObject.name = originalObject.gameObject.name;
			copyObject.gameObject.GetComponentInChildren<MeshFilter>().mesh = meshCopy;
			copyObject.gameObject.GetComponentInChildren<MeshRenderer>().material.mainTexture = textureCopy;
			copyObject.gameObject.GetComponentInChildren<MeshCollider>().sharedMesh = meshCopy;
			
			copyObject.GenerateGuid();
			_sceneObjectsRegistry.Register(copyObject);
		}
	}
}