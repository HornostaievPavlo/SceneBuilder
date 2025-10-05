using UnityEngine;

namespace Gameplay
{
	public class SceneObjectCopyInstantiator : MonoBehaviour
	{
		public SceneObject CreateCopy(SceneObject originalObject)
		{
			Transform selectableTr = originalObject.transform;

			var meshCopy = new Mesh();
			meshCopy.Clear();
			meshCopy = selectableTr.GetComponentInChildren<MeshFilter>().mesh;
			meshCopy.name = "test";

			Material materialCopy = selectableTr.GetComponentInChildren<MeshRenderer>().material;
			Texture2D textureCopy = IOUtility.DuplicateTexture((Texture2D)materialCopy.mainTexture);

			SceneObject copyObject = Instantiate(originalObject, Vector3.zero, Quaternion.identity, selectableTr.parent);

			copyObject.gameObject.name = originalObject.gameObject.name;
			copyObject.gameObject.GetComponentInChildren<MeshFilter>().mesh = meshCopy;
			copyObject.gameObject.GetComponentInChildren<MeshRenderer>().material.mainTexture = textureCopy;
			copyObject.gameObject.GetComponentInChildren<MeshCollider>().sharedMesh = meshCopy;
			
			return copyObject;
		}
	}
}