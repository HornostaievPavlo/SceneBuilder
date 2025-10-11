using System.Threading.Tasks;
using LocalSaves;

namespace Services.Loading
{
	public interface ILoadService
	{
		Task<bool> LoadModel(string modelPath);
		void LoadCamera();
		void LoadLabel();
		void LoadLocalSave(LocalSave localSave);
	}
}