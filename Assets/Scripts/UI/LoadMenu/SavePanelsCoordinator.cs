using System.Collections.Generic;
using System.IO;
using Services.Loading;
using UnityEngine;
using Zenject;

public class SavePanelsCoordinator : MonoBehaviour
{
    [SerializeField]
    private GameObject loadMenu;

    [SerializeField]
    private GameObject panelPrefab;

    [SerializeField]
    private RectTransform content;

    public static int panelsCounter = 0;

    [HideInInspector]
    public List<SavePanel> panels = new();

    private ILoadService _loadService;

    [Inject]
    private void Construct(ILoadService loadService)
    {
        _loadService = loadService;
    }

    private void Start()
    {
        CreateRowsForExistingSaveFiles();
    }

    /// <summary>
    /// Looks into directory with save files
    /// and creates UI row for each of them
    /// </summary>
    private void CreateRowsForExistingSaveFiles()
    {
        var directories = new List<string>(Directory.EnumerateDirectories(IOUtility.dataPath));

        foreach (var dir in directories)
        {
            CreateRowForNewSaveFile();
        }
    }

    /// <summary>
    /// Creates UI row on runtime saving
    /// </summary>
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

    /// <summary>
    /// Sets buttons events for new UI row
    /// </summary>
    /// <param name="panel">Target panel</param>
    private void AddRowButtonsListeners(SavePanel panel)
    {
        panel.loadButton.onClick.AddListener(() => _loadService.LoadAssetsFromSaveFile(panel.currentNumber));
        panel.loadButton.onClick.AddListener(() => loadMenu.SetActive(false));

        panel.deleteButton.onClick.AddListener(() => DeleteSaveFile(panel));
    }

    /// <summary>
    /// Sets scene index for panel loading button
    /// </summary>
    /// <param name="panel">Target panel</param>
    private void UpdateLoadingButtonIndex(SavePanel panel)
    {
        panel.loadButton.onClick.RemoveAllListeners();
        panel.loadButton.onClick.AddListener(() => _loadService.LoadAssetsFromSaveFile(panel.currentNumber));
        panel.loadButton.onClick.AddListener(() => loadMenu.SetActive(false));

        panel.deleteButton.onClick.RemoveAllListeners();
        panel.deleteButton.onClick.AddListener(() => DeleteSaveFile(panel));
    }

    /// <summary>
    /// Loads texture from directory and
    /// sets it to preview field of UI row
    /// </summary>
    /// <param name="panel">Target panel</param>
    public void AddSaveFilePreview(SavePanel panel)
    {
        string pathToPreviewTexture =
            IOUtility.scenePath +
            panel.currentNumber.ToString() +
            IOUtility.previewFile;

        Texture loadedPreview = IOUtility.OpenDirectoryAndLoadTexture(pathToPreviewTexture);
        panel.preview.texture = loadedPreview;
    }

    /// <summary>
    /// Removes panel and save file directory
    /// </summary>
    /// <param name="panel">Panel to remove</param>
    private void DeleteSaveFile(SavePanel panel)
    {
        panelsCounter--;

        panels.Remove(panel);

        string directoryPath = IOUtility.scenePath + panel.currentNumber.ToString();
        Directory.Delete(directoryPath, true);

        RearrangeSaveFiles();

        Destroy(panel.gameObject);
    }

    /// <summary>
    /// Changes save folders names
    /// according to order in directory
    /// </summary>
    private void RearrangeSaveFiles()
    {
        string[] directories = Directory.GetDirectories(IOUtility.dataPath);

        for (int i = 0; i < directories.Length; i++)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directories[i]);

            string currentName = IOUtility.dataPath + @"/" + directoryInfo.Name;

            string targetName = IOUtility.scenePath + (i + 1);

            if (currentName != targetName)
                Directory.Move(currentName, targetName);

            panels[i].currentNumber = i + 1;
            UpdateLoadingButtonIndex(panels[i]);
        }
    }
}
