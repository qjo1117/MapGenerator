using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorMap, MeshMap };
    public DrawMode drawMode = DrawMode.NoiseMap;

    [Header("�⺻����")]
    public int      width = 0;
    public int      height = 0;
    public float    noiseScale = 0.0f;

    [Header("���μ���")]
    public int      octaves = 0;
    [Range(0.0f, 1.0f)]
    public float    persistance = 0.0f;
    public float    lacunarity = 0.0f;

    [Header("�õ�")]
    public int      seed = 0;
    public Vector2  offset = Vector2.zero;

    [Header("�ڵ�������Ʈ")]
    // �ڵ� ������Ʈ�� ��ų���ΰ�?
    public bool isUpdated = false;

    // �׷��� ������ ������ ������.
    public TerrainType[] regions;

    public void GeneratorMap()
	{
        // NoiseMap ����
        float[,] noiseMap = Noise.GenerateNoiseMap(width, height, seed, noiseScale, octaves, persistance, lacunarity, offset);

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if(display == null) {
            return;
		}
        
        // Mode�� ���� Map����
        switch(drawMode) {
            case DrawMode.NoiseMap:
                display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
                break;
            case DrawMode.ColorMap: {
                    Color[] colorMap = GeneratorColorMap(noiseMap);
                    display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, width, height));
                }
                break;
            case DrawMode.MeshMap: {
                    Color[] colorMap = GeneratorColorMap(noiseMap);
                    display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap), TextureGenerator.TextureFromColorMap(colorMap, width, height));
                }
                break;

        }

	}

    Color[] GeneratorColorMap(float[,] noiseMap)
	{
        int regionSize = regions.Length;
        Color[] colorMap = new Color[width * height];
        for (int y = 0; y < height; ++y) {
            for (int x = 0; x < width; ++x) {
                float currentHeight = noiseMap[x, y];

                // ������ �˻��ؼ� �ش��ϴ� ������ �������ش�.
                for (int i = 0; i < regionSize; ++i) {
                    if (currentHeight <= regions[i].height) {
                        colorMap[y * width + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        return colorMap;
    }

	private void OnValidate()
	{
		if(width < 1) {
            width = 1;
		}
        if(height < 1) {
            height = 1;
		}
        if(lacunarity < 1) {
            lacunarity = 1;
        }
        if(octaves < 0) {
            octaves = 0;
        }
	}
}


[System.Serializable]
public class TerrainType
{
    // �� ������ ���� �׷����� ������ �ٸ��� �����Ѵ�.
    public string name = "Terrain";
    public float height = 0.0f;             
    public Color color = Color.black;
}