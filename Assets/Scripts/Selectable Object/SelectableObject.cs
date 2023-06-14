using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    [HideInInspector] public ObjectType type;

    private CreatedObjectManager _createdObjectManager;

    private void Start()
    {
        _createdObjectManager = FindObjectOfType<CreatedObjectManager>();

        _createdObjectManager.AddCreatedObjectToList(this);
    }

    private void OnDestroy()
    {
        _createdObjectManager.RemoveCreatedObjectFromList(this);
    }
}