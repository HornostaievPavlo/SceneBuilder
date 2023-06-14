using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    [HideInInspector] public ObjectType type;

    private RowsInitializer _createdObjectManager;

    private void Start()
    {
        _createdObjectManager = FindObjectOfType<RowsInitializer>();

        _createdObjectManager.AddCreatedObjectToList(this);
    }

    private void OnDestroy()
    {
        _createdObjectManager.RemoveCreatedObjectFromList(this);
    }
}