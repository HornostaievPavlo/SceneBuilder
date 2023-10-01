using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionSystem : MonoBehaviour
{
    private Camera mainCamera;

    public static event Action<SelectableObject> OnObjectSelected;
    public static event Action OnObjectDeselected;

    private void Awake() => mainCamera = Camera.main;

    private void Update() => TryRaycastSelection();

    /// <summary>
    /// Fires OnObjectSelected or OnObjectDeselected
    /// according to result of raycast 
    /// </summary>
    private void TryRaycastSelection()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, mainCamera.farClipPlane))
            {
                var selected = hit.transform.gameObject.GetComponentInParent<SelectableObject>();

                if (selected != null)
                    OnObjectSelected.Invoke(selected);
            }
            else
                OnObjectDeselected.Invoke();
        }
    }
}