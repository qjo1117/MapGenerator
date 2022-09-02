using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class EndLessTerrain : MonoBehaviour
{
    public const float maxViewDistance = 300.0f;
    public Transform viewer;

    public static Vector2 viewerPosition = Vector2.zero;
    private int chunckSize = MapGenerator.mapChunckSize - 1;
    private int chunckVisibleInViewDistance = 1;

    private Dictionary<Vector2, TerrainChunck> dicTerrainChunck = new Dictionary<Vector2, TerrainChunck>();

	private void Start()
	{
        chunckSize = MapGenerator.mapChunckSize - 1;
        chunckVisibleInViewDistance = Mathf.RoundToInt(maxViewDistance / chunckSize);
	}

    private void UpdateVisibleChuncks()
	{
        int currentChunckCoordX = Mathf.RoundToInt(viewerPosition.x / chunckSize);
        int currentChunckCoordY = Mathf.RoundToInt(viewerPosition.y / chunckSize);

        for (int yOffset = -chunckVisibleInViewDistance; yOffset <= chunckVisibleInViewDistance; ++yOffset) {
            for (int xOffset = -chunckVisibleInViewDistance; xOffset <= chunckVisibleInViewDistance; ++xOffset) {
                Vector2 viewedChunckCoord = new Vector2(currentChunckCoordX + xOffset, currentChunckCoordY + yOffset);

                if(dicTerrainChunck.ContainsKey(viewedChunckCoord) == true) {
                    dicTerrainChunck[viewedChunckCoord].UpdateTerrainChunck();
                }
                else {
                    dicTerrainChunck.Add(viewedChunckCoord, new TerrainChunck(viewedChunckCoord, chunckSize));
				}
            }
        }
	}

    public class TerrainChunck
	{
        private GameObject meshObject = null;
        private Vector2 position = Vector2.zero;
        private Bounds bounds;

        public TerrainChunck(Vector2 coord, int size)
		{
            position = coord * size;
            Vector3 vPosition = new Vector3(position.x, 0.0f, position.y);

            bounds = new Bounds(position, Vector2.one * size);

            meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            meshObject.transform.position = vPosition;
            meshObject.transform.localScale = (Vector3.one * size) / 10.0f;

            SetVisible(false);
        }

        public void UpdateTerrainChunck()
		{
            float viewerDistanceFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
            bool visible = viewerDistanceFromNearestEdge <= maxViewDistance;

            SetVisible(visible);
        }

        public void SetVisible(bool visible)
		{
            meshObject.SetActive(visible);
		}
	}
}
