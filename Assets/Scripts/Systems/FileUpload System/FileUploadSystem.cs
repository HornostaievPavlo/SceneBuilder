using System.Collections;
using TMPro;
using UnityEngine;

public class FileUploadSystem : MonoBehaviour
{
    [SerializeField] private LoadingSystem loadingSystem;

    [SerializeField] private TMP_Text visual;

    private IEnumerator LoadFile(string url)
    {
        var www = new WWW(url);
        yield return www;

        visual.text = www.isDone.ToString() + ", ";
        visual.text += www.bytes.Length + " bytes loaded, ";

        var data = www.bytes;

        Load(data);
    }

    public void FileSelected(string url)
    {
        StartCoroutine(LoadFile(url));
    }

    private async void Load(byte[] data)
    {
        bool success = await loadingSystem.LoadAssetsFromBytes(data);

        visual.text += "GLTF Loading " + success;
    }
}
