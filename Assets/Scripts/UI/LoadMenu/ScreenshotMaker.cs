using UnityEngine;
using UnityEngine.UI;

public class ScreenshotMaker : MonoBehaviour
{
    private Camera mainCamera;

    public RawImage icon;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    public byte[] CaptureCameraView()
    {
        var width = Screen.width;
        var height = Screen.height;
        var photo = new Texture2D(width, height, TextureFormat.RGBA32, false);

        RenderTexture renderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);

        mainCamera.targetTexture = renderTexture;
        mainCamera.Render();

        RenderTexture.active = mainCamera.targetTexture;
        photo.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        mainCamera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(renderTexture);

        photo.Apply();

        var bytes = photo.EncodeToJPG(100);
        Destroy(photo);

        return bytes;
    }

    public void TestScreenshot()
    {        
        byte[] bytes = CaptureCameraView();

        Texture2D tex = new Texture2D(Screen.width, Screen.height);
        tex.LoadImage(bytes);
        tex.Apply();

        icon.texture = tex;
    }
}
