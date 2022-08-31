using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
	public static float[,] GenerateNoiseMap(int width, int height, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
	{
		float[,] map = new float[width, height];
		
		System.Random random = new System.Random(seed);
		Vector2[] octaveOffsets = new Vector2[octaves];

		for (int i = 0; i < octaves; ++i) {
			float offsetX = random.Next(-100000, 100000) + offset.x;
			float offsetY = random.Next(-100000, 100000) + offset.y;

			octaveOffsets[i] = new Vector2(offsetX, offsetY);
		}

		// 0�� ������ ��츦 �����Ѵ�.
		if(scale <= 0) {
			scale = 0.0001f;
		}

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		float halfWidth = width / 2.0f;
		float halfHeight = height / 2.0f;

		// ���� �����Ѵ�.
		for (int y = 0; y < height; ++y) {
			for (int x = 0; x < width; ++x) {

				float amplitude = 1.0f;
				float frequency = 1.0f;
				float noiseHeight = 0.0f;

				// ��Ÿ�긦 �����ϱ� ���ؼ� ��Ÿ�긶�� ����� ���ش�.
				// ���������� ��Ÿ�긦 ���� ���� ���� height���̴�.
				for (int i = 0; i < octaves; ++i) {
					// �����Ͽ� ���� x,y�� ���� �� �ֵ����Ѵ�.
					float tempX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
					float tempY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

					// ParlinNoise�� ����Ѵ�.
					float parlinValue = Mathf.PerlinNoise(tempX, tempY) * 2 - 1;
					noiseHeight += parlinValue * amplitude;

					// �� ��Ÿ�긶�� �����Ǵ� ���� ���� �� ������ �޶�����.
					amplitude *= persistance;		// ������ ���Ӽ����� �����ش�.
					frequency *= lacunarity;		// ������ ��ī�ο����� �����ش�.
				}
				// ������ ��Ÿ��Layer�� ���� �ٸ��� ���õǾ� ���������� �־����� ����̱⶧���� �Ϻ��ϰ� �и��� ��������� ��� ����� �ƴϴ�.

				// �ִ�, �ּ��� ���� �˾Ƴ���.
				if(noiseHeight > maxNoiseHeight) {
					maxNoiseHeight = noiseHeight;
				}
				else if (noiseHeight < minNoiseHeight) {
					minNoiseHeight = noiseHeight;
				}

				map[x, y] = noiseHeight;
			}
		}
		
		for (int y = 0; y < height; ++y) {
			for (int x = 0; x < width; ++x) {
				map[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, map[x, y]);
			}
		}

		return map;
	}
}

