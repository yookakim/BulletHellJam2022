using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class DirtTileData : Tile
{
	/*	// public GameObject gameObject;
		public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
		{
			// tileData.gameObject = 
			// gameObject = 
		}*/

	// [SerializeField] public GameObject gameObject;

	// [SerializeField] private GameObject dirtTilePrefab;

	public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
	{
		base.GetTileData(position, tilemap, ref tileData);
		// tileData.gameObject = dirtTilePrefab;

		// Debug.Log("watafak, " + position);

		// TerrainTile thisTile = tileData.gameObject.GetComponent<TerrainTile>();
		// Debug.Log("Tilemap manager: " + tilemap.GetComponent<Transform>().GetComponentInParent<TilemapManager>());
		// thisTile.TilemapManager = tilemap.GetComponent<Transform>().GetComponentInParent<TilemapManager>();
	}

	public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
	{
		if (go != null)
		{
			TerrainTile thisTile = go.GetComponent<TerrainTile>();
			thisTile.TilePosition = position;
			thisTile.TilemapManager = tilemap.GetComponent<Transform>().GetComponentInParent<TilemapManager>();
		}
		return true;
	}
}
