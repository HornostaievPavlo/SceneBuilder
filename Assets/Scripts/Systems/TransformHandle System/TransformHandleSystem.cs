using RuntimeHandle;
using System.Collections;
using Gameplay;
using Services.SceneObjectSelectionService;
using UnityEngine;
using Zenject;

public class TransformHandleSystem : MonoBehaviour
{
    private RuntimeTransformHandle handle;

    private int cameraIgnoringLayer;
    
    private ISceneObjectSelectionService _sceneObjectSelectionService;

    [Inject]
    private void Construct(ISceneObjectSelectionService sceneObjectSelectionService)
    {
        _sceneObjectSelectionService = sceneObjectSelectionService;
    }

    private void Awake()
    {
        handle = GetComponentInChildren<RuntimeTransformHandle>(true);

        cameraIgnoringLayer = LayerMask.NameToLayer("Gizmo");
    }

    private void OnEnable()
    {
        _sceneObjectSelectionService.OnObjectSelected += HandleObjectSelected;
        _sceneObjectSelectionService.OnObjectDeselected += HandleObjectDeselected;
    }

    private void OnDisable()
    {
        _sceneObjectSelectionService.OnObjectSelected -= HandleObjectSelected;
        _sceneObjectSelectionService.OnObjectDeselected -= HandleObjectDeselected;
    }

    private void HandleObjectSelected(SceneObject scene)
    {
        handle.gameObject.SetActive(true);
        handle.target = scene.transform;

        StartCoroutine(SetLayerOfGizmoChildren());
    }

    public void HandleObjectDeselected()
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
