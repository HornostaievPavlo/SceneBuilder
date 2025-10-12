using System;
using System.Collections.Generic;
using Plain;

namespace Services.LocalSavesRepository
{
	public interface ILocalSavesRepository
	{
		event Action<LocalSave> OnLocalSaveCreated;
		event Action<LocalSave> OnLocalSaveDeleted;
		void AddLocalSave(LocalSave localSave);
		void RemoveLocalSave(LocalSave localSave);
		List<LocalSave> GetLocalSaves();
	}
}