using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SavesRowsCoordinator : MonoBehaviour
{
    [SerializeField]
    private LoadingSystem loadingSystem;

    [SerializeField]
    private GameObject panelPrefab;

    [SerializeField]
    private RectTransform content;

    public static int panelsCounter = 0;

    public List<SaveFilePanel> panels = new List<SaveFilePanel>();

    List<string> directories;

    //TODO: refactor to be able to run rows creation on disabled menu (separate system)
    private void Start() => CreateRowsForExistingSaveFiles();

    private void CreateRowsForExistingSaveFiles()
    {
        directories = new List<string>(Directory.EnumerateDirectories(IOUtility.savesPath));

        foreach (var dir in directories)
        {
            CreateRowForNewSaveFile();
        }
    }

    public void CreateRowForNewSaveFile()
    {
        panelsCounter++;

        var panelGO = Instantiate(panelPrefab, content);
        SaveFilePanel panel = panelGO.GetComponentInChildren<SaveFilePanel>(true);
        panel.currentNumber = panelsCounter;

        panels.Add(panel);

        AddRowButtonsListeners(panel);
        AddSaveFilePreview(panel);
    }

    private void AddRowButtonsListeners(SaveFilePanel panel)
    {
        panel.loadButton.onClick.AddListener(() => loadingSystem.LoadAssetsFromSaveFile(panel.currentNumber));
        panel.loadButton.onClick.AddListener(() => this.gameObject.SetActive(false));

        panel.deleteButton.onClick.AddListener(() => DeleteSaveFile(panel));
    }

    private void UpdateLoadingButtonIndex(SaveFilePanel panel)
    {
        panel.loadButton.onClick.RemoveAllListeners();
        panel.loadButton.onClick.AddListener(() => loadingSystem.LoadAssetsFromSaveFile(panel.currentNumber));
        panel.loadButton.onClick.AddListener(() => this.gameObject.SetActive(false));

        panel.deleteButton.onClick.RemoveAllListeners();
        panel.deleteButton.onClick.AddListener(() => DeleteSaveFile(panel));
    }

    public void AddSaveFilePreview(SaveFilePanel panel)
    {
        string pathToPreviewTexture =
            IOUtility.scenePath +
            panel.currentNumber.ToString() +
            IOUtility.previewFile;

        Texture loadedPreview = IOUtility.OpenDirectoryAndLoadTexture(pathToPreviewTexture);
        panel.preview.texture = loadedPreview;
    }

    private void DeleteSaveFile(SaveFilePanel panel)
    {
        panelsCounter--;

        panels.Remove(panel);

        string directoryPath = IOUtility.scenePath + panel.currentNumber.ToString();
        Directory.Delete(directoryPath, true);

        string[] directories = Directory.GetDirectories(IOUtility.savesPath);

        for (int i = 0; i < directories.Length; i++)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directories[i]);

            string currentName = IOUtility.savesPath + @"\" + directoryInfo.Name;
            Debug.Log(currentName);

            string targetName = IOUtility.scenePath + (i + 1);
            Debug.Log(targetName);

            Directory.Move(currentName, targetName);
        }

        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].currentNumber = i + 1;
            UpdateLoadingButtonIndex(panels[i]);
        }

        Destroy(panel.gameObject);
    }
}
