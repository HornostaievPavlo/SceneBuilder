using UnityEngine;

public static class Constants
{
	public const string DuckModelPath = "https://raw.githubusercontent.com/KhronosGroup/glTF-Sample-Models/master/2.0/Duck/glTF/Duck.gltf";

	public const string AssetFile = "/Asset.gltf";
	public const string TextureFile = "/Texture.png";
	public const string PreviewFile = "/Preview.png";
	
	public static readonly string ApplicationDataPath = Application.persistentDataPath;
	public static readonly string ScenePath = $"{ApplicationDataPath}/Scene";

	public const string ModelPrefabPath = "Assets/Prefabs/SceneObjects/ModelHolder.prefab";
	public const string CameraPrefabPath = "Assets/Prefabs/SceneObjects/Camera.prefab";
	public const string LabelPrefabPath = "Assets/Prefabs/SceneObjects/Label.prefab";
	
	public const string ModelHolderObjectName = "ModelHolder";
	
	public const int ZeroCameraDepth = 0;
	public const int LowestCameraDepth = 1;
	public const int MiddleCameraDepth = 2;
	public const int HighestCameraDepth = 3;

	public static readonly Color InfoWidgetSelectedColor = Color.blue;
	public static readonly Color InfoWidgetUnselectedColor = Color.white;
}