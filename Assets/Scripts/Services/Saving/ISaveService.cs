using System.Threading.Tasks;

namespace Services.Saving
{
	public interface ISaveService
	{
		/// <summary>
		///     Saves all scene assets,
		///     creates save file row in UI
		/// </summary>
		Task SaveCurrentScene();
	}
}