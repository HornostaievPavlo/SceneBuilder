using RuntimeHandle;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastItemSelection : MonoBehaviour
{
    [SerializeField] private Menu Menu;

    [SerializeField] private GameObject selectedObjectContolButtons;

    [SerializeField] private GameObject transformHandle;

    private CreatedObjectController CreatedObjectController;

    private RuntimeTransformHandle RuntimeTransformHandle;

    private Camera mainCamera;

    [HideInInspector] public SelectableObject raycastSelectableObj;

    [HideInInspector] public GameObject selectedObject;

    [HideInInspector] public int indexOfSelected;

    private void Start()
    {
        RuntimeTransformHandle = transformHandle.GetComponent<RuntimeTransformHandle>();

        CreatedObjectController = GetComponent<CreatedObjectController>();

        mainCamera = Camera.main;
    }

    private void Update()
    {
        RaycastSelection();
    }

    private void RaycastSelection() // selection by raycasting and finding any collider on a way of ray
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit raycastHit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); // throwing a raycast

            if (Physics.Raycast(ray, out raycastHit, 100.0f)) // condition when raycast hits collider
            {
                Transform selectedTransform = raycastHit.transform; // take the transform which was hit

                var selectionParameter = selectedTransform.GetComponentInParent<SelectableObject>(); // perform selection if it contains SelectableObject component

                if (selectionParameter != null)
                {
                    selectedObject = selectionParameter.gameObject;

                    raycastSelectableObj = selectionParameter;

                    indexOfSelected = CreatedObjectController.dictOfLists[selectionParameter.type].IndexOf(selectionParameter.gameObject);

                    ItemSelection(true, selectionParameter.transform);

                    Menu.SelectRow(true);
                }
            }
            else // no collider - deselect
            {
                Menu.SelectRow(false);

                ItemSelection(false, transform);

                raycastSelectableObj = null;
            }
        }
    }

    public void ItemSelection(bool isSelected, Transform target) // unified selection/deselection according to bool value 
    {
        transformHandle.SetActive(isSelected);

        selectedObjectContolButtons.SetActive(isSelected);

        RuntimeTransformHandle.target = isSelected ? target.transform : RuntimeTransformHandle.transform;

        selectedObject = isSelected ? target.gameObject : null;
    }
}