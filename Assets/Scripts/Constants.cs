using UnityEngine;

public static class Constants
{
	public const string DuckModelPath = "https://raw.githubusercontent.com/KhronosGroup/glTF-Sample-Models/master/2.0/Duck/glTF/Duck.gltf";

	public const string AssetFile = "/Asset.gltf";
	public const string PreviewFile = "/Preview.png";
	
	public static readonly string ApplicationDataPath = Application.persistentDataPath;
	public static readonly string ScenePath = $"{ApplicationDataPath}/Scene";

	public const string ModelPrefabPath = "SceneObjects/ModelHolder";
	public const string CameraPrefabPath = "SceneObjects/Camera";
	public const string LabelPrefabPath = "SceneObjects/Label";
	
	public const string ModelHolderObjectName = "ModelHolder";
	
	public const string ColorProperty = "baseColorFactor";
	public const string TextureProperty = "baseColorTexture";
	
	public const int ZeroCameraDepth = 0;
	public const int LowestCameraDepth = 1;
	public const int MiddleCameraDepth = 2;
	public const int HighestCameraDepth = 3;

	public static readonly Color InfoWidgetSelectedColor = Color.blue;
	public static readonly Color InfoWidgetUnselectedColor = Color.white;

	public static readonly Color[] PaintingColors =
	{
		new(1f, 0f, 0f),             // Red
		new(0f, 1f, 0f),             // Green
		new(0f, 0f, 1f),             // Blue
		new(0f, 1f, 1f),             // Cyan
		new(1f, 1f, 0f),             // Yellow
		new(1f, 0.647f, 0f),         // Orange
		new(0.5f, 0f, 0.5f),         // Purple
		new(1f, 0.412f, 0.706f),     // Pink
		new(0.545f, 0.271f, 0.075f), // Brown
		new(0f, 0f, 0f),             // Black
		new(0.5f, 0.5f, 0.5f),       // Gray
		new(1f, 1f, 1f)              // White
	};
}