using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
	public static MeshData GenerateTerrainMesh(float[,] heightMap)
	{
		int width = heightMap.GetLength(0);
		int height = heightMap.GetLength(1);

		// 정점을 크기의 중간으로 맞추기 위해 offset을 셋팅한다.
		float topLeftX = (width - 1) / -2.0f;
		float topLeftZ = (height - 1) / 2.0f;

		MeshData meshData = new MeshData(width, height);
		int vertexIndex = 0;

		// MeshData의 값을 셋팅해준다.
		for (int y = 0; y < height; ++y) {
			for (int x = 0; x < width; ++x) {
				meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightMap[x, y], topLeftZ - y);
				meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

				// 마지막은 무시해도 된다.
				if (x < width - 1 && y < height - 1) {
					meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
					meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
				}

				vertexIndex += 1;
			}
		}

		return meshData;
	}
}

public class MeshData
{
	public Vector3[]	vertices = null;
	public int[]		triangles = null;
	public Vector2[]	uvs = null;

	int					triangleIndex = 0;

	public MeshData(int width, int height)
	{
		vertices = new Vector3[width * height];
		uvs = new Vector2[width * height];
		triangles = new int[(width - 1) * (height - 1) * 6];

	}

	public void AddTriangle(int a, int b, int c)
	{
		triangles[triangleIndex] = a;
		triangles[triangleIndex + 1] = b;
		triangles[triangleIndex + 2] = c;

		triangleIndex += 3;
	}

	public Mesh CreateMesh()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();

		return mesh;
	}
}