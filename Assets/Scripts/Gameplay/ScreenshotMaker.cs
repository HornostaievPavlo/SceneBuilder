using UnityEngine;

public class ScreenshotMaker : MonoBehaviour
{
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = GetComponent<Camera>();
    }

    public Texture2D CreatePreview()
    {
        int width = Screen.width;
        int height = Screen.height;
        var resultTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        RenderTexture renderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);

        _mainCamera.targetTexture = renderTexture;
        _mainCamera.Render();

        RenderTexture.active = _mainCamera.targetTexture;
        resultTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        _mainCamera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(renderTexture);

        resultTexture.Apply();
        return resultTexture;
    }
}
