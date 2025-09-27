using System.Threading.Tasks;

namespace Services.Loading
{
	public interface ILoadService
	{
		/// <summary>
		///     Handles loading assets from local save file
		/// </summary>
		/// <param name="sceneNumber">Index of save file</param>
		void LoadAssetsFromSaveFile(int sceneNumber);

		/// <summary>
		///     General model loading procedure.
		///     Handles adding of colliders to models
		/// </summary>
		/// <param name="modelPath">Path to .glb file in local storage</param>
		/// <returns>Success of loading</returns>
		Task<bool> LoadModel(string modelPath);
	}
}