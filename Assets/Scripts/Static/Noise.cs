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

		// 0을 나누는 경우를 제외한다.
		if(scale <= 0) {
			scale = 0.0001f;
		}

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		float halfWidth = width / 2.0f;
		float halfHeight = height / 2.0f;

		// 맵을 셋팅한다.
		for (int y = 0; y < height; ++y) {
			for (int x = 0; x < width; ++x) {

				float amplitude = 1.0f;
				float frequency = 1.0f;
				float noiseHeight = 0.0f;

				// 옥타브를 조합하기 위해서 옥타브마다 계산을 해준다.
				// 최종적으로 옥타브를 전부 더한 값이 height값이다.
				for (int i = 0; i < octaves; ++i) {
					// 스케일에 따라서 x,y를 얻을 수 있도록한다.
					float tempX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
					float tempY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

					// ParlinNoise를 사용한다.
					float parlinValue = Mathf.PerlinNoise(tempX, tempY) * 2 - 1;
					noiseHeight += parlinValue * amplitude;

					// 각 옥타브마다 설정되는 값에 따라서 값 셋팅이 달라진다.
					amplitude *= persistance;		// 진폭을 지속성으로 곱해준다.
					frequency *= lacunarity;		// 진동을 날카로움으로 곱해준다.
				}
				// 각각의 옥타브Layer에 값이 다르게 셋팅되어 최종값으로 넣어지는 방식이기때문에 완벽하게 분리된 노이즈맵을 얻는 방식은 아니다.

				// 최대, 최소의 값을 알아낸다.
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

