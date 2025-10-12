using System;
using System.Threading.Tasks;
using Plain;
using UnityEngine;

namespace Services.Loading
{
	public interface ILoadService
	{
		public event Action OnLocalSaveLoaded;
		Task<bool> LoadModel(string modelPath);
		void LoadCamera();
		void LoadLabel();
		void LoadLocalSave(LocalSave localSave);
		Texture LoadTexture(string path);
	}
}