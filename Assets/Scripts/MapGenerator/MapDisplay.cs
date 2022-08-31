using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer			textureRenderer = null;
	public MeshFilter		meshFilter = null;
	public MeshRenderer		meshRenderer = null;

    public void DrawTexture(Texture2D texture)
	{
		// Plane에 그려준다.
		textureRenderer.sharedMaterial.mainTexture = texture;
		textureRenderer.transform.localScale = new Vector3(texture.width, 1.0f, texture.height);
	}


	public void DrawMesh(MeshData meshData, Texture2D texture)
	{
		meshFilter.sharedMesh = meshData.CreateMesh();
		meshRenderer.material.mainTexture = texture;
	}
}
