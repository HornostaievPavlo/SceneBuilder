using System;
using UnityEngine;

public class SelectionSystem : MonoBehaviour
{
    [SerializeField] private InputSystem inputSystem;

    public static event Action<SelectableObject> OnObjectSelected;
    public static event Action OnObjectDeselected;

    private void OnEnable()
    {
        inputSystem.RayHit += OnRayHit;
        inputSystem.RayMiss += OnRayMiss;
    }

    private void OnDisable()
    {
        inputSystem.RayHit -= OnRayHit;
        inputSystem.RayMiss -= OnRayMiss;
    }

    private void OnRayHit(RaycastHit hit)
    {
        var selected = hit.transform.gameObject.GetComponentInParent<SelectableObject>();

        if (selected != null)
            OnObjectSelected(selected);
    }

    private void OnRayMiss()
    {
        OnObjectDeselected();
    }
}