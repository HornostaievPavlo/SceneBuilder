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
        Texture2D textureCopy = DuplicateTexture((Texture2D)materialCopy.mainTexture);

        var copy = Instantiate(selectable, Vector3.zero, Quaternion.identity, selectableTr.parent);

        copy.gameObject.name = selectable.gameObject.name;
        copy.gameObject.GetComponentInChildren<MeshFilter>().mesh = meshCopy;
        copy.gameObject.GetComponentInChildren<MeshRenderer>().material.mainTexture = textureCopy;
        copy.gameObject.GetComponentInChildren<MeshCollider>().sharedMesh = meshCopy;
    }

    public static void DeleteSelectableObject(SelectableObject selectable)
    {
        if (selectable == null) return;

        TransformHandleSystem handle = FindObjectOfType<TransformHandleSystem>();
        handle.OnObjectDeselected();

        Destroy(selectable.gameObject);
    }

    /// <summary>
    /// Makes readable copy of texture
    /// </summary>
    /// <param name="source">Original texture</param>
    /// <returns>Readable copy</returns>
    private static Texture2D DuplicateTexture(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
}
