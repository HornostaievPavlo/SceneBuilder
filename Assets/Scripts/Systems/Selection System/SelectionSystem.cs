using RuntimeHandle;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionSystem : MonoBehaviour
{
    //[SerializeField] private LeftMenu leftMenu;

    [SerializeField] private SurfacePainter surfacePainter;

    [SerializeField] private RuntimeTransformHandle runtimeTransformHandle;

    [SerializeField] private RowsInitializer rowsInitializer;

    [SerializeField] private GameObject controlsButtons;

    [SerializeField] private GameObject rightMenu;

    [HideInInspector] public SelectableObject selectableObject;

    [HideInInspector] public GameObject selectedObject;

    [HideInInspector] public int indexOfSelected;

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
    /// Selects object if it has SelectableObject component attached,
    /// deselects if it has not
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
                    //selectedObject = selectionParameter.gameObject;
                    //selectableObject = selectionParameter;
                    //indexOfSelected = rowsInitializer.dictOfLists[selectionParameter.type].IndexOf(selectionParameter.gameObject);
                    //ItemSelection(true, selectionParameter.transform);
                    //leftMenu.SelectRow(true);

                    OnObjectSelected.Invoke(selected);
                }
            }
            else
            {
                //leftMenu.SelectRow(false);
                //ItemSelection(false, transform);
                //selectableObject = null;

                OnObjectDeselected.Invoke();
            }
        }
    }

    /// <summary>
    /// Performs selection and deselection of the object 
    /// </summary>
    /// <param name="isSelected">True to select, false to deselect</param>
    /// <param name="target">Which object to operate</param>
    public void ItemSelection(bool isSelected, Transform target)
    {
        runtimeTransformHandle.gameObject.SetActive(isSelected);

        controlsButtons.SetActive(isSelected);

        if (!isSelected) rightMenu.SetActive(false);

        runtimeTransformHandle.target = isSelected ? target.transform : runtimeTransformHandle.transform;

        selectedObject = isSelected ? target.gameObject : null;

        surfacePainter.GetAllModelMaterials(selectedObject, isSelected);
    }
}