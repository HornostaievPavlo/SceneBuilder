using System.Collections.Generic;
using System.IO;
using LocalSaves;
using Services.Loading;
using UnityEngine;
using Zenject;

namespace Services.LocalSavesRepository
{
	public class LocalSavesRepository : ILocalSavesRepository, IInitializable
	{
		private readonly List<LocalSave> _localSaves = new();
		
		private readonly ILoadService _loadService;

		public LocalSavesRepository(ILoadService loadService)
		{
			_loadService = loadService;
		}

		public void Initialize()
		{
			RefreshLocalSaves();
		}

		public List<LocalSave> GetLocalSaves()
		{
			return _localSaves;
		}

		private void RefreshLocalSaves()
		{
			List<string> directoryPaths = GetLocalSaveDirectoryPaths();
			
			foreach (string path in directoryPaths)
			{
				Texture texture = _loadService.LoadTexture(path + Constants.PreviewFile);
				
				var localSave = new LocalSave(path, texture);
				_localSaves.Add(localSave);
			}
		}

		private List<string> GetLocalSaveDirectoryPaths()
		{
			return new List<string>(Directory.EnumerateDirectories(Constants.DataPath));
		}
	}
}