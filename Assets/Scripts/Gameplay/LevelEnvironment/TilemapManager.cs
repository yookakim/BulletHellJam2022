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
		if (!terrainTilemap.HasTile(tilePosition))
		{
			Debug.LogError("Tried to remove nonexisting tile");
			return;
		}
		GameObject tileObjectToRemove = terrainTilemap.GetInstantiatedObject(tilePosition);
		

		StartCoroutine(DelayedDestroy(tileObjectToRemove, tilePosition));
		Destroy(tileObjectToRemove);




		// terrainTilemap.SetTile(tilePosition, null);
	}

	public void ReplaceTile(Vector3Int postion, TileBase replaceWith)
	{

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


		if (terrainTilemap.HasTile(new Vector3Int(tilePosition.x, tilePosition.y - 1, 0)))
		{
			GameObject tileOneBelowObject = terrainTilemap.GetInstantiatedObject(new Vector3Int(tilePosition.x, tilePosition.y - 1, 0));

			if (tileOneBelowObject != null)
			{
				TerrainTile tileOneBelow = tileOneBelowObject.GetComponent<TerrainTile>();
				StartCoroutine(DelayedReplaceTile(new Vector3Int(tilePosition.x, tilePosition.y - 1, 0), tileOneBelow.TallerColliderVersion));
				Destroy(tileOneBelow.gameObject);

			}
		}
	}

	private IEnumerator DelayedReplaceTile(Vector3Int position, TileBase tileToReplace)
	{
		yield return new WaitForEndOfFrame();
		terrainTilemap.SetTile(position, tileToReplace);
	}
}
