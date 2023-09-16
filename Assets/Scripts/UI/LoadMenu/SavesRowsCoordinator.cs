using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavesRowsCoordinator : MonoBehaviour
{
    [SerializeField]
    private LoadingSystem loadingSystem;

    [SerializeField]
    private GameObject rowPrefab;

    [SerializeField]
    private RectTransform content;

    public static int scenesCounter;

    List<string> directories;

    private void Awake() => CreateRowsForExistingSaveFiles();

    private void CreateRowsForExistingSaveFiles()
    {
        directories = new List<string>(Directory.EnumerateDirectories(SaveLoadUtility.scenesPath));

        foreach (var dir in directories)
        {
            CreateRowForNewSaveFile();
        }
    }

    public void CreateRowForNewSaveFile()
    {
        scenesCounter++;

        var row = Instantiate(rowPrefab, content);

        TMP_Text sceneNumber = row.GetComponentInChildren<TMP_Text>();

        int currentRowNumber = scenesCounter;
        sceneNumber.text = currentRowNumber.ToString();

        //Debug.Log("Counter - " + scenesCounter);

        AddRowButtonsListeners(row, currentRowNumber);
    }

    private void AddRowButtonsListeners(GameObject row, int rowNumber)
    {
        Button[] rowButtons = row.GetComponentsInChildren<Button>(true);

        Button loadButton = rowButtons[0];
        var deleteButton = rowButtons[1];

        loadButton.onClick.AddListener(() => loadingSystem.LoadAssetsFromSaveFile(rowNumber));
        loadButton.onClick.AddListener(() => this.gameObject.SetActive(false));
    }
}
