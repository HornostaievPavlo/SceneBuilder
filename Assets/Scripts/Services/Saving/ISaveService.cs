using System.Threading.Tasks;
using UnityEngine;

namespace Services.Saving
{
	public interface ISaveService
	{
		Task SaveScene(Texture2D preview);
	}
}