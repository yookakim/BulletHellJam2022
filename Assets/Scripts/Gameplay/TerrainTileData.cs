using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TerrainTileData : Tile
{
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
