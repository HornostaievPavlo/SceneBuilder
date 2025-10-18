using Gameplay;
using Plain;
using Services.Instantiation;
using Services.SceneObjectsRegistry;
using UnityEngine;

namespace Services.SceneObjectCopying
{
	public class SceneObjectCopyService : ISceneObjectCopyService
	{
		private readonly ReadableTextureCopyInstantiator _textureCopyInstantiator;
		private readonly IInstantiateService _instantiateService;
		private readonly ISceneObjectsRegistry _sceneObjectsRegistry;

		public SceneObjectCopyService(
			IInstantiateService instantiateService,
			ISceneObjectsRegistry sceneObjectsRegistry)
		{
			_instantiateService = instantiateService;
			_sceneObjectsRegistry = sceneObjectsRegistry;
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
			
			GameObject copyGameObject = Object.Instantiate(originalObject.gameObject, Vector3.zero, Quaternion.identity, _sceneObjectsRegistry.SceneObjectsHolder);
			
			if (copyGameObject.TryGetComponent(out SceneObject existingSceneObject))
			{
				Object.DestroyImmediate(existingSceneObject);
			}
			
			copyGameObject.name = originalObject.gameObject.name;
			copyGameObject.GetComponentInChildren<MeshFilter>().mesh = meshCopy;
			copyGameObject.GetComponentInChildren<MeshRenderer>().material.mainTexture = textureCopy;
			copyGameObject.GetComponentInChildren<MeshCollider>().sharedMesh = meshCopy;
			
			SceneObject sceneObject = _instantiateService.AddSceneObjectComponent(copyGameObject, originalObject.TypeId);
			
			sceneObject.AnimateScale(isScalingUp: true);
		}
	}
}
