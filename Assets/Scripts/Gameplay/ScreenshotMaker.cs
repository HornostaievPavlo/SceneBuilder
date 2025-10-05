using UnityEngine;

public class ScreenshotMaker : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake() => mainCamera = GetComponent<Camera>();

    private Texture2D CaptureCameraView()
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
        return photo;
    }

    public void MakePreviewScreenshot(int sceneNumber)
    {
        Texture2D screenshot = CaptureCameraView();

        string directoryPath = IOUtility.scenePath + sceneNumber;
        string screenshotPath = directoryPath + Constants.PreviewFile;
        IOUtility.CreateDirectoryAndSaveTexture
            (screenshot, directoryPath, screenshotPath);
    }
}
