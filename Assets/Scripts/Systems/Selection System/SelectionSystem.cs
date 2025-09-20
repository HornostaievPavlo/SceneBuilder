using System;
using Gameplay;
using Services.InputService;
using UnityEngine;
using Zenject;

public class SelectionSystem : MonoBehaviour
{
    private IInputService _inputService;
    
    public static event Action<SceneObject> OnObjectSelected;
    public static event Action OnObjectDeselected;

    [Inject]
    private void Construct(IInputService inputService)
    {
        _inputService = inputService;
    }

    private void OnEnable()
    {
        _inputService.RayHit += OnRayHit;
        _inputService.RayMiss += OnRayMiss;
    }

    private void OnDisable()
    {
        _inputService.RayHit -= OnRayHit;
        _inputService.RayMiss -= OnRayMiss;
    }

    private void OnRayHit(RaycastHit hit)
    {
        var selected = hit.transform.gameObject.GetComponentInParent<SceneObject>();

        if (selected != null)
            OnObjectSelected(selected);
    }

    private void OnRayMiss()
    {
        OnObjectDeselected();
    }
}