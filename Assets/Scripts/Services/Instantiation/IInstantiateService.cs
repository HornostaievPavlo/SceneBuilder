using UnityEngine;

namespace Services.Instantiation
{
	public interface IInstantiateService
	{
		T Instantiate<T>(GameObject prefab) where T : Component;
	}
}