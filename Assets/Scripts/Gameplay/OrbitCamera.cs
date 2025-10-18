using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Services.Input;
using Services.SceneObjectSelection;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    [RequireComponent(typeof(Camera))]
    public class OrbitCamera : MonoBehaviour
    {
        private Camera _mainCamera;

        private float _zoomAmount;
        private Vector2 _pan;
        
        private Vector3 _focusPosition;
        private Vector3 _cameraPosition;
        
        private Quaternion _rotation;
        private Quaternion _defaultCameraRotation;
        
        private Coroutine _focusCoroutine;

        private const float MinPitchAngle = 3f;
        private const float MaxPitchAngle = 90f;
        
        private const float FocusMovementDuration = 0.75f;
        private readonly AnimationCurve _focusMovementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    
        private IInputService _inputService;
        private ISceneObjectSelectionService _sceneObjectSelectionService;

        [Inject]
        private void Construct(IInputService inputService, ISceneObjectSelectionService sceneObjectSelectionService)
        {
            _inputService = inputService;
            _sceneObjectSelectionService = sceneObjectSelectionService;
        }

        private void Awake()
        {
            _mainCamera = GetComponent<Camera>();
            
            SetCameraPosition(transform.position);
            _defaultCameraRotation = _rotation;
        }

        private void OnEnable()
        {
            _inputService.OnSecondaryDrag += HandleSecondaryDrag;
            _inputService.OnPrimaryDrag += HandlePrimaryDrag;
            _inputService.OnZoom += HandleZoom;
        }

        private void OnDisable()
        {
            _inputService.OnSecondaryDrag -= HandleSecondaryDrag;
            _inputService.OnPrimaryDrag -= HandlePrimaryDrag;
            _inputService.OnZoom -= HandleZoom;
        }

        public void FocusOnSelectedObject()
        {
            FocusCamera(_sceneObjectSelectionService.SelectedObject);
        }
        
        private void HandleSecondaryDrag(Vector2 delta)
        {
            delta *= _zoomAmount;
            SetPan(new Vector2(_pan.x - delta.x, _pan.y - delta.y));
        }

        private void HandlePrimaryDrag(Vector2 delta)
        {
            Quaternion currentRotation = _rotation;

            currentRotation = Quaternion.AngleAxis(delta.x, Vector3.up) * currentRotation;

            Vector3 up = currentRotation * Vector3.up;
            Vector3 right = currentRotation * Vector3.right;

            float pitch = Vector3.SignedAngle(Vector3.up, up, right);
            float pitchDelta = Mathf.Clamp(pitch - delta.y, MinPitchAngle, MaxPitchAngle) - pitch;

            currentRotation = Quaternion.AngleAxis(pitchDelta, right) * currentRotation;
            SetRotation(currentRotation);
        }

        private void HandleZoom(float delta)
        {
            float zoom = 1f - delta;
            zoom = _zoomAmount * zoom;
            
            if (zoom < 0.05f)
            {
                zoom = 0.05f;
            }

            SetZoomAmount(zoom);
        }

        private void FocusCamera(SceneObject target)
        {
            if (target == null) 
                return;

            if (_focusCoroutine != null)
            {
                StopCoroutine(_focusCoroutine);
            }

            _focusCoroutine = StartCoroutine(FocusCameraCoroutine(target));
        }

        private IEnumerator FocusCameraCoroutine(SceneObject target)
        {
            Bounds bounds = GetBounds(target.gameObject);

            float radius = bounds.extents.magnitude;
            radius *= 1.1f;

            float distance = radius / Mathf.Sin(_mainCamera.fieldOfView * Mathf.Deg2Rad * 0.5f);
            
            Vector3 targetFocusPosition = bounds.center;
            Vector3 targetCameraPosition = GetCameraPosition(targetFocusPosition, _defaultCameraRotation, distance);
            Vector2 targetPan = Vector2.zero;

            Vector3 startFocusPosition = _focusPosition;
            Vector3 startCameraPosition = _cameraPosition;
            Vector2 startPan = _pan;

            float elapsedTime = 0f;

            while (elapsedTime < FocusMovementDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / FocusMovementDuration);
                float curveValue = _focusMovementCurve.Evaluate(t);

                _focusPosition = Vector3.Lerp(startFocusPosition, targetFocusPosition, curveValue);
                _pan = Vector2.Lerp(startPan, targetPan, curveValue);
                
                Vector3 lerpedCameraPosition = Vector3.Lerp(startCameraPosition, targetCameraPosition, curveValue);
                GetOrbitValues(_focusPosition, lerpedCameraPosition, out _rotation, out _zoomAmount);
                _cameraPosition = GetCameraPosition(_focusPosition, _rotation, _zoomAmount);
                
                UpdateTransform();

                yield return null;
            }

            SetFocusPosition(targetFocusPosition);
            SetCameraPosition(targetCameraPosition);
            SetPan(targetPan);

            _focusCoroutine = null;
        }

        private void SetFocusPosition(Vector3 value)
        {
            _focusPosition = value;
            _cameraPosition = GetCameraPosition(_focusPosition, _rotation, _zoomAmount);
            UpdateTransform();
        }

        private void SetPan(Vector2 value)
        {
            _pan = value;
            _cameraPosition = GetCameraPosition(_focusPosition, _rotation, _zoomAmount);
            UpdateTransform();
        }

        private void SetRotation(Quaternion value)
        {
            _rotation = value;
            _cameraPosition = GetCameraPosition(_focusPosition, _rotation, _zoomAmount);
            UpdateTransform();
        }

        private void SetZoomAmount(float value)
        {
            _zoomAmount = value;
            _cameraPosition = GetCameraPosition(_focusPosition, _rotation, _zoomAmount);
            UpdateTransform();
        }

        private void SetCameraPosition(Vector3 value)
        {
            _cameraPosition = value;
            GetOrbitValues(_focusPosition, _cameraPosition, out _rotation, out _zoomAmount);
            UpdateTransform();
        }

        private void UpdateTransform()
        {
            transform.SetPositionAndRotation(_cameraPosition + _rotation * _pan, _rotation);
        }

        private static Vector3 GetCameraPosition(Vector3 focusPosition, Quaternion rotation, float zoom)
        {
            return focusPosition + rotation * Vector3.back * zoom;
        }

        private static void GetOrbitValues(Vector3 focusPosition, Vector3 cameraPosition, out Quaternion rotation, out float zoom)
        {
            Vector3 lookVector = focusPosition - cameraPosition;
            rotation = Quaternion.LookRotation(lookVector);
            zoom = lookVector.magnitude;
        }

        private static Bounds GetBounds(GameObject root)
        {
            IEnumerable<Bounds> allBounds = root.GetComponentsInChildren<Renderer>(false).Select(r => r.bounds).ToList();
            int boundsAmount = allBounds.Count();

            var maxDeviation = 2f;

            if (maxDeviation > 0f && boundsAmount > 0)
            {
                Vector3 meanPosition = allBounds.Select(b => b.center).Aggregate((a, b) => a + b) / boundsAmount;
                IEnumerable<Tuple<float, Bounds>> boundsMagnitudes = allBounds.Select(bounds => Tuple.Create((bounds.center - meanPosition).magnitude, bounds)).ToList();
                float meanMagnitude = boundsMagnitudes.Select(b => b.Item1).Aggregate((a, b) => a + b) / boundsAmount;

                allBounds = boundsMagnitudes.Where(b =>
                {
                    float v = b.Item1 - meanMagnitude;
                    return Mathf.Sqrt(v * v) <= maxDeviation;
                })
                    .Select(b => b.Item2)
                    .ToList();
            }
            
            if (allBounds.Any() == false)
                return new Bounds();

            Bounds bounds = allBounds.First();
            
            foreach (Bounds boundsElement in allBounds)
            {
                bounds.Encapsulate(boundsElement);
            }
            
            return bounds;
        }
    }
}
