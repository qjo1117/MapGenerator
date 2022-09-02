using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorMap, MeshMap };
    [Header("Render Mode")]
    public DrawMode drawMode = DrawMode.NoiseMap;

    [Header("기본설정")]
    public const int       mapChunckSize = 241;
    [Range(0,6)]
    public int      levelOfDetail = 1;
    public float    noiseScale = 0.0f;

    [Header("세부설정")]
    public int      octaves = 0;
    [Range(0.0f, 1.0f)]
    public float    persistance = 0.0f;
    public float    lacunarity = 0.0f;

    [Header("시드")]
    public int      seed = 0;
    public Vector2  offset = Vector2.zero;

    [Header("HeightMultiplier")]
    public float            meshHeightMultiplier = 1.0f;
    public AnimationCurve   meshHeightCurve;

    [Header("자동업데이트")]
    // 자동 업데이트를 시킬것인가?
    public bool isUpdated = false;

    // 그려질 색깔의 구역을 나눈다.
    public TerrainType[] regions;

    public void GeneratorMap()
	{
        // NoiseMap 생성
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunckSize, mapChunckSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if(display == null) {
            return;
		}
        
        // Mode에 따른 Map생성
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

                // 구역을 검사해서 해당하는 색깔을 지정해준다.
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
    // 각 높이의 따라서 그려지는 색깔을 다르게 지정한다.
    public string name = "Terrain";
    public float height = 0.0f;             
    public Color color = Color.black;
}