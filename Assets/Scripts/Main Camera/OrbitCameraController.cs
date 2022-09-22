using UnityEngine;

public class OrbitCameraController : MonoBehaviour
{
    [SerializeField] private float rotationSensitivity;
    [SerializeField] private float scrollSensitivity;

    private Transform mainCamera;

    private Transform cameraFocalPoint;

    private float yRotation;
    private float xRotation;

    private void Start()
    {
        cameraFocalPoint = GetComponent<Transform>();
        mainCamera = GetComponentInChildren<Camera>().transform;
    }

    private void Update()
    {
        RotateCamera();

        ZoomCamera();
    }

    private void RotateCamera() // rotating around focal point
    {
        if (Input.GetMouseButton(1))
        {
            float mousePositionX = Input.GetAxis("Mouse X") * rotationSensitivity * Time.deltaTime;
            float mousePositionY = Input.GetAxis("Mouse Y") * rotationSensitivity * Time.deltaTime;

            yRotation += mousePositionX;
            xRotation -= mousePositionY;

            //xRotation = Mathf.Clamp(xRotation, -7.5f, 90); // if we will need a rotating restriction

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }
    }

    private void ZoomCamera() // scrolling to and out from focal point
    {
        var mouseWheel = Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity * Time.deltaTime;

        mainCamera.Translate(Vector3.forward * mouseWheel);

        // visually looks better to go down while zooming to small models
        float scrollingDownDivisor = 5.25f;
        mainCamera.Translate(Vector3.down * mouseWheel / scrollingDownDivisor);
    }
}