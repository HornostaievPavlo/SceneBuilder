using System.Threading.Tasks;

namespace Services.Loading
{
	public interface ILoadService
	{
		Task<bool> LoadModel(string modelPath);
		void LoadAssetsFromSaveFile(int sceneNumber);
	}
}