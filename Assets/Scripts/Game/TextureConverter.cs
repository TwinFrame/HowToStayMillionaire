using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class TextureConverter : MonoBehaviour
{
	//[SerializeField] private int _width;
	//[SerializeField] private int _height;

	//private int _currentWidth;
	//[SerializeField] int _currentHeight;

	public Texture2D GetTexture2DFromBytes(byte[] bytes, int width, int height)
	{
		Texture2D tex2D = new Texture2D(width, height, TextureFormat.ARGB32, mipChain: false, linear: false);

		//tex2D.LoadImage(bytes);
		tex2D.LoadRawTextureData(bytes);

		//tex2D.LoadRawTextureData(bytes);
		tex2D.Apply();

		return tex2D;
	}

	public byte[] GetRawTextureDataFromRenderTexture(RenderTexture renderTexture, int width, int height)
	{
		if (renderTexture == null || !renderTexture.IsCreated())
			return null;

		// Allocate
		var sRgbRenderTex = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32,
			RenderTextureReadWrite.sRGB);
		var tex = new Texture2D(width, height, TextureFormat.ARGB32, mipChain: false, linear: false);

		// Linear to Gamma Conversion
		Graphics.Blit(renderTexture, sRgbRenderTex);

		// Copy memory from RenderTexture
		var tmp = RenderTexture.active;
		RenderTexture.active = sRgbRenderTex;
		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		tex.Apply();
		RenderTexture.active = tmp;

		// Get PNG bytes
		//byte[] bytes = tex.EncodeToPNG();

		byte[] bytes = tex.GetRawTextureData();

		// Destroy
		Destroy(tex);
		RenderTexture.ReleaseTemporary(sRgbRenderTex);

		return bytes;
	}
}
