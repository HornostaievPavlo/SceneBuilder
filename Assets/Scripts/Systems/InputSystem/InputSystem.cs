using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputSystem : MonoBehaviour
{
    [SerializeField] private float axisPanRate = 1f;
    [SerializeField] private float axisRotRate = 100f;
    [SerializeField] private float mousePanRate = 1f;
    [SerializeField] private float mouseRotRate = 100f;
    [SerializeField] private float mouseZoomRate = 0.05f;

    public event Action TouchBeginAction = delegate { };
    public event Action TouchReleaseAction = delegate { };
    public event Action<Vector2> SecondaryDragAction = delegate { };
    public event Action<Vector2> PrimaryDragAction = delegate { };
    public event Action<float> ZoomAction = delegate { };
    public event Action<Vector2> TapAction = delegate { };
    public event Action<RaycastHit> RayHit;
    public event Action RayMiss;

    public Vector2 InputScreenSpacePosition { get; private set; }

    private bool hadFocus;
    private bool hasTouch;
    private Vector3 previousMousePosition;
    private Camera mainCamera;

    private void Awake() => mainCamera = Camera.main;

    private void Update()
    {
        PollMouse();

        previousMousePosition = Input.mousePosition;
        hadFocus = Application.isFocused;
    }

    /// <summary>
    /// Process mouse input.
    /// </summary>
    private void PollMouse()
    {
        InputScreenSpacePosition = Input.mousePosition;
        Vector2 delta = Vector2.zero;
        // If window was just refocused, previous mouse position will be invalid
        // so keep delta 0 unless we had focus for at least 1 frame
        if (hadFocus)
        {
            delta = NormalizeScreenVector(Input.mousePosition - previousMousePosition);
        }
        // On left mouse button single click - raycast
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Raycast(Input.mousePosition);
        }
        // If the left mouse button is not held - end touch
        if (!Input.GetMouseButton(0))
        {
            EndTouch();
        }
        // If the right mouse button is held - rotate
        if (Input.GetMouseButton(1))
        {
            BeginTouch();
            PrimaryDragAction(delta * mouseRotRate);
        }
        // If middle mouse held - pan
        else if (Input.GetMouseButton(2))
        {
            SecondaryDragAction(delta * mousePanRate);
        }
        else if (Input.mouseScrollDelta.y != 0f)
        {
            ZoomAction(Input.mouseScrollDelta.y * mouseZoomRate);
        }
    }

    private void BeginTouch()
    {
        if (hasTouch) return;

        hasTouch = true;
        TouchBeginAction();
    }

    private void EndTouch()
    {
        if (!hasTouch) return;

        hasTouch = false;
        TouchReleaseAction();
    }

    /// <summary>
    /// Normalize a screen space vector by dividing it by native screen height.
    /// </summary>
    private Vector2 NormalizeScreenVector(Vector2 v)
    {
        int height = Screen.currentResolution.height;
        return v / height;
    }

    /// <summary>
    /// Applies panning from axis values.
    /// </summary>
    public void AxisPan(float horizontal, float vertical)
    {
        Vector2 delta = new Vector2(horizontal * Time.deltaTime, vertical * Time.deltaTime);
        delta *= axisPanRate;
        SecondaryDragAction(delta);
    }

    /// <summary>
    /// Applies rotation from axis values.
    /// </summary>
    public void AxisRotate(float horizontal, float vertical)
    {
        Vector2 delta = new Vector2(horizontal * Time.deltaTime, vertical * Time.deltaTime);
        delta *= axisRotRate;
        PrimaryDragAction(delta);
    }

    /// <summary>
    /// Performs a raycast from screenPosition. Fires RayHit
    /// </summary>
    private void Raycast(Vector2 screenPosition)
    {
        if (RayHit == null) return;

        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, mainCamera.farClipPlane))
            RayHit(hit);
        else
            RayMiss();
    }
}
