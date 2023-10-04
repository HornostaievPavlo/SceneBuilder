using System;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    [SerializeField] float touchTapDuration = 0.2f;
    [SerializeField] float touchPanRate = 1f;
    [SerializeField] float touchRotRate = 100f;
    [SerializeField] float touchZoomRate = 1f;
    [SerializeField] float axisPanRate = 1f;
    [SerializeField] float axisRotRate = 100f;
    [SerializeField] float mousePanRate = 1f;
    [SerializeField] float mouseRotRate = 100f;
    [SerializeField] float mouseZoomRate = 0.05f;

    bool hadFocus;
    Vector3 previousMousePosition;
    float previousTouchTime = 0f;
    bool hasTouch;

    public event Action TouchBeginAction = delegate { };
    public event Action TouchReleaseAction = delegate { };
    public event Action<Vector2> SecondaryDragAction = delegate { };
    public event Action<Vector2> PrimaryDragAction = delegate { };
    public event Action<float> ZoomAction = delegate { };
    public event Action<Vector2> TapAction = delegate { };
    public event Action Reset = delegate { };
    public event Action<RaycastHit> RayHit;
    public Vector2 InputScreenSpacePosition { get; private set; }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            PollTouch();
        }
        else
        {
            PollMouse();
        }

        PollKeys();
        previousMousePosition = Input.mousePosition;
        hadFocus = Application.isFocused;
    }

    /// <summary>
    /// Process touch input.
    /// </summary>
    void PollTouch()
    {
        InputScreenSpacePosition = Input.GetTouch(0).position;
        Vector2 delta = Vector2.zero;
        if (Input.touchCount != 1)
        {
            EndTouch();
        }
        if (Input.touchCount == 1)
        {
            BeginTouch();
            HandleSingleTouch(Input.GetTouch(0));
        }
        else if (Input.touchCount == 2)
        {
            HandlePinch(Input.GetTouch(0), Input.GetTouch(1));
        }
        else if (Input.touchCount > 2)
        {
            HandleMultiTouch(Input.touchCount);
        }
    }

    /// <summary>
    /// Handle single touch input.
    /// </summary>
    void HandleSingleTouch(Touch touch)
    {
        Vector2 delta = Vector2.zero;
        if (touch.phase == TouchPhase.Moved)
        {
            previousTouchTime = 0f;
            delta = GetTouchDeltaPosition(touch);
            delta = NormalizeScreenVector(delta);
            PrimaryDragAction(delta * touchRotRate);
        }
        // If the touch just began - remember time
        else if (touch.phase == TouchPhase.Began)
        {
            previousTouchTime = Time.unscaledTime;
        }
        // If the touch ended within some time - fire raycast & tap event
        else if (touch.phase == TouchPhase.Ended)
        {
            if (Time.unscaledTime - previousTouchTime <= touchTapDuration)
            {
                TapAction(NormalizeScreenVector(touch.position));
                Raycast(touch.position);
            }
        }
    }

    /// <summary>
    /// Handle pinch/zoom gesture.
    /// </summary>
    void HandlePinch(Touch touch0, Touch touch1)
    {
        if (touch0.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved)
        {
            Vector2 touch0delta = GetTouchDeltaPosition(touch0);
            Vector2 touch1delta = GetTouchDeltaPosition(touch1);

            Vector2 touch0previous = NormalizeScreenVector(touch0.position - touch0delta);
            Vector2 touch1previous = NormalizeScreenVector(touch1.position - touch1delta);
            float previousDistance = Vector2.Distance(touch0previous, touch1previous);

            Vector2 touch0current = NormalizeScreenVector(touch0.position);
            Vector2 touch1current = NormalizeScreenVector(touch1.position);
            float currentDistance = Vector2.Distance(touch0current, touch1current);

            float difference = currentDistance - previousDistance;
            ZoomAction(difference * touchZoomRate);
        }
    }

    /// <summary>
    /// Handle multiple touches as one blob.
    /// </summary>
    void HandleMultiTouch(int touchCount)
    {
        Vector2 delta = Vector2.zero;
        for (int i = 0; i < touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Moved) delta += GetTouchDeltaPosition(touch);
        }
        delta = NormalizeScreenVector(delta);
        SecondaryDragAction(delta * touchPanRate);
    }

    /// <summary>
    /// Process mouse input.
    /// </summary>
    void PollMouse()
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
        if (Input.GetMouseButtonDown(0))
        {
            Raycast(Input.mousePosition);
        }
        // If the left mouse button is not held - end touch
        if (!Input.GetMouseButton(0))
        {
            EndTouch();
        }
        // If the left mouse button is held - rotate
        if (Input.GetMouseButton(0))
        {
            BeginTouch();
            PrimaryDragAction(delta * mouseRotRate);
        }
        // If right mouse held or middle mouse held - pan
        else if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            SecondaryDragAction(delta * mousePanRate);
        }
        else if (Input.mouseScrollDelta.y != 0f)
        {
            ZoomAction(Input.mouseScrollDelta.y * mouseZoomRate);
        }
    }

    void BeginTouch()
    {
        if (hasTouch) return;

        hasTouch = true;
        TouchBeginAction();
    }

    void EndTouch()
    {
        if (!hasTouch) return;

        hasTouch = false;
        TouchReleaseAction();
    }

    /// <summary>
    /// Process keyboard input.
    /// </summary>
    void PollKeys()
    {
        // Reset camera when space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Reset();
        }
    }

    /// <summary>
    /// Get touch delta position.
    /// Touch deltaTime may be different from update deltaTime.
    /// Furthermore, due to an unknown issue on some platforms Touch deltaPosition is inverted.
    /// </summary>
    Vector2 GetTouchDeltaPosition(Touch t)
    {
        Vector2 deltaPosition = t.deltaPosition;
        if (t.deltaTime > 0f)
        {
            deltaPosition = deltaPosition / t.deltaTime * Time.deltaTime;
        }
#if !UNITY_EDITOR && UNITY_WEBGL
        deltaPosition *= -1;
#endif
        return deltaPosition;
    }

    /// <summary>
    /// Normalize a screen space vector by dividing it by native screen height.
    /// </summary>
    Vector2 NormalizeScreenVector(Vector2 v)
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
    void Raycast(Vector2 screenPosition)
    {
        // Exit early if there are no subscribers to event
        if (RayHit == null) return;

        Camera camera = cameraManager.GetCurrentCamera();
        Ray ray = camera.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, camera.farClipPlane))
        {
            RayHit(hit);
        }
    }


}
