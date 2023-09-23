using System;
using System.Collections;
using UnityEngine;

public class ModelUploadingSystem : MonoBehaviour
{
    public static event Action<byte[]> OnModelUploaded;

    /// <summary>
    /// Serves for receiving url of file from JS drag and drop event
    /// </summary>
    /// <param name="url">File url</param>
    public void UploadModel(string url)
    {
        StartCoroutine(GetUploadedModelData(url));
    }

    /// <summary>
    /// Taking raw bytes from request object
    /// </summary>
    /// <param name="url">File url</param>
    /// <returns>Web request</returns>
    private IEnumerator GetUploadedModelData(string url)
    {
        var request = new WWW(url);
        yield return request;

        OnModelUploaded.Invoke(request.bytes);
    }
}
