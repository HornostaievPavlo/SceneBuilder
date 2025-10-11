using System.Threading.Tasks;
using LocalSaves;
using UnityEngine;

namespace Services.Loading
{
	public interface ILoadService
	{
		Task<bool> LoadModel(string modelPath, string localSaveDirectoryPath = "");
		void LoadCamera();
		void LoadLabel();
		void LoadLocalSave(LocalSave localSave);
		Texture LoadTexture(string path);
	}
}