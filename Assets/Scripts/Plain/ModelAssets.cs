using UnityEngine;

namespace Plain
{
	public class ModelAssets
	{
		public Mesh Mesh { get; set; }
		public Material Material { get; set; }
		public Texture Texture { get; set; }

		public ModelAssets(Mesh mesh, Material material, Texture texture)
		{
			Mesh = mesh;
			Material = material;
			Texture = texture;
		}
	}
}
