using System.Collections.Generic;
using UnityEngine;

public class SaveLoadUtility : MonoBehaviour
{
    public static readonly string scenePath = @"D:\_GLTF\Saves\Scene.gltf";

    public static readonly string assetsSavePath = @"D:\_GLTF\Saves";

    public static readonly string duckModelPath =
        "https://raw.githubusercontent.com/KhronosGroup/glTF-Sample-Models/master/2.0/Duck/glTF/Duck.gltf";

    public static Transform assetsParent;

    //public static List<GameObject> savingTargets = new List<GameObject>();

    private void Start()
    {
        assetsParent = GameObject.Find("Assets Placeholder").transform;
    }   
}
