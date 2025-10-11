using System;
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
		
		public event Action<LocalSave> OnLocalSaveCreated;
		public event Action<LocalSave> OnLocalSaveDeleted;

		public LocalSavesRepository(ILoadService loadService)
		{
			_loadService = loadService;
		}

		public void Initialize()
		{
			RefreshLocalSaves();
		}

		public void AddLocalSave(LocalSave localSave)
		{
			if (_localSaves.Contains(localSave))
				return;
			
			_localSaves.Add(localSave);
			OnLocalSaveCreated?.Invoke(localSave);
		}

		public void RemoveLocalSave(LocalSave localSave)
		{
			if (_localSaves.Contains(localSave) == false)
				return;
			
			RefreshLocalSaves();
			OnLocalSaveDeleted?.Invoke(localSave);
		}

		public List<LocalSave> GetLocalSaves()
		{
			return _localSaves;
		}

		private void RefreshLocalSaves()
		{
			_localSaves.Clear();
			
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
			return new List<string>(Directory.EnumerateDirectories(Constants.ApplicationDataPath));
		}
	}
}