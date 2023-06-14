using RuntimeHandle;
using UnityEngine;

public class ApplicationUI : MonoBehaviour
{
    [SerializeField] private SelectionSystem _raycastItemSelection;

    [SerializeField] private RuntimeTransformHandle _runtimeTransformHandle;

    [SerializeField] private GameObject _uiToggle;

    [SerializeField] private GameObject _cameraFocalPoint;

    /// <summary>
    /// Turns all UI elements on/off 
    /// </summary>
    /// <param name="isActive">Toggle value</param>
    public void SetUIActive(bool isActive)
    {
        gameObject.SetActive(isActive);

        _uiToggle.gameObject.transform.eulerAngles = new Vector3(0, 0, isActive ? 0 : 180);
    }

    /// <summary>
    /// Alligns camera focal point to currently selected object
    /// </summary>
    public void CenterCameraFocalPoint()
    {
        if (_raycastItemSelection.selectedObject != null)
        {
            _cameraFocalPoint.transform.position = _raycastItemSelection.selectedObject.transform.position;
        }
    }

    /// <summary>
    /// Makes a copy of selected object
    /// </summary>
    public void CopySelectedObject()
    {
        if (_raycastItemSelection.selectedObject != null)
        {
            Vector3 copyPosition = new Vector3(0, _raycastItemSelection.selectedObject.transform.position.y, 0);

            Instantiate(_raycastItemSelection.selectedObject.transform,
                        copyPosition,
                        _raycastItemSelection.selectedObject.transform.rotation,
                        _raycastItemSelection.selectedObject.transform.parent);
        }
    }

    /// <summary>
    /// Deletes selected object and its ui row from the scene
    /// </summary>
    public void RemoveObject()
    {
        if (_raycastItemSelection.selectedObject != null)
        {
            Destroy(_raycastItemSelection.selectedObject);

            _raycastItemSelection.ItemSelection(false, _raycastItemSelection.selectedObject.transform);
        }
    }

    /// <summary>
    /// Changes mode of the TransformHandle to translation 
    /// </summary>
    public void SetPositionType()
    {
        _runtimeTransformHandle.type = HandleType.POSITION;
    }

    /// <summary>
    /// Changes mode of the TransformHandle to rotation 
    /// </summary>
    public void SetRotationType()
    {
        _runtimeTransformHandle.type = HandleType.ROTATION;
    }

    /// <summary>
    /// Changes mode of the TransformHandle to scaling 
    /// </summary>
    public void SetScaleType()
    {
        _runtimeTransformHandle.type = HandleType.SCALE;
    }

    /// <summary>
    /// Exits application
    /// </summary>
    public void QuitApplication()
    {
        Application.Quit();
    }
}