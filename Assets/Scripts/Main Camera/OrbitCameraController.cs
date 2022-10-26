using UnityEngine;

public class OrbitCameraController : MonoBehaviour
{
    [Tooltip("Responsiveness of camera rotation")]
    [SerializeField]
    private float _rotationSensitivity;

    [Tooltip("Responsiveness of camera zoom")]
    [SerializeField]
    private float _scrollSensitivity;

    private Transform _mainCamera;

    private float _yRotation;
    private float _xRotation;

    private void Start()
    {
        _mainCamera = GetComponentInChildren<Camera>().transform;
    }

    private void Update()
    {
        RotateCamera();

        ZoomCamera();
    }

    /// <summary>
    /// Rotates camera around focal point
    /// </summary>
    private void RotateCamera()
    {
        int rightMouseButton = 1;

        if (Input.GetMouseButton(rightMouseButton))
        {
            float mousePositionX = Input.GetAxis("Mouse X") * _rotationSensitivity * Time.deltaTime;
            float mousePositionY = Input.GetAxis("Mouse Y") * _rotationSensitivity * Time.deltaTime;

            _yRotation += mousePositionX;
            _xRotation -= mousePositionY;

            transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        }
    }

    /// <summary>
    /// Zooms camera is side of focal point
    /// </summary>
    private void ZoomCamera()
    {
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel") * _scrollSensitivity * Time.deltaTime;

        _mainCamera.Translate(Vector3.forward * mouseWheel);

        // looks better to move a bit down while zooming to small models
        float scrollingDownDivisor = 5.25f;
        _mainCamera.Translate(Vector3.down * mouseWheel / scrollingDownDivisor);
    }
}