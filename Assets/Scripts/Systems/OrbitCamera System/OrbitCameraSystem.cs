using UnityEngine;

public class OrbitCameraSystem : MonoBehaviour
{
    //[SerializeField] private Slider rotationSlider;
    //[SerializeField] private Slider zoomSlider;
    //[SerializeField] private Slider movementSlider;

    //private float rotationSensitivity;
    //private float zoomSensitivity;
    //private float movingSensitivity;

    //private Transform currentSelection;

    //private Transform mainCamera;

    //private float _yRotation;
    //private float _xRotation;

    //private void Awake()
    //{
    //    mainCamera = GetComponentInChildren<Camera>().transform;

    //    InitializeSensitivity();
    //}

    //private void InitializeSensitivity()
    //{
    //    rotationSensitivity = rotationSlider.value;
    //    rotationSlider.onValueChanged.AddListener(OnRotationValueChanged);

    //    zoomSensitivity = zoomSlider.value;
    //    zoomSlider.onValueChanged.AddListener(OnZoomValueChanged);

    //    movingSensitivity = movementSlider.value;
    //    movementSlider.onValueChanged.AddListener(OnMovementValueChanged);
    //}

    //private void OnRotationValueChanged(float value) => rotationSensitivity = value;

    //private void OnZoomValueChanged(float value) => zoomSensitivity = value;

    //private void OnMovementValueChanged(float value) => movingSensitivity = value;

    //private void OnEnable()
    //{
    //    SelectionSystem.OnObjectSelected += OnObjectSelected;
    //    SelectionSystem.OnObjectDeselected += OnObjectDeselected;
    //}

    //private void OnObjectSelected(SelectableObject selectable) => currentSelection = selectable.transform;

    //private void OnObjectDeselected() => currentSelection = null;

    //private void OnDisable()
    //{
    //    SelectionSystem.OnObjectSelected -= OnObjectSelected;
    //    SelectionSystem.OnObjectDeselected -= OnObjectDeselected;
    //}

    //private void Update()
    //{
    //    if (Input.GetMouseButton(1))
    //        RotateCamera();

    //    if (Input.GetAxis("Mouse ScrollWheel") != 0)
    //        ZoomCamera();

    //    if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl))
    //        MoveFocalPoint();
    //}

    ///// <summary>
    ///// Rotates camera around focal point
    ///// </summary>
    //private void RotateCamera()
    //{
    //    float mousePositionX = Input.GetAxis("Mouse X") * rotationSensitivity * Time.deltaTime;
    //    float mousePositionY = Input.GetAxis("Mouse Y") * rotationSensitivity * Time.deltaTime;

    //    _yRotation += mousePositionX;
    //    _xRotation -= mousePositionY;

    //    transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
    //}

    ///// <summary>
    ///// Zooms camera is side of focal point
    ///// </summary>
    //private void ZoomCamera()
    //{
    //    float mouseWheel = Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity * Time.deltaTime;
    //    mainCamera.Translate(Vector3.forward * mouseWheel);

    //    float scrollingDownDivisor = 5f;
    //    mainCamera.Translate((Vector3.down * mouseWheel) / scrollingDownDivisor);
    //}

    ///// <summary>
    ///// Moves camera focal point according to direction of mouse movement
    ///// Works on both LCTRL and LMB are pressed
    ///// </summary>
    //private void MoveFocalPoint()
    //{
    //    float mousePositionX = Input.GetAxis("Mouse X") * movingSensitivity * Time.deltaTime;
    //    float mousePositionY = Input.GetAxis("Mouse Y") * movingSensitivity * Time.deltaTime;

    //    Vector3 movingVector = new Vector3(-mousePositionX, -mousePositionY, 0);

    //    transform.Translate(movingVector * movingSensitivity * Time.deltaTime);
    //}

    ///// <summary>
    ///// Aligns camera focal point to currently selected object
    ///// </summary>
    //public void AlignCameraWithSelection()
    //{
    //    if (currentSelection) transform.position = currentSelection.position;
    //}


    //////////////////////////////

    [SerializeField] InputSystem inputSystem;
    [SerializeField] float minPitchAngle = -90f;
    [SerializeField] float maxPitchAngle = 90f;

    Vector3 defaultCameraPosition;
    Quaternion defaultCameraRotation;
    Camera cam;

    Vector3 focusPosition;
    /// <summary>
    /// Focus point of camera orbit.
    /// </summary>
    public Vector3 FocusPosition
    {
        get { return focusPosition; }
        set
        {
            focusPosition = value;
            cameraPosition = GetCameraPosition(focusPosition, rotation, zoomAmount);
            UpdateTransform();
        }
    }
    Vector2 pan;
    /// <summary>
    /// Position offset on a plane flush with the camera.
    /// </summary>
    public Vector2 Pan
    {
        get { return pan; }
        set
        {
            pan = value;
            cameraPosition = GetCameraPosition(focusPosition, rotation, zoomAmount);
            UpdateTransform();
        }
    }
    Quaternion rotation;
    /// <summary>
    /// Camera rotation around focus point.
    /// </summary>
    public Quaternion Rotation
    {
        get { return rotation; }
        set
        {
            rotation = value;
            cameraPosition = GetCameraPosition(focusPosition, rotation, zoomAmount);
            UpdateTransform();
        }
    }
    float zoomAmount;
    /// <summary>
    /// Camera distance from focus point.
    /// </summary>
    public float ZoomAmount
    {
        get { return zoomAmount; }
        set
        {
            zoomAmount = value;
            cameraPosition = GetCameraPosition(focusPosition, rotation, zoomAmount);
            UpdateTransform();
        }
    }
    Vector3 cameraPosition;
    /// <summary>
    /// World space camera position. 
    /// </summary>
    public Vector3 CameraPosition
    {
        get { return cameraPosition; }
        set
        {
            cameraPosition = value;
            GetOrbitValues(focusPosition, cameraPosition, transform.up, out rotation, out zoomAmount);
            UpdateTransform();
        }
    }
    void UpdateTransform()
    {
        transform.SetPositionAndRotation(cameraPosition + rotation * pan, rotation);
    }

    void Awake()
    {
        cam = GetComponent<Camera>();

        CameraPosition = transform.position;
        defaultCameraPosition = CameraPosition;
        defaultCameraRotation = Rotation;
    }

    void OnEnable()
    {
        inputSystem.SecondaryDragAction += Move;
        inputSystem.PrimaryDragAction += Rotate;
        inputSystem.ZoomAction += Zoom;
    }

    void OnDisable()
    {
        inputSystem.SecondaryDragAction -= Move;
        inputSystem.PrimaryDragAction -= Rotate;
        inputSystem.ZoomAction -= Zoom;
    }

    /// <summary>
    /// Returns world space camera position from orbit values.
    /// </summary>
    public static Vector3 GetCameraPosition(Vector3 focusPosition, Quaternion rotation, float zoom)
    {
        return focusPosition + rotation * Vector3.back * zoom;
    }

    /// <summary>
    /// Returns orbit camera values from focus position and camera position.
    /// </summary>
    public static void GetOrbitValues(Vector3 focusPosition, Vector3 cameraPosition, Vector3 up, out Quaternion rotation, out float zoom)
    {
        Vector3 lookVector = focusPosition - cameraPosition;
        rotation = Quaternion.LookRotation(lookVector);
        zoom = lookVector.magnitude;
    }

    /// <summary>
    /// Applies panning from view space motion vector.
    /// </summary>
    /// <param name="delta">position delta</param>
    public void Move(Vector2 delta)
    {
        delta *= zoomAmount;
        Pan = new Vector2(Pan.x - delta.x, Pan.y - delta.y);
    }

    /// <summary>
    /// Applies rotation from view space motion vector.
    /// </summary>
    /// <param name="delta">position delta</param>
    public void Rotate(Vector2 delta)
    {
        Quaternion r = Rotation;

        // Rotate around Y axis first
        r = Quaternion.AngleAxis(delta.x, Vector3.up) * r;

        // Get local rotated unit vectors
        Vector3 up = r * Vector3.up;
        Vector3 right = r * Vector3.right;

        // Get current pitch
        float pitch = Vector3.SignedAngle(Vector3.up, up, right);

        // Limit pitch change
        float pitchDelta = Mathf.Clamp(pitch - delta.y, minPitchAngle, maxPitchAngle) - pitch;

        // Change pitch
        r = Quaternion.AngleAxis(pitchDelta, right) * r;

        Rotation = r;
    }

    /// <summary>
    /// Applies zoom from delta value.
    /// </summary>
    /// <param name="delta">positive for zoom in, negative for zoom out</param>
    public void Zoom(float delta)
    {
        float zoom = 1f - delta;
        zoom = ZoomAmount * zoom;
        if (zoom < 0.05f) zoom = 0.05f;
        ZoomAmount = zoom;
    }

    /// <summary>
    /// Focuses camera on target object. Sets new default camera position.
    /// </summary>
    //public void FocusCamera(GameObject target)
    //{
    //    Bounds b = target.GetBounds(2f);
    //    float radius = b.extents.magnitude;
    //    radius *= 1.1f; // 10% zoom out
    //    float distance = radius / Mathf.Sin(cam.fieldOfView * Mathf.Deg2Rad * 0.5f);
    //    FocusPosition = b.center;
    //    CameraPosition = GetCameraPosition(FocusPosition, defaultCameraRotation, distance);
    //    defaultCameraPosition = CameraPosition;
    //    Pan = Vector2.zero;
    //}

    /// <summary>
    /// Resets pan, rotation, zoom and camera parameters to default values.
    /// </summary>
    public void Reset()
    {
        CameraPosition = defaultCameraPosition;
        Pan = Vector2.zero;
    }
}
