using UnityEngine;
using UnityEngine.UI;

public class LabelTransformHelper : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;

        var worldCanvas = GetComponentInChildren<Canvas>();
        worldCanvas.worldCamera = mainCamera;

        Button frameButton = GetComponentInChildren<Button>();
        frameButton.onClick.AddListener(TurnLabelTowardsCamera);
    }

    public void TurnLabelTowardsCamera()
    {
        transform.LookAt(mainCamera.transform);
    }
}
