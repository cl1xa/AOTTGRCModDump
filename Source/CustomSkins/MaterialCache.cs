using System.Collections.Generic;
using UnityEngine;

namespace CustomSkins;

internal class MaterialCache
{
	private static Dictionary<string, Material> _IdToMaterial = new Dictionary<string, Material>();

	public static Material TransparentMaterial;

	public static void Init()
	{
		TransparentMaterial = new Material(Shader.Find("Transparent/Diffuse"));
		Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, mipmap: false);
		texture2D.SetPixel(0, 0, new Color(0f, 0f, 0f, 0f));
		texture2D.Apply();
		TransparentMaterial.mainTexture = texture2D;
	}

	public static void Clear()
	{
		_IdToMaterial.Clear();
	}

	public static bool ContainsKey(string rendererId, string url)
	{
		return _IdToMaterial.ContainsKey(GetId(rendererId, url));
	}

	public static Material GetMaterial(string rendererId, string url)
	{
		return _IdToMaterial[GetId(rendererId, url)];
	}

	public static void SetMaterial(string rendererId, string url, Material material)
	{
		string id = GetId(rendererId, url);
		if (_IdToMaterial.ContainsKey(id))
		{
			_IdToMaterial[id] = material;
		}
		else
		{
			_IdToMaterial.Add(id, material);
		}
	}

	private static string GetId(string rendererId, string url)
	{
		return rendererId + "," + url;
	}
}
