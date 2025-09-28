using System.Threading.Tasks;

namespace Services.Loading
{
	public interface ILoadService
	{
		Task<bool> LoadModel(string modelPath);
		void LoadCamera();
		void LoadAssetsFromSaveFile(int sceneNumber);
	}
}