using UnityEngine;

namespace Services.Instantiation
{
	public interface IInstantiateService
	{
		T Instantiate<T>(GameObject prefab, Transform parent = null) where T : Component;
	}
}