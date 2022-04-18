using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
	[SerializeField] private Tilemap terrainTilemap;
	[SerializeField] private Tilemap terrainRoofTilemap;

	private void Awake()
	{
		// terrainTilemap = GetComponent<Tilemap>();
	}

	public void RemoveTile(Vector3Int tilePosition)
	{
		GameObject tileObjectToRemove = terrainTilemap.GetInstantiatedObject(tilePosition);


		StartCoroutine(DelayedDestroy(tileObjectToRemove, tilePosition));
		Destroy(tileObjectToRemove);

		// terrainTilemap.SetTile(tilePosition, null);
	}

	public void RemoveTileBlock(BoundsInt boundsToRemove)
	{
		BoundsInt.PositionEnumerator positionsToRemove = boundsToRemove.allPositionsWithin;
		// StartCoroutine(DelayedDestroyBlock(boundsToRemove));
		foreach (Vector3Int position in positionsToRemove)
		{
			RemoveTile(position);
			// GameObject tileObjectToRemove = terrainTilemap.GetInstantiatedObject(position);
			// Destroy(tileObjectToRemove);
		}
	}

	private IEnumerator DelayedDestroy(GameObject objectToRemove, Vector3Int tilePosition)
	{
		yield return new WaitForEndOfFrame();
		// Destroy(objectToRemove);
		terrainTilemap.SetTile(tilePosition, null);
		terrainRoofTilemap.SetTile(tilePosition, null);
	}

	private IEnumerator DelayedDestroyBlock(BoundsInt boundsToRemove)
	{
		yield return new WaitForEndOfFrame();

		terrainTilemap.SetTilesBlock(boundsToRemove, null);
		terrainRoofTilemap.SetTilesBlock(boundsToRemove, null);
	}
}
