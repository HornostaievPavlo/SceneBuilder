using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class SavesRowsCoordinator : MonoBehaviour
{
    [SerializeField]
    private GameObject rowPrefab;

    [SerializeField]
    private RectTransform content;

    public static int scenesCounter;

    List<string> directories;

    private void Awake () => CreateRowsForExistingSaveFiles();

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
        sceneNumber.text = scenesCounter.ToString();

        Debug.Log("Counter - " + scenesCounter);
    }
}
