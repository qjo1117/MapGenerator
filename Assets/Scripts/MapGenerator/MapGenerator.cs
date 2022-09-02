using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorMap, MeshMap };
    [Header("Render Mode")]
    public DrawMode drawMode = DrawMode.NoiseMap;

    [Header("�⺻����")]
    public const int       mapChunckSize = 241;
    [Range(0,6)]
    public int      levelOfDetail = 1;
    public float    noiseScale = 0.0f;

    [Header("���μ���")]
    public int      octaves = 0;
    [Range(0.0f, 1.0f)]
    public float    persistance = 0.0f;
    public float    lacunarity = 0.0f;

    [Header("�õ�")]
    public int      seed = 0;
    public Vector2  offset = Vector2.zero;

    [Header("HeightMultiplier")]
    public float            meshHeightMultiplier = 1.0f;
    public AnimationCurve   meshHeightCurve;

    [Header("�ڵ�������Ʈ")]
    // �ڵ� ������Ʈ�� ��ų���ΰ�?
    public bool isUpdated = false;

    // �׷��� ������ ������ ������.
    public TerrainType[] regions;

    public void GeneratorMap()
	{
        // NoiseMap ����
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunckSize, mapChunckSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

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
                    display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapChunckSize, mapChunckSize));
                }
                break;
            case DrawMode.MeshMap: {
                    Color[] colorMap = GeneratorColorMap(noiseMap);
                    display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColorMap(colorMap, mapChunckSize, mapChunckSize));
                }
                break;

        }

	}

    Color[] GeneratorColorMap(float[,] noiseMap)
	{
        int regionSize = regions.Length;
        Color[] colorMap = new Color[mapChunckSize * mapChunckSize];
        for (int y = 0; y < mapChunckSize; ++y) {
            for (int x = 0; x < mapChunckSize; ++x) {
                float currentHeight = noiseMap[x, y];

                // ������ �˻��ؼ� �ش��ϴ� ������ �������ش�.
                for (int i = 0; i < regionSize; ++i) {
                    if (currentHeight <= regions[i].height) {
                        colorMap[y * mapChunckSize + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        return colorMap;
    }

	private void OnValidate()
	{
        if(lacunarity < 1) {
            lacunarity = 1;
        }
        if(octaves < 0) {
            octaves = 0;
        }

        if(meshHeightMultiplier < 0.0f) {
            meshHeightMultiplier = 0.001f;
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