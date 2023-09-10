using UnityEngine;

public class OrbitCameraSystem : MonoBehaviour
{
    [Tooltip("Responsiveness of camera movement")]
    [SerializeField]
    private float rotationSensitivity;

    [Tooltip("Responsiveness of camera zoom")]
    [SerializeField]
    private float zoomSensitivity;

    [Tooltip("Responsiveness of focal point movement")]
    [SerializeField]
    private float movingSensitivity;

    private Transform currentSelection;

    private Transform mainCamera;

    private float _yRotation;
    private float _xRotation;

    private void Awake() => mainCamera = GetComponentInChildren<Camera>().transform;

    private void OnEnable()
    {
        SelectionSystem.OnObjectSelected += OnObjectSelected;
        SelectionSystem.OnObjectDeselected += OnObjectDeselected;
    }

    private void OnObjectSelected(SelectableObject selectable)
    {
        currentSelection = selectable.transform;
    }

    private void OnObjectDeselected()
    {
        currentSelection = null;
    }

    private void OnDisable()
    {
        SelectionSystem.OnObjectSelected -= OnObjectSelected;
        SelectionSystem.OnObjectDeselected -= OnObjectDeselected;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
            RotateCamera();

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            ZoomCamera();

        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl))
            MoveFocalPoint();
    }

    /// <summary>
    /// Rotates camera around focal point
    /// </summary>
    private void RotateCamera()
    {
        float mousePositionX = Input.GetAxis("Mouse X") * rotationSensitivity * Time.deltaTime;
        float mousePositionY = Input.GetAxis("Mouse Y") * rotationSensitivity * Time.deltaTime;

        _yRotation += mousePositionX;
        _xRotation -= mousePositionY;

        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
    }

    /// <summary>
    /// Zooms camera is side of focal point
    /// </summary>
    private void ZoomCamera()
    {
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity * Time.deltaTime;
        mainCamera.Translate(Vector3.forward * mouseWheel);

        float scrollingDownDivisor = 5f;
        mainCamera.Translate((Vector3.down * mouseWheel) / scrollingDownDivisor);
    }

    /// <summary>
    /// Moves camera focal point according to direction of mouse movement
    /// Works on both LCTRL and LMB are pressed
    /// </summary>
    private void MoveFocalPoint()
    {
        float mousePositionX = Input.GetAxis("Mouse X") * movingSensitivity * Time.deltaTime;
        float mousePositionY = Input.GetAxis("Mouse Y") * movingSensitivity * Time.deltaTime;

        Vector3 movingVector = new Vector3(-mousePositionX, -mousePositionY, 0);

        transform.Translate(movingVector * movingSensitivity * Time.deltaTime);
    }

    /// <summary>
    /// Alligns camera focal point to currently selected object
    /// </summary>
    public void AllignCameraWithSelection()
    {
        if (currentSelection) transform.position = currentSelection.position;
    }
}
