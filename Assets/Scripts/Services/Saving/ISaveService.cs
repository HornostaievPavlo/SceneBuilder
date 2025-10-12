using System.Threading.Tasks;
using Plain;
using UnityEngine;

namespace Services.Saving
{
	public interface ISaveService
	{
		Task CreateLocalSave(Texture2D preview);
		void DeleteLocalSave(LocalSave localSave);
	}
}