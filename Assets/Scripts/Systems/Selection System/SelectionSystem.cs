using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionSystem : MonoBehaviour
{
    //[SerializeField] private LeftMenu leftMenu;

    [SerializeField] private SurfacePainter surfacePainter;
    [SerializeField] private GameObject rightMenu;

    private Camera mainCamera;

    /// <summary>
    /// Changing selection logic to event based type
    /// </summary>
    public static event Action<SelectableObject> OnObjectSelected;
    public static event Action OnObjectDeselected;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        TryRaycastSelection();
    }

    /// <summary>
    /// Fires OnObjectSelected or OnObjectDeselected
    /// according to result of raycast 
    /// </summary>
    private void TryRaycastSelection()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, mainCamera.farClipPlane))
            {
                var selected = raycastHit.transform.gameObject.GetComponentInParent<SelectableObject>();

                if (selected != null)
                {
                    //leftMenu.SelectRow(true);

                    OnObjectSelected.Invoke(selected);
                }
            }
            else
            {
                //leftMenu.SelectRow(false);

                OnObjectDeselected.Invoke();
            }
        }
    }

    public void ItemSelection(bool isSelected, Transform target)
    {
        if (!isSelected) rightMenu.SetActive(false);

        //surfacePainter.GetAllModelMaterials(selectedObject, isSelected);
    }
}