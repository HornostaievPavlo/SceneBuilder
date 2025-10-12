using UnityEngine;

namespace Gameplay
{
	public class ReadableTextureCopyInstantiator
	{
		public Texture2D CreateReadableTexture(Texture sourceTexture)
		{
			RenderTexture renderTex = RenderTexture.GetTemporary(
				sourceTexture.width,
				sourceTexture.height,
				0,
				RenderTextureFormat.Default,
				RenderTextureReadWrite.Linear);

			Graphics.Blit(sourceTexture, renderTex);
			RenderTexture previous = RenderTexture.active;
			RenderTexture.active = renderTex;
			Texture2D readableText = new Texture2D(sourceTexture.width, sourceTexture.height);
			readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
			readableText.Apply();
			RenderTexture.active = previous;
			RenderTexture.ReleaseTemporary(renderTex);
			return readableText;
		}
	}
}