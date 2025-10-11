using System.Collections.Generic;
using LocalSaves;
using UnityEngine;

namespace Services.LocalSaves
{
	public interface ILocalSavesService
	{
		List<LocalSave> GetLocalSaves();
	}
}