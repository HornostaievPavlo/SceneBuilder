using System.IO;
using UnityEngine;

public class IOUtility : MonoBehaviour
{
    public static readonly string savesPath = @"D:\_GLTF\Saves";
    public static readonly string scenePath = @"D:\_GLTF\Saves\Scene";

    public static readonly string sceneFile = @"\Asset.gltf";
    public static readonly string textureFile = @"\Texture.png";
    public static readonly string previewFile = @"\Preview.png";

    public static readonly string duckModelPath =
        "https://raw.githubusercontent.com/KhronosGroup/glTF-Sample-Models/master/2.0/Duck/glTF/Duck.gltf";

    public static Transform assetsParent;

    private void Start()
    {
        assetsParent = GameObject.Find("Assets Placeholder").transform;
    }

    public static SelectableObject[] CollectSelectableObjects()
    {
        return assetsParent.GetComponentsInChildren<SelectableObject>();
    }

    /// <summary>
    /// Makes readable copy of texture
    /// </summary>
    /// <param name="source">Original texture</param>
    /// <returns>Readable copy</returns>
    public static Texture2D DuplicateTexture(Texture2D source)
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

    /// <summary>
    /// Creates directory and saves given texture
    /// </summary>
    /// <param name="texture">File to save</param>
    /// <param name="directory">Path to directory</param>
    /// <param name="file">Path to file</param>
    public static void CreateDirectoryAndSaveTexture(Texture2D texture, string directory, string file)
    {
        byte[] textureBytes = texture.EncodeToPNG();

        var folder = Directory.CreateDirectory(directory);
        var fullPath = Path.Combine(folder.FullName, file);

        File.WriteAllBytes(fullPath, textureBytes);
    }

    /// <summary>
    /// Loads texture from given path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Texture OpenDirectoryAndLoadTexture(string path)
    {
        byte[] loadedBytes = File.ReadAllBytes(path);

        Texture2D textureFromBytes = new Texture2D(2, 2);
        textureFromBytes.LoadImage(loadedBytes);

        return textureFromBytes;
    }
}
