using System;
using System.Collections.Generic;
using System.Linq;
using Services.InputService;
using Services.SceneObjectSelectionService;
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

        private const float MinPitchAngle = -90f;
        private const float MaxPitchAngle = 90f;
    
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
            _defaultCameraRotation = GetRotation();
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
            SetPan(new Vector2(GetPan().x - delta.x, GetPan().y - delta.y));
        }

        private void HandlePrimaryDrag(Vector2 delta)
        {
            Quaternion currentRotation = GetRotation();

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
            zoom = GetZoomAmount() * zoom;
            
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
        
            Bounds bounds = GetBounds(target.gameObject);

            float radius = bounds.extents.magnitude;
            radius *= 1.1f;

            float distance = radius / Mathf.Sin(_mainCamera.fieldOfView * Mathf.Deg2Rad * 0.5f);
            SetFocusPosition(bounds.center);

            SetCameraPosition(GetCameraPosition(GetFocusPosition(), _defaultCameraRotation, distance));
            SetPan(Vector2.zero);
        }

        private Vector3 GetFocusPosition()
        {
            return _focusPosition;
        }

        private void SetFocusPosition(Vector3 value)
        {
            _focusPosition = value;
            _cameraPosition = GetCameraPosition(_focusPosition, _rotation, _zoomAmount);
            UpdateTransform();
        }

        private Vector2 GetPan()
        {
            return _pan;
        }

        private void SetPan(Vector2 value)
        {
            _pan = value;
            _cameraPosition = GetCameraPosition(_focusPosition, _rotation, _zoomAmount);
            UpdateTransform();
        }

        private Quaternion GetRotation()
        {
            return _rotation;
        }

        private void SetRotation(Quaternion value)
        {
            _rotation = value;
            _cameraPosition = GetCameraPosition(_focusPosition, _rotation, _zoomAmount);
            UpdateTransform();
        }

        private float GetZoomAmount()
        {
            return _zoomAmount;
        }

        private void SetZoomAmount(float value)
        {
            _zoomAmount = value;
            _cameraPosition = GetCameraPosition(_focusPosition, _rotation, _zoomAmount);
            UpdateTransform();
        }

        private Vector3 GetCameraPosition()
        {
            return _cameraPosition;
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
