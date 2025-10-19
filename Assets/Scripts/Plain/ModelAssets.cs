using UnityEngine;

namespace Plain
{
	public class ModelAssets
	{
		public Mesh Mesh { get; }
		public Material Material { get; }
		public Texture Texture { get; }

		public ModelAssets(Mesh mesh, Material material, Texture texture)
		{
			Mesh = mesh;
			Material = material;
			Texture = texture;
		}
	}
}
