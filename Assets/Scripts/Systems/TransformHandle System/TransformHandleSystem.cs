using RuntimeHandle;
using System.Collections;
using UnityEngine;

public class TransformHandleSystem : MonoBehaviour
{
    private RuntimeTransformHandle handle;

    private int cameraIgnoringLayer;

    private void Awake()
    {
        handle = GetComponentInChildren<RuntimeTransformHandle>(true);

        cameraIgnoringLayer = LayerMask.NameToLayer("Gizmo");
    }

    private void OnEnable()
    {
        SelectionSystem.OnObjectSelected += OnObjectSelected;
        SelectionSystem.OnObjectDeselected += OnObjectDeselected;
    }

    private void OnDisable()
    {
        SelectionSystem.OnObjectSelected -= OnObjectSelected;
        SelectionSystem.OnObjectDeselected -= OnObjectDeselected;
    }

    private void OnObjectSelected(SelectableObject selectable)
    {
        handle.gameObject.SetActive(true);
        handle.target = selectable.transform;

        StartCoroutine(SetLayerOfGizmoChildren());
    }

    public void OnObjectDeselected()
    {
        handle.gameObject.SetActive(false);
        handle.target = null;
    }

    public void SetPositionType()
    {
        handle.type = HandleType.POSITION;
        StartCoroutine(SetLayerOfGizmoChildren());
    }

    public void SetRotationType()
    {
        handle.type = HandleType.ROTATION;
        StartCoroutine(SetLayerOfGizmoChildren());
    }

    public void SetScaleType()
    {
        handle.type = HandleType.SCALE;
        StartCoroutine(SetLayerOfGizmoChildren());
    }

    private IEnumerator SetLayerOfGizmoChildren()
    {
        yield return null;

        var children = handle.GetComponentsInChildren<Transform>();

        foreach (var child in children)
        {
            child.gameObject.layer = cameraIgnoringLayer;
        }
    }
}
