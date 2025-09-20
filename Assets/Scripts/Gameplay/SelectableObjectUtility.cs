using Gameplay;
using UnityEngine;

public class SelectableObjectUtility : MonoBehaviour
{
    public static void CopySelectableObject(SceneObject scene)
    {
        if (scene == null) return;

        Transform selectableTr = scene.transform;

        var meshCopy = new Mesh();
        meshCopy.Clear();
        meshCopy = selectableTr.GetComponentInChildren<MeshFilter>().mesh;
        meshCopy.name = "test";

        Material materialCopy = selectableTr.GetComponentInChildren<MeshRenderer>().material;
        Texture2D textureCopy = IOUtility.DuplicateTexture((Texture2D)materialCopy.mainTexture);
        MeshCollider colliderCopy = selectableTr.GetComponentInChildren<MeshCollider>();

        var copy = Instantiate(scene, Vector3.zero, Quaternion.identity, selectableTr.parent);

        copy.gameObject.name = scene.gameObject.name;
        copy.gameObject.GetComponentInChildren<MeshFilter>().mesh = meshCopy;
        copy.gameObject.GetComponentInChildren<MeshRenderer>().material.mainTexture = textureCopy;
        copy.gameObject.GetComponentInChildren<MeshCollider>().sharedMesh = meshCopy;
    }

    public static void DeleteSelectableObject(SceneObject scene)
    {
        if (scene == null) return;

        TransformHandleSystem handle = FindFirstObjectByType<TransformHandleSystem>();
        handle.HandleObjectDeselected();

        Destroy(scene.gameObject);
    }
}
