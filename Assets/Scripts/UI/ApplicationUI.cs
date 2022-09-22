using RuntimeHandle;
using UnityEngine;

public class ApplicationUI : MonoBehaviour
{
    [SerializeField] private RaycastItemSelection RaycastItemSelection;

    [SerializeField] private RuntimeTransformHandle RuntimeTransformHandle;

    [SerializeField] private GameObject toggle;

    [SerializeField] private GameObject mainCameraFocalPoint;

    public void SetUIActive(bool isActive)
    {
        gameObject.SetActive(isActive);

        toggle.gameObject.transform.eulerAngles = new Vector3(0, 0, isActive ? 0 : 180);
    }

    public void CenterCameraFocalPoint()
    {
        if (RaycastItemSelection.selectedObject != null)
        {
            mainCameraFocalPoint.transform.position = RaycastItemSelection.selectedObject.transform.position;
        }
    }

    public void CopySelectedObject()
    {
        if (RaycastItemSelection.selectedObject != null)
        {
            Vector3 copyPosition = new Vector3(0, RaycastItemSelection.selectedObject.transform.position.y, 0);

            Instantiate(RaycastItemSelection.selectedObject.transform,
                        copyPosition,
                        RaycastItemSelection.selectedObject.transform.rotation,
                        RaycastItemSelection.selectedObject.transform.parent);
        }
    }

    public void RemoveObject()
    {
        if (RaycastItemSelection.selectedObject != null)
        {
            Destroy(RaycastItemSelection.selectedObject);

            RaycastItemSelection.ItemSelection(false, RaycastItemSelection.selectedObject.transform);
        }
    }

    public void SetPositionType()
    {
        RuntimeTransformHandle.type = HandleType.POSITION;
    }
    public void SetRotationType()
    {
        RuntimeTransformHandle.type = HandleType.ROTATION;
    }
    public void SetScaleType()
    {
        RuntimeTransformHandle.type = HandleType.SCALE;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}