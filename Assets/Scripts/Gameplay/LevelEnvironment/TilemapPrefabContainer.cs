using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapPrefabContainer : MonoBehaviour
{
    [SerializeField] Tilemap groundTiles;
    [SerializeField] Tilemap terrainTiles;
    [SerializeField] Tilemap roofTiles;

	public BoundsInt GetBoundsOfPrefabTilemap()
	{
		groundTiles.CompressBounds();
		terrainTiles.CompressBounds();
		roofTiles.CompressBounds();

		BoundsInt groundTilesBounds = groundTiles.cellBounds;
		BoundsInt terrainTilesBounds = terrainTiles.cellBounds;
		BoundsInt roofTilesBounds = roofTiles.cellBounds;

		int minX = Mathf.Min(groundTilesBounds.xMin, terrainTilesBounds.xMin, roofTilesBounds.xMin);
		int maxX = Mathf.Max(groundTilesBounds.xMax, terrainTilesBounds.xMax, roofTilesBounds.xMax);
		int minY = Mathf.Min(groundTilesBounds.yMin, terrainTilesBounds.yMin, roofTilesBounds.yMin);
		int maxY = Mathf.Max(groundTilesBounds.yMax, terrainTilesBounds.yMax, roofTilesBounds.yMax);

		BoundsInt newBounds = new BoundsInt(new Vector3Int(minX, minY, 0), new Vector3Int(maxX - minX, maxY - minY, 1));

		return newBounds;
	}

	public Tilemap GetGroundTilemap()
	{
		Tilemap tilemap = groundTiles;

		groundTiles.CompressBounds();
		return tilemap;
	}    
	public Tilemap GetTerrainTilemap()
	{
		Tilemap tilemap = terrainTiles;

		groundTiles.CompressBounds();
		return tilemap;
	}    
	public Tilemap GetRoofTilemap()
	{
		Tilemap tilemap = roofTiles;

		groundTiles.CompressBounds();
		return tilemap;
	}
}
