using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class DirtTile : Tile
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
	}
}
