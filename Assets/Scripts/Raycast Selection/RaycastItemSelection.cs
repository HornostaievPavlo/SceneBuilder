using RuntimeHandle;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastItemSelection : MonoBehaviour
{
    [SerializeField] private Menu menu;

    [SerializeField] private ColorManipulator colorManipulator;

    [SerializeField] private GameObject contolsButtons;

    [SerializeField] private GameObject transformHandle;

    [HideInInspector] public SelectableObject raycastSelectableObj;

    [HideInInspector] public GameObject selectedObject;

    [HideInInspector] public int indexOfSelected;

    private CreatedObjectManager _createdObjectManager;

    private RuntimeTransformHandle _runtimeTransformHandle;

    private Camera _mainCamera;

    private void Start()
    {
        _runtimeTransformHandle = transformHandle.GetComponent<RuntimeTransformHandle>();

        _createdObjectManager = GetComponent<CreatedObjectManager>();

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

                    raycastSelectableObj = selectionParameter;

                    indexOfSelected = _createdObjectManager.dictOfLists[selectionParameter.type].IndexOf(selectionParameter.gameObject);

                    ItemSelection(true, selectionParameter.transform);

                    menu.SelectRow(true);
                }
            }
            else
            {
                menu.SelectRow(false);

                ItemSelection(false, transform);

                raycastSelectableObj = null;
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
        transformHandle.SetActive(isSelected);

        contolsButtons.SetActive(isSelected);

        _runtimeTransformHandle.target = isSelected ? target.transform : _runtimeTransformHandle.transform;

        selectedObject = isSelected ? target.gameObject : null;

        colorManipulator.GetAllModelMaterials(selectedObject, isSelected);
    }
}