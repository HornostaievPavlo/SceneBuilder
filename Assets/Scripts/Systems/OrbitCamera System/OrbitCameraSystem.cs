using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitCameraSystem : MonoBehaviour
{
    [SerializeField] private InputSystem inputSystem;

    [SerializeField] private float minPitchAngle = -90f;
    [SerializeField] private float maxPitchAngle = 90f;

    private Camera mainCamera;
    private Quaternion defaultCameraRotation;

    private GameObject currentSelection;

    private Vector3 focusPosition;
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

    private Vector2 pan;
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

    private Quaternion rotation;
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

    private float zoomAmount;
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

    private Vector3 cameraPosition;
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

    private void UpdateTransform()
    {
        transform.SetPositionAndRotation(cameraPosition + rotation * pan, rotation);
    }

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();

        CameraPosition = transform.position;
        defaultCameraRotation = Rotation;
    }

    private void OnEnable()
    {
        SelectionSystem.OnObjectSelected += OnObjectSelected;
        SelectionSystem.OnObjectDeselected += OnObjectDeselected;
        inputSystem.SecondaryDragAction += Move;
        inputSystem.PrimaryDragAction += Rotate;
        inputSystem.ZoomAction += Zoom;
    }

    private void OnDisable()
    {
        SelectionSystem.OnObjectSelected -= OnObjectSelected;
        SelectionSystem.OnObjectDeselected -= OnObjectDeselected;
        inputSystem.SecondaryDragAction -= Move;
        inputSystem.PrimaryDragAction -= Rotate;
        inputSystem.ZoomAction -= Zoom;
    }

    private void OnObjectSelected(SelectableObject selectable)
    {
        currentSelection = selectable.gameObject;
    }

    private void OnObjectDeselected() => currentSelection = null;

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
    public void FocusCamera(GameObject target)
    {
        Bounds b = GetBounds(target, 2f);

        float radius = b.extents.magnitude;
        radius *= 1.1f;

        float distance = radius / Mathf.Sin(mainCamera.fieldOfView * Mathf.Deg2Rad * 0.5f);
        FocusPosition = b.center;

        CameraPosition = GetCameraPosition(FocusPosition, defaultCameraRotation, distance);
        Pan = Vector2.zero;
    }

    /// <summary>
    /// Iterates through all Renderers on the GameObject and returns their combined Bounds.
    /// </summary>
    /// <param name="maxDeviation">standard deviation of outliers to discard</param>
    public static Bounds GetBounds(GameObject root, float maxDeviation = 0f)
    {
        var allBounds = root.GetComponentsInChildren<Renderer>(false).Select(r => r.bounds);
        int count = allBounds.Count();

        if (maxDeviation > 0f && count > 0)
        {
            var meanPosition = allBounds.Select(b => b.center).Aggregate((a, b) => a + b) / count;
            var boundsMagnitudes = allBounds.Select(bounds => Tuple.Create((bounds.center - meanPosition).magnitude, bounds));
            var meanMagnitude = boundsMagnitudes.Select(b => b.Item1).Aggregate((a, b) => a + b) / count;

            allBounds = boundsMagnitudes.Where(b =>
            {
                var v = b.Item1 - meanMagnitude;
                return Mathf.Sqrt(v * v) <= maxDeviation;
            }).Select(b => b.Item2);
        }
        if (!allBounds.Any()) return new Bounds();

        var bounds = allBounds.First();
        foreach (var b in allBounds)
        {
            bounds.Encapsulate(b);
        }
        return bounds;
    }

    public void AlignCameraWithSelection() => FocusCamera(currentSelection);
}
