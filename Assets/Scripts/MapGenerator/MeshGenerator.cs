using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
	public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, int levelOfDetail)
	{
		int width = heightMap.GetLength(0);
		int height = heightMap.GetLength(1);

		// ���� LevelOfDetail�� 0�� ��� 1�� �ٲ��ش�.
		int meshSimplificationIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;
		int verticesPerLine = ((width - 1) / meshSimplificationIncrement) + 1;

		// ������ ũ���� �߰����� ���߱� ���� offset�� �����Ѵ�.
		float topLeftX = (width - 1) / -2.0f;
		float topLeftZ = (height - 1) / 2.0f;

		MeshData meshData = new MeshData(width, height);
		int vertexIndex = 0;

		// MeshData�� ���� �������ش�.
		for (int y = 0; y < height; y += meshSimplificationIncrement) {
			for (int x = 0; x < width; x += meshSimplificationIncrement) {
				meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);
				meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

				// �������� �����ص� �ȴ�.
				if (x < width - 1 && y < height - 1) {
					meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
					meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
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

	// �ﰢ���� �����ϴ� IndexBuffer�� ������
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