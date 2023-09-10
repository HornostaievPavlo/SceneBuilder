using RuntimeHandle;
using UnityEngine;

public class TransformHandleSystem : MonoBehaviour
{
    private RuntimeTransformHandle handle;

    private void Awake()
    {
        handle = GetComponentInChildren<RuntimeTransformHandle>(true);
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
    }

    private void OnObjectDeselected()
    {
        handle.gameObject.SetActive(false);
        handle.target = null;
    }

    public void SetPositionType() => handle.type = HandleType.POSITION;

    public void SetRotationType() => handle.type = HandleType.ROTATION;

    public void SetScaleType() => handle.type = HandleType.SCALE;
}
