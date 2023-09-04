using System.Collections.Generic;
using UnityEngine;

public class SaveLoadUtility : MonoBehaviour
{
    public static readonly string modelPath = @"D:\_GLTF\Saves\Asset\Model.gltf";
    public static readonly string texturePath = @"D:\_GLTF\Saves\Asset\Texture.png";

    public static readonly string assetSavePath = @"D:\_GLTF\Saves";

    public static readonly string duckModelPath =
        "https://raw.githubusercontent.com/KhronosGroup/glTF-Sample-Models/master/2.0/Duck/glTF/Duck.gltf";

    public static Transform assetsParent;

    public static List<GameObject> savingTargets = new List<GameObject>();

    private void Start()
    {
        assetsParent = GameObject.Find("Assets Placeholder").transform;
    }

    public static void ClearTargetsList()
    {
        savingTargets.Clear();
    }
}
