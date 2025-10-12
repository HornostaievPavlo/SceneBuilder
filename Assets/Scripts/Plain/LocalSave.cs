using UnityEngine;

namespace Plain
{
	public class LocalSave
	{
		public string DirectoryPath { get; private set; }
		public Texture Preview { get; private set; }
		
		public LocalSave(string directoryPath, Texture preview)
		{
			DirectoryPath = directoryPath;
			Preview = preview;
		}
	}
}