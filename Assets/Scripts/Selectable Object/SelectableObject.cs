using UnityEngine;
public class SelectableObject : MonoBehaviour
{
    public ObjectType type;

    private CreatedObjectController CreatedObjectController;

    private void Start()
    {
        CreatedObjectController = GetComponentInParent<CreatedObjectController>();

        CreatedObjectController.AddCreatedObjectToList(this);
    }

    private void OnDestroy()
    {
        CreatedObjectController.RemoveCreatedObjectFromList(this);
    }
}