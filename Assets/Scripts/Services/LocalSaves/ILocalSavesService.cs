using System.Collections.Generic;
using LocalSaves;

namespace Services.LocalSaves
{
	public interface ILocalSavesService
	{
		List<LocalSave> GetLocalSaves();
	}
}