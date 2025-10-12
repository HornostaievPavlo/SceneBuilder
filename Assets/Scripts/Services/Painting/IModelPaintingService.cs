using UnityEngine;

namespace Services.Painting
{
	public interface IModelPaintingService
	{
		void SetColor(Color color);
		void SetColorTint(float value);
		void SetTexture(Texture texture);
		void RestoreOriginalMaterial();
	}
}