using UnityEngine;
using System.Collections;
using GLTFast;
using TMPro;

public class ModelSpawner : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    private Transform modelParent;

    private GameObject modelPlaceholder;

    private void Start()
    {
        modelParent = this.transform;
    }

    public void ImportModel() // load model, change enum type and add SelectableObject script
    {
        if (inputField.text != "")
        {
            modelPlaceholder = new GameObject();
            modelPlaceholder.name = "Model";
            modelPlaceholder.transform.position = new Vector3(0, 0, 0);

            modelPlaceholder.transform.SetParent(modelParent);

            SelectableObject selectableObject = modelPlaceholder.AddComponent<SelectableObject>();
            selectableObject.type = ObjectType.Model;

            var gltfAsset = modelPlaceholder.AddComponent<GltfAsset>();

            string pathToModel = inputField.text.ToString();

            //string pathToModel = "https://raw.githubusercontent.com/KhronosGroup/glTF-Sample-Models/master/2.0/Duck/glTF/Duck.gltf";

            gltfAsset.Load(pathToModel); // gltf model loading

            StartCoroutine(AddColliders());
        }
    }

    private IEnumerator AddColliders() // adding colliders to all mesh renderers to be able to click at any place of the model
    {
        yield return new WaitForSeconds(1); // less than second is not working for some models

        MeshRenderer[] meshRenderers = modelPlaceholder.GetComponentsInChildren<MeshRenderer>();

        if (meshRenderers.Length >= 1)
        {
            foreach (MeshRenderer item in meshRenderers)
            {
                var itemCollider = item.gameObject.AddComponent<MeshCollider>();

                itemCollider.convex = true; // all colliders created are convex
            }
        }
    }
}
