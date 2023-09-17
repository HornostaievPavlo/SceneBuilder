using UnityEngine;

public class SelectableObjectUtility : MonoBehaviour
{
    public static void CopySelectableObject(SelectableObject selectable)
    {
        Transform selectableTr = selectable.transform;

        Mesh meshCopy = selectableTr.GetComponentInChildren<MeshFilter>().mesh;
        Material materialCopy = selectableTr.GetComponentInChildren<MeshRenderer>().material;
        Texture2D textureCopy = IOUtility.DuplicateTexture((Texture2D)materialCopy.mainTexture);
        MeshCollider colliderCopy = selectableTr.GetComponentInChildren<MeshCollider>();

        var copy = Instantiate(selectable, Vector3.zero, Quaternion.identity, selectableTr.parent);

        copy.gameObject.name = selectable.gameObject.name;
        copy.gameObject.GetComponentInChildren<MeshFilter>().mesh = meshCopy;
        copy.gameObject.GetComponentInChildren<MeshRenderer>().material.mainTexture = textureCopy;
        copy.gameObject.GetComponentInChildren<MeshCollider>().sharedMesh = colliderCopy.sharedMesh;
    }

    public static void DeleteSelectableObject(SelectableObject selectable)
    {
        TransformHandleSystem handle = FindObjectOfType<TransformHandleSystem>();
        handle.OnObjectDeselected();

        Destroy(selectable.gameObject);
    }
}
