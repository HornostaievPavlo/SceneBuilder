using RuntimeHandle;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionSystem : MonoBehaviour
{
    [SerializeField] private LeftMenu leftMenu;

    [SerializeField] private ColorManipulator colorManipulator;

    [SerializeField] private RuntimeTransformHandle runtimeTransformHandle;

    [SerializeField] private RowsInitializer rowsInitializer;

    [SerializeField] private GameObject controlsButtons;

    [SerializeField] private GameObject rightMenu;

    [HideInInspector] public SelectableObject selectableObject;

    [HideInInspector] public GameObject selectedObject;

    [HideInInspector] public int indexOfSelected;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        RaycastSelection();
    }

    /// <summary>
    /// Selects object if it has SelectableObject component attached,
    /// deselects if it has not
    /// </summary>
    private void RaycastSelection()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit raycastHit;
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition); // throwing a raycast

            if (Physics.Raycast(ray, out raycastHit, 100.0f)) // condition when raycast hits collider
            {
                Transform selectedTransform = raycastHit.transform;

                // perform selection if it contains SelectableObject component
                var selectionParameter = selectedTransform.GetComponentInParent<SelectableObject>();

                if (selectionParameter != null)
                {
                    selectedObject = selectionParameter.gameObject;

                    selectableObject = selectionParameter;

                    indexOfSelected = rowsInitializer.dictOfLists[selectionParameter.type].IndexOf(selectionParameter.gameObject);

                    ItemSelection(true, selectionParameter.transform);

                    leftMenu.SelectRow(true);
                }
            }
            else
            {
                leftMenu.SelectRow(false);

                ItemSelection(false, transform);

                selectableObject = null;
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

        colorManipulator.GetAllModelMaterials(selectedObject, isSelected);
    }
}