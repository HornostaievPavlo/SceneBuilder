using System.Collections.Generic;
using System.IO;
using LocalSaves;
using UnityEngine;
using Zenject;

namespace Services.LocalSaves
{
	public class LocalSavesService : ILocalSavesService, IInitializable
	{
		private readonly List<LocalSave> _localSaves = new();

		public void Initialize()
		{
			RefreshLocalSaves();
		}

		public List<LocalSave> GetLocalSaves()
		{
			return _localSaves;
		}

		public Texture LoadTexture(string path)
		{
			byte[] loadedBytes = File.ReadAllBytes(path);

			Texture2D textureFromBytes = new Texture2D(2, 2);
			textureFromBytes.LoadImage(loadedBytes);

			return textureFromBytes;
		}

		private void RefreshLocalSaves()
		{
			List<string> directoryPaths = GetLocalSaveDirectoryPaths();
			
			foreach (string path in directoryPaths)
			{
				Texture texture = LoadTexture(path + Constants.PreviewFile);
				
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