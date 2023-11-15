using UnityEngine;

public class SelectableObjectUtility : MonoBehaviour
{
    public static void CopySelectableObject(SelectableObject selectable)
    {
        if (selectable == null) return;

        Transform selectableTr = selectable.transform;

        var meshCopy = new Mesh();
        meshCopy.Clear();
        meshCopy = selectableTr.GetComponentInChildren<MeshFilter>().mesh;
        meshCopy.name = "test";

        Material materialCopy = selectableTr.GetComponentInChildren<MeshRenderer>().material;
        Texture2D textureCopy = IOUtility.DuplicateTexture((Texture2D)materialCopy.mainTexture);
        MeshCollider colliderCopy = selectableTr.GetComponentInChildren<MeshCollider>();

        var copy = Instantiate(selectable, Vector3.zero, Quaternion.identity, selectableTr.parent);

        copy.gameObject.name = selectable.gameObject.name;
        copy.gameObject.GetComponentInChildren<MeshFilter>().mesh = meshCopy;
        copy.gameObject.GetComponentInChildren<MeshRenderer>().material.mainTexture = textureCopy;
        //copy.gameObject.GetComponentInChildren<MeshCollider>().sharedMesh = colliderCopy.sharedMesh;
        copy.gameObject.GetComponentInChildren<MeshCollider>().sharedMesh = meshCopy;
    }

    public static void DeleteSelectableObject(SelectableObject selectable)
    {
        if(selectable == null) return;

        TransformHandleSystem handle = FindObjectOfType<TransformHandleSystem>();
        handle.OnObjectDeselected();

        Destroy(selectable.gameObject);
    }
}
