using UnityEngine;

namespace Services.Painting
{
	public interface IModelPaintingService
	{
		void SetColorTint(float value);
		void CacheModelMaterials(GameObject target, bool isSelected);
	}
}