using GLTFast;
using System.Collections;
using TMPro;
using UnityEngine;

public class ModelSpawner : MonoBehaviour
{
    [Tooltip("Field where path to model is provided")]
    [SerializeField]
    private TMP_InputField _inputField;

    private Transform _modelParent;

    private GameObject _modelPlaceholder;

    private void Start()
    {
        _modelParent = this.transform;
    }

    /// <summary>
    /// Imports model from path,
    /// adds SelectableObject component,
    /// sets object type to Model
    /// </summary>
    public void ImportModel()
    {
        if (_inputField.text != "")
        {
            _modelPlaceholder = new GameObject();
            _modelPlaceholder.name = "Model";
            _modelPlaceholder.transform.position = new Vector3(0, 0, 0);

            _modelPlaceholder.transform.SetParent(_modelParent);

            SelectableObject selectableObject = _modelPlaceholder.AddComponent<SelectableObject>();
            selectableObject.type = ObjectType.Model;

            var gltfAsset = _modelPlaceholder.AddComponent<GltfAsset>();

            string pathToModel = _inputField.text.ToString();

            // TODO : add button to load duck directly from scene

            //string pathToModel = "https://raw.githubusercontent.com/KhronosGroup/glTF-Sample-Models/master/2.0/Duck/glTF/Duck.gltf";

            gltfAsset.Load(pathToModel); // gltf model loading

            StartCoroutine(AddColliders());
        }
    }

    /// <summary>
    /// Adds colliders to all child gameObjects in model hierarchy
    /// </summary>
    /// <returns>Waits one second</returns>
    private IEnumerator AddColliders()
    {
        yield return new WaitForSeconds(1);

        MeshRenderer[] meshRenderers = _modelPlaceholder.GetComponentsInChildren<MeshRenderer>();

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