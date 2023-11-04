using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SavePanelsCoordinator : MonoBehaviour
{
    [SerializeField]
    private GameObject loadMenu;

    [SerializeField]
    private GameObject panelPrefab;

    [SerializeField]
    private RectTransform content;

    private LoadingSystem loadingSystem;

    public static int panelsCounter = 0;

    public List<SavePanel> panels = new List<SavePanel>();

    List<string> directories;

    private void Start()
    {
        loadingSystem = GetComponent<LoadingSystem>();

        CreateRowsForExistingSaveFiles();
    }

    private void CreateRowsForExistingSaveFiles()
    {
        //directories = new List<string>(Directory.EnumerateDirectories(IOUtility.savesPath));

        // replace with GETAMOUNT request

        foreach (var dir in directories)
        {
            CreateRowForNewSaveFile();
        }
    }

    public void CreateRowForNewSaveFile()
    {
        panelsCounter++;

        var panelGO = Instantiate(panelPrefab, content);
        SavePanel panel = panelGO.GetComponentInChildren<SavePanel>(true);
        panel.currentNumber = panelsCounter;

        panels.Add(panel);

        AddRowButtonsListeners(panel);
        AddSaveFilePreview(panel);
    }

    private void AddRowButtonsListeners(SavePanel panel)
    {
        panel.loadButton.onClick.AddListener(() => loadingSystem.LoadAssetsFromSaveFile(panel.currentNumber));
        panel.loadButton.onClick.AddListener(() => loadMenu.SetActive(false));

        panel.deleteButton.onClick.AddListener(() => DeleteSaveFile(panel));
    }

    private void UpdateLoadingButtonIndex(SavePanel panel)
    {
        panel.loadButton.onClick.RemoveAllListeners();
        panel.loadButton.onClick.AddListener(() => loadingSystem.LoadAssetsFromSaveFile(panel.currentNumber));
        panel.loadButton.onClick.AddListener(() => loadMenu.SetActive(false));

        panel.deleteButton.onClick.RemoveAllListeners();
        panel.deleteButton.onClick.AddListener(() => DeleteSaveFile(panel));
    }

    public void AddSaveFilePreview(SavePanel panel)
    {
        string pathToPreviewTexture =
            IOUtility.scenePath +
            panel.currentNumber.ToString() +
            IOUtility.previewFile;

        //Texture loadedPreview = IOUtility.OpenDirectoryAndLoadTexture(pathToPreviewTexture);
        //panel.preview.texture = loadedPreview;
    }

    private void DeleteSaveFile(SavePanel panel)
    {
        panelsCounter--;

        panels.Remove(panel);

        string directoryPath = IOUtility.scenePath + panel.currentNumber.ToString();
        //Directory.Delete(directoryPath, true);

        // Removing save file from storage - DELETE"{Id}" + rearrange

        // RearrangeSaveFiles(); Rearrange through api

        Destroy(panel.gameObject);
    }

    private void RearrangeSaveFiles() // Move to api from here
    {
        string[] directories = Directory.GetDirectories(IOUtility.savesPath);

        for (int i = 0; i < directories.Length; i++)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directories[i]);

            string currentName = IOUtility.savesPath + @"\" + directoryInfo.Name;
            Debug.Log("Current name" + currentName);

            string targetName = IOUtility.scenePath + (i + 1);
            Debug.Log("Target name" + targetName);

            Directory.Move(currentName, targetName);

            panels[i].currentNumber = i + 1;
            UpdateLoadingButtonIndex(panels[i]);
        }
    }
}
