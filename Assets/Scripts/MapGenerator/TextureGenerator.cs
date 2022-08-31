using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{
	public static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height)
	{
		Texture2D texture = new Texture2D(width, height);
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels(colorMap);
		texture.Apply();

		return texture;
	}

	public static Texture2D TextureFromHeightMap(float[,] heightMap)
	{
		// ���� ������ ���̸� ���´�.
		int width = heightMap.GetLength(0);
		int height = heightMap.GetLength(1);

		// �ؽ�ó�� �����ѵ� �ؽ�ó�� �ȼ��� ��´�.
		Texture2D texture = new Texture2D(width, height);
		Color[] colorMap = new Color[width * height];

		for (int i = 0; i < height; ++i) {
			for (int j = 0; j < width; ++j) {
				// NoiseMap�� �������̰� Black, White�� ������ �������� �Ѵ�.
				colorMap[i * width + j] = Color.Lerp(Color.black, Color.white, heightMap[j, i]);
			}
		}
		texture.SetPixels(colorMap);
		texture.Apply();

		return TextureFromColorMap(colorMap, width, height);
	}
}
