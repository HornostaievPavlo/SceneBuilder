using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WebClient : MonoBehaviour
{
    private string apiUrl = "http://localhost:3090/api/files";

    public Button postButton;

    private void Awake()
    {
        postButton.onClick.AddListener(StartPostRequest);
    }


    [Serializable]
    public class SaveFile
    {
        public int Id;
    }

    private IEnumerator SendPostRequest(int data)
    {
        SaveFile saveFile = new SaveFile { Id = data };

        string jsonData = JsonUtility.ToJson(saveFile);

        Dictionary<string, string> headers = new Dictionary<string, string>
    {
        { "Content-Type", "application/json" }
    };

        using (UnityWebRequest www = new UnityWebRequest(apiUrl, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));
            www.downloadHandler = new DownloadHandlerBuffer();

            foreach (var header in headers)
            {
                www.SetRequestHeader(header.Key, header.Value);
            }

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                Debug.Log("POST request successful!");
                Debug.Log("Response: " + www.downloadHandler.text);
            }
        }
    }

    public void StartPostRequest()
    {
        int fileId = 123;
        StartCoroutine(SendPostRequest(fileId));
    }
}
