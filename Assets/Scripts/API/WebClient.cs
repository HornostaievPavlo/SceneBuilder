using System;
using System.Collections;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WebClient : MonoBehaviour
{
    public ScreenshotMaker ScreenshotMaker;

    public Button getAmountButton;
    public Button postPreviewButton;

    public TMP_Text outputField;

    readonly string saveFileUrl = "http://localhost:3090/Webserver/SaveFile";
    readonly string storageUrl = "http://localhost:3090/Webserver/Storage";

    private JsonSerializerSettings serializerSettings = new JsonSerializerSettings();

    private void Awake()
    {
        getAmountButton.onClick.AddListener(StartGetAmountRequest);
        postPreviewButton.onClick.AddListener(StartPostPreviewRequest);

        serializerSettings.TypeNameHandling = TypeNameHandling.Auto;
        serializerSettings.Formatting = Formatting.Indented;
    }

    private IEnumerator SendGetRequest()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(storageUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                //Debug.Log("GET request successful!");
                //Debug.Log("Response: " + www.downloadHandler.text);

                // You can parse the response here if needed.
                int numberOfSaves = int.Parse(www.downloadHandler.text);

                outputField.text = numberOfSaves.ToString();
            }
        }
    }

    private async Task SendPost(SaveFileRequest request)
    {
        string httpContentStr = JsonConvert.SerializeObject(request, serializerSettings);

        var httpClient = new HttpClient();

        HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(saveFileUrl,
            new StringContent(httpContentStr, Encoding.UTF8, "application/json"));

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            throw new Exception(httpResponseMessage.StatusCode.ToString());
        }
    }

    public void StartGetAmountRequest()
    {
        StartCoroutine(SendGetRequest());
    }

    int test = 0;
    private async void StartPostPreviewRequest()
    {
        test++;

        var tex = ScreenshotMaker.CaptureCameraView();
        byte[] dataToSend = tex.EncodeToPNG();

        SaveFileRequest request = new SaveFileRequest
        {
            Id = test,
            PreviewData = dataToSend
        };

        await SendPost(request);
    }
}
