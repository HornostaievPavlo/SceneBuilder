using GLTFast;
using System.Collections;
using TMPro;
using UnityEngine;

public class ModelSpawner : MonoBehaviour
{
    [Tooltip("Field where path to model is provided")]
    [SerializeField]
    private TMP_InputField _inputField;

    private GameObject placeholder; 
    
    private Transform _modelParent;

    /// <summary>
    /// Imports model from path,
    /// adds SelectableObject component,
    /// sets object type to Model
    /// </summary>
    public void ImportModel()
    {
        if (_inputField.text != "")
        {
            placeholder = new GameObject();
            placeholder.name = "Model";
            placeholder.transform.position = new Vector3(0, 0, 0);

            placeholder.transform.SetParent(_modelParent);

            SelectableObject selectableObject = placeholder.AddComponent<SelectableObject>();
            selectableObject.type = ObjectType.Model;

            var gltfAsset = placeholder.AddComponent<GltfAsset>();

            string pathToModel = _inputField.text.ToString();

            gltfAsset.Load(pathToModel);

            StartCoroutine(AddColliders());
        }
    }

    /// <summary>
    /// Loads duck model into scene
    /// </summary>
    public void ImportDuckModel()
    {
        string pathToDuck = "https://raw.githubusercontent.com/KhronosGroup/glTF-Sample-Models/master/2.0/Duck/glTF/Duck.gltf";

        placeholder = new GameObject();
        placeholder.name = "Model";
        placeholder.transform.position = new Vector3(0, 0, 0);

        placeholder.transform.SetParent(_modelParent);

        SelectableObject selectableObject = placeholder.AddComponent<SelectableObject>();
        selectableObject.type = ObjectType.Model;

        var gltfAsset = placeholder.AddComponent<GltfAsset>();

        gltfAsset.Load(pathToDuck);

        StartCoroutine(AddColliders());
    }

    /// <summary>
    /// Adds colliders to all child gameObjects in model hierarchy
    /// </summary>
    /// <returns>Waits one second</returns>
    private IEnumerator AddColliders()
    {
        yield return new WaitForSeconds(1);

        MeshRenderer[] meshRenderers = placeholder.GetComponentsInChildren<MeshRenderer>();

        if (meshRenderers.Length >= 1)
        {
            foreach (MeshRenderer item in meshRenderers)
            {
                var itemCollider = item.gameObject.AddComponent<MeshCollider>();

                itemCollider.convex = true;
            }
        }
    }
}