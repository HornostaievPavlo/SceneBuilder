using System.Collections.Generic;
using LocalSaves;

namespace Services.LocalSavesRepository
{
	public interface ILocalSavesRepository
	{
		List<LocalSave> GetLocalSaves();
	}
}